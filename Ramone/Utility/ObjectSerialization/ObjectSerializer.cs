using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using Ramone.IO;


namespace Ramone.Utility.ObjectSerialization
{
  public class ObjectSerializer
  {
    protected Type DataType { get; set; }

    protected IPropertyVisitor Visitor { get; set; }

    protected ObjectSerializerSettings Settings { get; set; }


    public ObjectSerializer(Type t)
    {
      DataType = t;
    }


    public void Serialize(object data, IPropertyVisitor visitor, ObjectSerializerSettings settings = null)
    {
      Visitor = visitor;
      Settings = settings ?? new ObjectSerializerSettings();

      if (data != null && data.GetType() != DataType)
        throw new ArgumentException(string.Format("Cannot serialize {0} - expected {1}.", data.GetType(), DataType), "data");

      visitor.Begin();
      Serialize(data, DataType, "");
      visitor.End();
    }


    public object Deserialize(NameValueCollection values, ObjectSerializerSettings settings = null)
    {
      if (DataType == typeof(NameValueCollection))
        return values;

      Settings = settings ?? new ObjectSerializerSettings();

      object result = Activator.CreateInstance(DataType);
      foreach (string key in values.AllKeys)
      {
        if (key != null)
        {
          string value = values[key];
          IEnumerator propertyNames = key.Split(Settings.PropertySeparator).GetEnumerator();
          propertyNames.MoveNext();
          Evaluate(result, DataType, propertyNames, value);
        }
      }
      return result;
    }


    #region Serialization internals

    protected void Serialize(object data, Type dataType, string prefix)
    {
      IObjectSerializerFormater formater = Settings.Formaters.GetFormater(dataType);
      if (formater != null)
      {
        data = formater.Format(data);
        dataType = typeof(string);
      }

      if (data == null)
        return;

      if (typeof(IDictionary).IsAssignableFrom(dataType))
        SerializeDictionary((IDictionary)data, dataType, prefix);
      else if (typeof(NameValueCollection).IsAssignableFrom(dataType))
        SerializeNameValueCollection((NameValueCollection)data, dataType, prefix);
      else if (typeof(IList).IsAssignableFrom(dataType))
        SerializeList((IList)data, dataType, prefix);
      else if (typeof(IFile).IsAssignableFrom(dataType))
        SerializeFile((IFile)data, dataType, prefix);
      else if (IsSimpleType(dataType))
        SerializeSimpleValue(data, dataType, prefix);
      else
        SerializeProperties(data, dataType, prefix);
    }


    protected bool IsSimpleType(Type t)
    {
      return (!t.IsClass || t == typeof(string));
    }


    protected void SerializeSimpleValue(object data, Type dataType, string prefix)
    {
      string name = prefix;
      
      string formatedValue = "";
      if (data is DateTime)
        formatedValue = ((DateTime)data).ToString(Settings.DateTimeFormat);
      else if (data is decimal)
        formatedValue = ((decimal)data).ToString(Settings.Culture.NumberFormat);
      else if (data is float)
        formatedValue = ((float)data).ToString(Settings.Culture.NumberFormat);
      else if (data is double)
        formatedValue = ((double)data).ToString(Settings.Culture.NumberFormat);
      else if (data != null)
        formatedValue = data.ToString();
      
      Visitor.SimpleValue(name, data, formatedValue);
    }


    protected void SerializeProperties(object data, Type dataType, string prefix)
    {
      foreach (PropertyInfo p in dataType.GetProperties())
      {
        string propertyName = prefix != string.Empty 
                              ? string.Format(Settings.PropertyFormat, prefix, p.Name)
                              : p.Name;
        object propertyValue = (data != null ? p.GetValue(data, null) : null);

        Serialize(propertyValue, p.PropertyType, propertyName);
      }
    }


    protected void SerializeDictionary(IDictionary dict, Type dataType, string prefix)
    {
      foreach (DictionaryEntry entry in dict)
      {
        string name = prefix != string.Empty
                      ? string.Format(Settings.DictionaryFormat, prefix, entry.Key)
                      : entry.Key.ToString();
        Serialize(entry.Value, entry.Value != null ? entry.Value.GetType() : null, name);
      }
    }


    protected void SerializeNameValueCollection(NameValueCollection collection, Type dataType, string prefix)
    {
      foreach (string name in collection)
      {
        string prefixedName = prefix != string.Empty
                              ? string.Format(Settings.DictionaryFormat, prefix, name)
                              : name;
        string value = collection[name];
        Serialize(value, value != null ? value.GetType() : null, prefixedName);
      }
    }


    protected void SerializeList(IList collection, Type dataType, string prefix)
    {
      for (int i=0; i<collection.Count; ++i)
      {
        string name = string.Format(Settings.ArrayFormat, prefix, i);
        Serialize(collection[i], collection[i] != null ? collection[i].GetType() : null, name);
      }
    }


    protected void SerializeFile(IFile file, Type dataType, string prefix)
    {
      Visitor.File(file, prefix);
    }

    #endregion


    #region Deserialization internals

    protected void Evaluate(object classValue, Type t, IEnumerator propertyNames, string value)
    {
      string propertyName = (string)propertyNames.Current;

      if (typeof(Hashtable).IsAssignableFrom(t))
      {
        Hashtable d = (Hashtable)classValue;
        if (d != null)
        {
          if (propertyNames.MoveNext())
          {
            if (!d.ContainsKey(propertyName))
            {
              Hashtable nextD = new Hashtable();
              d[propertyName] = nextD;
            }
            Evaluate(d[propertyName], typeof(Hashtable), propertyNames, value);
          }
          else
          {
            d[propertyName] = value;
          }
        }
      }
      else if (typeof(NameValueCollection).IsAssignableFrom(t))
      {
        NameValueCollection d = (NameValueCollection)classValue;
        if (d != null)
        {
          string fullName = propertyName;
          while (propertyNames.MoveNext())
            fullName += Settings.PropertySeparator + (string)propertyNames.Current;
          d[fullName] = value;
        }
      }
      else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>))
      //else if (typeof(Dictionary<,>).IsAssignableFrom(t))
      {
        Type[] dictionaryArgTypes = t.GetGenericArguments();
        bool isSimpleType;
        object p = EvaluateProperty(classValue, dictionaryArgTypes[1], () => ((IDictionary)classValue)[propertyName], value, out isSimpleType);

        if (isSimpleType)
        {
          string fullName = propertyName;
          while (propertyNames.MoveNext())
            fullName += Settings.PropertySeparator + (string)propertyNames.Current;
          ((IDictionary)classValue)[fullName] = p;
        }
        else
        {
          IDictionary d = (IDictionary)classValue;
          if (propertyNames.MoveNext())
          {
            if (!d.Contains(propertyName))
            {
              d[propertyName] = p;
            }
            Evaluate(d[propertyName], dictionaryArgTypes[1], propertyNames, value);
          }
          else
          {
            d[propertyName] = p;
          }
        }
      }
      else if (typeof(IDictionary).IsAssignableFrom(t))
      {
        IDictionary d = (IDictionary)classValue;
        if (d != null)
        {
          d[propertyName] = value;
        }
      }
      else
      {
        PropertyInfo property = t.GetProperty(propertyName);

        if (property != null)
        {
          bool IsSimpleType;
          object propertyValue = EvaluateProperty(classValue, property.PropertyType, () => property.GetValue(classValue, new object[] { }), value, out IsSimpleType);
          property.SetValue(classValue, propertyValue, new object[] { });

          if (propertyNames.MoveNext())
          {
            Evaluate(propertyValue, property.PropertyType, propertyNames, value);
          }
        }
      }
    }


    protected object EvaluateProperty(object classValue, Type propertyType, Func<object> propertyAccessor, string value, out bool IsSimpleType)
    {
      IsSimpleType = true;
      if (propertyType == typeof(int))
      {
        int i;
        int.TryParse(value, out i);
        return i;
      }
      else if (propertyType == typeof(DateTime))
      {
        DateTime d;
        DateTime.TryParse(value, out d);
        return d;
      }
      else if (propertyType == typeof(float))
      {
        float f;
        float.TryParse(value, System.Globalization.NumberStyles.Float, Settings.Culture.NumberFormat, out f);
        return f;
      }
      else if (propertyType == typeof(double))
      {
        Double d;
        double.TryParse(value, System.Globalization.NumberStyles.Float, Settings.Culture.NumberFormat, out d);
        return d;
      }
      else if (propertyType == typeof(decimal))
      {
        Decimal d;
        Decimal.TryParse(value, System.Globalization.NumberStyles.Float, Settings.Culture.NumberFormat, out d);
        return d;
      }
      else if (propertyType == typeof(string))
      {
        return value;
      }
      else
      {
        object propertyValue = propertyAccessor();

        if (propertyValue == null)
        {
          propertyValue = Activator.CreateInstance(propertyType);
        }

        IsSimpleType = false;
        return propertyValue;
      }
    }

    #endregion
  }
}

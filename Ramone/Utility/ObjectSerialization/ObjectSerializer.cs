using System;
using System.Collections;
using System.Reflection;
using System.Collections.Specialized;
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
      PropertyInfo property = t.GetProperty(propertyName);

      if (property != null)
      {
        object propertyValue = EvaluateProperty(classValue, property, value);
        property.SetValue(classValue, propertyValue, new object[] { });

        if (propertyNames.MoveNext())
        {
          Evaluate(propertyValue, property.PropertyType, propertyNames, value);
        }
      }
    }


    protected object EvaluateProperty(object classValue, PropertyInfo property, string value)
    {
      if (property.PropertyType == typeof(int))
      {
        int i;
        int.TryParse(value, out i);
        return i;
      }
      else if (property.PropertyType == typeof(DateTime))
      {
        DateTime d;
        DateTime.TryParse(value, out d);
        return d;
      }
      else if (property.PropertyType == typeof(float))
      {
        float f;
        float.TryParse(value, System.Globalization.NumberStyles.Float, Settings.Culture.NumberFormat, out f);
        return f;
      }
      else if (property.PropertyType == typeof(double))
      {
        Double d;
        double.TryParse(value, System.Globalization.NumberStyles.Float, Settings.Culture.NumberFormat, out d);
        return d;
      }
      else if (property.PropertyType == typeof(decimal))
      {
        Decimal d;
        Decimal.TryParse(value, System.Globalization.NumberStyles.Float, Settings.Culture.NumberFormat, out d);
        return d;
      }
      else if (property.PropertyType == typeof(string))
      {
        return value;
      }
      else
      {
        object propertyValue = property.GetValue(classValue, new object[] { });

        if (propertyValue == null)
        {
          propertyValue = Activator.CreateInstance(property.PropertyType);
        }

        return propertyValue;
      }
    }

    #endregion
  }
}

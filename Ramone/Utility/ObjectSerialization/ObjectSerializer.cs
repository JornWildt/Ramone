using System;
using System.Collections;
using System.Reflection;


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


    protected void Serialize(object data, Type dataType, string prefix)
    {
      IObjectSerializerFormater formater = Settings.Formaters.GetFormater(dataType);
      if (formater != null)
      {
        data = formater.Format(data);
        dataType = typeof(string);
      }

      if (data == null)
        SerializeSimpleValue(data, dataType, prefix);
      else if (typeof(IDictionary).IsAssignableFrom(dataType))
        SerializeDictionary((IDictionary)data, dataType, prefix);
      else if (typeof(IList).IsAssignableFrom(dataType))
        SerializeList((IList)data, dataType, prefix);
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
      Visitor.SimpleValue(name, data);
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


    protected void SerializeList(IList collection, Type dataType, string prefix)
    {
      for (int i=0; i<collection.Count; ++i)
      {
        string name = string.Format(Settings.ArrayFormat, prefix, i);
        Serialize(collection[i], collection[i] != null ? collection[i].GetType() : null, name);
      }
    }
  }
}

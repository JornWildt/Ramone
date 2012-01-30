using System;
using System.Reflection;
using System.Collections;


namespace Ramone.Utility
{
  public interface IPropertyVisitor
  {
    void Begin();
    void SimpleValue(string name, object value);
    void End();
  }


  public class ObjectSerializer
  {
    protected Type DataType { get; set; }

    protected IPropertyVisitor Visitor { get; set; }


    public ObjectSerializer(Type t)
    {
      DataType = t;
    }


    public void Serialize(object data, IPropertyVisitor visitor)
    {
      Visitor = visitor;

      if (data != null && data.GetType() != DataType)
        throw new ArgumentException(string.Format("Cannot serialize {0} - expected {1}.", data.GetType(), DataType), "data");

      visitor.Begin();
      Serialize(data, DataType, "");
      visitor.End();
    }


    protected void Serialize(object data, Type dataType, string prefix)
    {
      if (data is IDictionary)
        SerializeDictionary((IDictionary)data, dataType, prefix);
      else if (data is IList)
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
      if (prefix != string.Empty)
        prefix = prefix + ".";

      foreach (PropertyInfo p in dataType.GetProperties())
      {
        string propertyName = prefix + p.Name;
        object propertyValue = (data != null ? p.GetValue(data, null) : null);

        Serialize(propertyValue, p.PropertyType, propertyName);
      }
    }


    protected void SerializeDictionary(IDictionary dict, Type dataType, string prefix)
    {
      foreach (DictionaryEntry entry in dict)
      {
        string name = string.Format("{0}[{1}]", prefix, entry.Key);
        Serialize(entry.Value, entry.Value != null ? entry.Value.GetType() : null, name);
      }
    }


    protected void SerializeList(IList collection, Type dataType, string prefix)
    {
      for (int i=0; i<collection.Count; ++i)
      {
        string name = string.Format("{0}[{1}]", prefix, i);
        Serialize(collection[i], collection[i] != null ? collection[i].GetType() : null, name);
      }
    }
  }
}

using System;
using System.Reflection;


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

      Serialize(data, DataType, "");
    }


    protected void Serialize(object data, Type dataType, string prefix)
    {
      foreach (PropertyInfo p in dataType.GetProperties())
      {
        string propertyName = p.Name;
        object propertyValue = (data != null ? p.GetValue(data, null) : null);

        Visitor.SimpleValue(propertyName, propertyValue);
      }
    }
  }
}

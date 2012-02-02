using System;


namespace Ramone.Utility.ObjectSerialization
{
  public interface IObjectSerializerFormaterManager
  {
    void AddFormater(Type t, IObjectSerializerFormater converter);
    void AddFormater<T>(IObjectSerializerFormater converter);
    void AddFormater<T>(Func<T,string> formater);
    IObjectSerializerFormater GetFormater(Type t);
    IObjectSerializerFormaterManager Clone();
  }
}

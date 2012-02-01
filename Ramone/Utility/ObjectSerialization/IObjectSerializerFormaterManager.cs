using System;


namespace Ramone.Utility.ObjectSerialization
{
  public interface IObjectSerializerFormaterManager
  {
    void AddFormater(Type t, IObjectSerializerFormater converter);
    IObjectSerializerFormater GetFormater(Type t);
  }
}

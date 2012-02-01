using System;
using System.Collections.Generic;


namespace Ramone.Utility.ObjectSerialization
{
  public class ObjectSerializerFormaterManager : IObjectSerializerFormaterManager
  {
    protected Dictionary<Type, IObjectSerializerFormater> Formaters = new Dictionary<Type, IObjectSerializerFormater>();


    #region IObjectSerializerFormaterManager Members

    public void AddFormater(Type t, IObjectSerializerFormater formater)
    {
      Formaters[t] = formater;
    }


    public IObjectSerializerFormater GetFormater(Type t)
    {
      if (t == null)
        return null;

      IObjectSerializerFormater f;
      Formaters.TryGetValue(t, out f);
      return f;
    }


    public IObjectSerializerFormaterManager Clone()
    {
      return new ObjectSerializerFormaterManager(this);
    }

    #endregion


    public ObjectSerializerFormaterManager()
    {
    }


    public ObjectSerializerFormaterManager(ObjectSerializerFormaterManager src)
    {
      Formaters = new Dictionary<Type, IObjectSerializerFormater>(src.Formaters);
    }
  }
}

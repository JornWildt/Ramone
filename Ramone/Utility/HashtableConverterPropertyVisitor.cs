using System;
using System.Collections.Generic;
using Ramone.IO;
using Ramone.Utility.ObjectSerialization;
using System.Collections;


namespace Ramone.Utility
{
  public class HashtableConverterPropertyVisitor : IPropertyVisitor
  {
    public Hashtable Result { get; set; }

    
    #region IPropertyVisitor Members

    public void Begin()
    {
      Result = new Hashtable();
    }


    public void SimpleValue(string name, object value, string formatedValue)
    {
      if (value != null)
        Result[name] = value;
    }


    public void File(IFile file, string name)
    {
      Result[name] = file;
    }


    public void End()
    {
    }

    #endregion
  }
}

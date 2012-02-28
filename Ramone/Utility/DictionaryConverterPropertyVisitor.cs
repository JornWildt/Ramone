using System;
using System.Collections.Generic;
using Ramone.IO;
using Ramone.Utility.ObjectSerialization;


namespace Ramone.Utility
{
  public class DictionaryConverterPropertyVisitor : IPropertyVisitor
  {
    public Dictionary<string, string> Result { get; set; }

    
    #region IPropertyVisitor Members

    public void Begin()
    {
      Result = new Dictionary<string, string>();
    }


    public void SimpleValue(string name, object value, string formatedValue)
    {
      Result[name] = formatedValue;
    }


    public void File(IFile file, string name)
    {
      throw new InvalidOperationException(string.Format("Cannot serialize Ramone IFile '{0}' to dictionary.", name));
    }


    public void End()
    {
    }

    #endregion
  }
}

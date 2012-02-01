using System.Collections.Generic;
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

    
    public void SimpleValue(string name, object value)
    {
      string s = (value != null ? value.ToString() : "");
      Result[name] = s;
    }


    public void End()
    {
    }

    #endregion
  }
}

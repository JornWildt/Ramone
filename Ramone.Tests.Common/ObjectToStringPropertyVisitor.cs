using System;
using Ramone.IO;
using Ramone.Utility.ObjectSerialization;


namespace Ramone.Tests.Common
{
  public class ObjectToStringPropertyVisitor : IPropertyVisitor
  {
    public string Result;


    #region IPropertyVisitor Members

    public void Begin()
    {
      Result = "";
    }

    public void SimpleValue(string name, object value, string formatedValue)
    {
      Result += string.Format("|{0}={1}", name, formatedValue);
    }


    public void File(IFile file, string name)
    {
      throw new InvalidOperationException(string.Format("Cannot serialize Ramone IFile '{0}' to string.", name));
    }

    public void End()
    {
    }

    #endregion
  }
}

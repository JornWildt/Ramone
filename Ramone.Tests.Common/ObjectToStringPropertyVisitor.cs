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

    public void SimpleValue(string name, object value)
    {
      Result += string.Format("|{0}={1}", name, value);
    }

    public void End()
    {
    }

    #endregion
  }
}

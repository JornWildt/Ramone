using System.IO;
using System.Web;
using Ramone.Utility.ObjectSerialization;


namespace Ramone.Utility
{
  public class FormUrlEncodingPropertyVisitor : IPropertyVisitor
  {
    protected TextWriter Writer;
    protected bool FirstValue = true;


    public FormUrlEncodingPropertyVisitor(TextWriter writer)
    {
      Writer = writer;
    }


    #region IPropertyVisitor

    public void Begin()
    {
    }


    public void SimpleValue(string name, object value, string formatedValue)
    {
      if (!FirstValue)
        Writer.Write("&");
      string output = HttpUtility.UrlEncode(name, Writer.Encoding) + "=" + HttpUtility.UrlEncode(formatedValue, Writer.Encoding);
      Writer.Write(output);
      FirstValue = false;
    }


    public void End()
    {
      Writer.Flush();
    }

    #endregion
  }
}

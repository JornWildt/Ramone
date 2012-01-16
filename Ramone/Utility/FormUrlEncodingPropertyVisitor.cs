using System.IO;
using System.Web;


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


    public void SimpleValue(string name, object value)
    {
      if (!FirstValue)
        Writer.Write("&");
      string s = (value != null ? value.ToString() : "");
      Writer.Write(HttpUtility.UrlEncode(name, Writer.Encoding) + "=" + HttpUtility.UrlEncode(s, Writer.Encoding));
      FirstValue = false;
    }


    public void End()
    {
      Writer.Flush();
    }

    #endregion
  }
}

using System;
using System.IO;
using System.Web;
using Ramone.IO;
using Ramone.Utility.ObjectSerialization;
using System.Text;


namespace Ramone.Utility
{
  public class FormUrlEncodingPropertyVisitor : IPropertyVisitor
  {
    protected TextWriter Writer;
    protected Encoding Encoding;
    protected bool FirstValue = true;


    public FormUrlEncodingPropertyVisitor(TextWriter writer, Encoding enc = null)
    {
      Writer = writer;
      Encoding = enc ?? Encoding.UTF8;
    }


    #region IPropertyVisitor

    public void Begin()
    {
    }


    public void SimpleValue(string name, object value, string formatedValue)
    {
      if (!FirstValue)
        Writer.Write("&");
      Writer.Write(HttpUtility.UrlEncode(name, Encoding));
      Writer.Write("=");
      Writer.Write(HttpUtility.UrlEncode(formatedValue, Encoding));
      FirstValue = false;
    }


    public void File(IFile file, string name)
    {
      throw new InvalidOperationException(string.Format("Cannot serialize Ramone IFile '{0}' as {1}.", name, MediaType.ApplicationFormUrlEncoded));
    }


    public void End()
    {
      Writer.Flush();
    }

    #endregion
  }
}

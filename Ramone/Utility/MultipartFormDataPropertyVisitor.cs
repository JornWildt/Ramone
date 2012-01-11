using System.IO;
using System.Web;
using System;


namespace Ramone.Utility
{
  public class MultipartFormDataPropertyVisitor : IPropertyVisitor
  {
    protected TextWriter Writer;
    protected string Boundary;


    public MultipartFormDataPropertyVisitor(TextWriter writer, string boundary = null)
    {
      Writer = writer;
      Boundary = (boundary != null ? boundary : Guid.NewGuid().ToString());
    }


    #region IPropertyVisitor

    public void SimpleValue(string name, object value)
    {
      string s = (value != null ? value.ToString() : "");
      string header = string.Format(@"
--{0}
Content-Disposition: form-data; name=""{1}""

", Boundary, HttpUtility.UrlEncode(name));

      Writer.Write(header);
      Writer.Write(s);
    }

    #endregion
  }
}

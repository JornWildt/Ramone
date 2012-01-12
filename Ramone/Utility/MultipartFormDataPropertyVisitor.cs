using System.IO;
using System.Web;
using System;
using Ramone.IO;
using System.Text;


namespace Ramone.Utility
{
  public class MultipartFormDataPropertyVisitor : IPropertyVisitor
  {
    protected TextWriter Writer;
    protected Stream Output;
    protected string Boundary;


    public MultipartFormDataPropertyVisitor(Stream s, Encoding encoding = null, string boundary = null)
    {
      if (encoding == null)
        encoding = Encoding.UTF8;
      Writer = new StreamWriter(s, encoding);
      Output = s;
      Boundary = (boundary != null ? boundary : Guid.NewGuid().ToString());
    }


    #region IPropertyVisitor

    public void Begin()
    {
    }


    public void SimpleValue(string name, object value)
    {
      string header = string.Format(@"
--{0}
Content-Disposition: form-data; name=""{1}""

", Boundary, HttpUtility.UrlEncode(name));

      Writer.Write(header);
      Writer.Flush();
      WritePropertyValue(value);
    }


    public void End()
    {
      Writer.Flush();
    }

    #endregion


    protected void WritePropertyValue(object value)
    {
      if (value is IFile)
      {
        IFile file = (IFile)value;
        // Make sure we write to right place in stream!
        Writer.Flush();
        using (Stream s = file.OpenStream())
        {
          s.CopyTo(Output);
        }
      }
      else
      {
        string s = (value != null ? value.ToString() : "");
        Writer.Write(s);
      }
    }
  }
}

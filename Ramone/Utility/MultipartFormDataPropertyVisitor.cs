using System;
using System.IO;
using System.Text;
using Ramone.IO;
using Ramone.Utility.ObjectSerialization;


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


    public void SimpleValue(string name, object value, string formatedValue)
    {
      string filename = "";
      string contentType = "";
      if (value is IFile)
      {
        IFile file = (IFile)value;
        filename = string.Format("; filename=\"{0}\"", System.IO.Path.GetFileName(file.Filename ?? "unknown"));
        if (file.ContentType != null)
          contentType = string.Format("\r\nContent-Type: {0}", file.ContentType);
      }
      else
      {
        contentType = string.Format("\r\nContent-Type: text/plain; charset={0}", Writer.Encoding.BodyName);
      }

      string header = string.Format(@"
--{0}
Content-Disposition: form-data; name=""{1}""{2}{3}

", Boundary, name, filename, contentType);

      // FIXME: escaping name and filename

      Writer.Write(header);
      Writer.Write(formatedValue);
    }


    public void File(IFile file, string name)
    {
      string contentType = "";
      string filename = string.Format("; filename=\"{0}\"", Path.GetFileName(file.Filename ?? "unknown"));
      if (file.ContentType != null)
        contentType = string.Format("\r\nContent-Type: {0}", file.ContentType);

      string header = string.Format(@"
--{0}
Content-Disposition: form-data; name=""{1}""{2}{3}

", Boundary, name, filename, contentType);

      // FIXME: escaping name and filename

      Writer.Write(header);
      Writer.Flush();
      using (Stream s = file.OpenStream())
      {
        s.CopyTo(Output);
      }
    }


    public void End()
    {
      Writer.Flush();
    }

    #endregion
  }
}

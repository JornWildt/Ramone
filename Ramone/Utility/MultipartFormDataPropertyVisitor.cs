using System;
using System.IO;
using System.Text;
using Ramone.IO;
using Ramone.Utility.ObjectSerialization;
using System.Text.RegularExpressions;
using System.Web;


namespace Ramone.Utility
{
  public class MultipartFormDataPropertyVisitor : IPropertyVisitor
  {
    protected TextWriter Writer;
    protected Stream Output;
    protected string Boundary;
    protected ObjectSerializerSettings Settings;

    public MultipartFormDataPropertyVisitor(Stream s, Encoding encoding = null, string boundary = null, ObjectSerializerSettings settings = null)
    {
      if (encoding == null)
        encoding = Encoding.UTF8;
      Writer = new StreamWriter(s, encoding);
      Output = s;
      Boundary = (boundary != null ? boundary : Guid.NewGuid().ToString());
      Settings = settings;
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
        filename = string.Format("; filename=\"{0}\"", FileUtility.GetFileName(file.Filename ?? "unknown"));
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
      string filename = FileUtility.GetFileName(file.Filename ?? "unknown");
      string asciiFilename = Regex.Replace(filename, @"[^\u0000-\u007F]", "x");
      string filenameFormat = string.Format("; filename=\"{0}\"", asciiFilename.Replace("\"", "\\\""));
      if (asciiFilename != filename && Settings != null && Settings.EnableNonAsciiCharactersInMultipartFilenames)
      {
        string utf8Filename = HttpUtility.UrlEncode(filename, Encoding.UTF8);
        filenameFormat += string.Format("; filename*=UTF-8''{0}", utf8Filename);
      }
      if (file.ContentType != null)
        contentType = string.Format("\r\nContent-Type: {0}", file.ContentType);

      string header = string.Format(@"
--{0}
Content-Disposition: form-data; name=""{1}""{2}{3}

", Boundary, name, filenameFormat, contentType);

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

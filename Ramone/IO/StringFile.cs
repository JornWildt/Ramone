using System.IO;
using System.Text;


namespace Ramone.IO
{
  public class StringFile : Ramone.IO.IFile
  {
    #region IFile Members

    public string ContentType { get; set; }

    public string Filename { get; set; }

    public Stream OpenStream()
    {
      return new MemoryStream(DataEncoding == null ? Encoding.UTF8.GetBytes(Data) : DataEncoding.GetBytes(Data));
    }

    #endregion

    public string Data { get; set; }

    public Encoding DataEncoding { get; set; }
  }
}

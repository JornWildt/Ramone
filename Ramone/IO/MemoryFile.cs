using System.IO;


namespace Ramone.IO
{
  public class MemoryFile : Ramone.IO.IFile
  {
    #region IFile Members

    public string ContentType { get; set; }

    public string Filename { get; set; }

    public Stream OpenStream()
    {
      return new MemoryStream(Bytes);
    }

    #endregion

    public byte[] Bytes { get; set; }
  }
}

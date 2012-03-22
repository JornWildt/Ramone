using System.IO;
using Ramone.IO;


namespace Ramone.Tests
{
  public class FileWithSpecialName : IFile
  {
    #region IFile Members

    public string Filename { get { return _title; } }

    public string ContentType { get; protected set; }

    public Stream OpenStream()
    {
      return new FileStream(_filename, FileMode.Open);
    }

    #endregion


    private string _filename;

    private string _title;

    public FileWithSpecialName(string filename, string title, string contentType = null)
    {
      _filename = filename;
      _title = title;
      ContentType = contentType;
    }
  }
}

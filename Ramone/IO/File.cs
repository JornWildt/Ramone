using System;
using System.IO;


namespace Ramone.IO
{
  public class File : IFile
  {
    #region IFile Members

    public string Filename
    {
      get { return TargetFilename == null ? DiskFilename : TargetFilename; }
    }

    public string ContentType { get; protected set; }

    public Stream OpenStream()
    {
      return new FileStream(DiskFilename, FileMode.Open);
    }

    #endregion


    public string DiskFilename { get; protected set; }

    public string TargetFilename { get; protected set; }


    public File(string filename, string contentType = null, string targetFilename = null)
    {
      DiskFilename = filename;
      TargetFilename = targetFilename;
      ContentType = contentType;
    }
  }
}

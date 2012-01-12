using System.IO;


namespace Ramone.IO
{
  public interface IFile
  {
    string Filename { get; }
    string ContentType { get; }
    Stream OpenStream();
  }
}

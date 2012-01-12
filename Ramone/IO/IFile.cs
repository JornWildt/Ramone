using System.IO;


namespace Ramone.IO
{
  public interface IFile
  {
    string Filename { get; }
    Stream OpenStream();
  }
}

using OpenRasta.IO;
using System.IO;
using OpenRasta.Web;
using System.Web;
using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Handlers
{
  public class FileHandler
  {
    public object Get()
    {
      MemoryStream s = new MemoryStream(new byte[] { 6, 7, 8, 9 });
      return new InMemoryFile(s) { ContentType = MediaType.ApplicationOctetStream };
    }


    //public object Post()
    //{
    //  byte[] data = new byte[100];
    //  int cou = HttpContext.Current.Request.InputStream.Read(data, 0, 100);
    //  MemoryStream s = new MemoryStream(data, 0, cou);
    //  return new InMemoryFile(s);
    //}


    //public object Post(IFile file)
    //{
    //  byte[] data = new byte[100];
    //  int cou = file.OpenStream().Read(data, 0, 100);
    //  MemoryStream s = new MemoryStream(data, 0, cou);
    //  return new InMemoryFile(s) { ContentType = file.ContentType, FileName = file.FileName };
    //}
  }
}
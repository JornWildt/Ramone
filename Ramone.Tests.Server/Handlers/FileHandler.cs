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
      return new InMemoryFile(s) { ContentType = OpenRasta.Web.MediaType.ApplicationOctetStream };
    }


    public object Post(Stream file)
    {
      byte[] data = new byte[100];
      int cou = file.Read(data, 0, 100);
      MemoryStream s = new MemoryStream(data, 0, cou);
      return new InMemoryFile(s);// { ContentType = file.ContentType, FileName = file.FileName };
    }
  }
}
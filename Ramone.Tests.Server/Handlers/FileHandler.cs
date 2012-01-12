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


    public object Post(MultipartData data)
    {
      return string.Format("{0}-{1}", data.Name, data.Age);
    }


    public class MultipartDataFile
    {
      public IFile DataFile { get; set; }
      public int Age { get; set; }
    }


    public object Post(MultipartDataFile data)
    {
      using (TextReader r = new StreamReader(data.DataFile.OpenStream()))
      {
        string content = r.ReadToEnd();
        return string.Format("{0}-{1}-{2}", data.DataFile.FileName, data.DataFile.ContentType, content, data.Age);
      }
    }
  }
}
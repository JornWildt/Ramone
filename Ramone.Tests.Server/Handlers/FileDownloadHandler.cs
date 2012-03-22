namespace Ramone.Tests.Server.Handlers
{
  public class FileDownloadHandler
  {
    public object Get()
    {
      return new Ramone.Tests.Server.Configuration.FileDownload { Content = "1234567890" };
    }
  }
}
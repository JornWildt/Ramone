using System.IO;
using OpenRasta.Codecs;


namespace Ramone.Tests.Server.Codecs
{
  [MediaType("*/*")]
  public class FileDownloadCodec : OpenRasta.Codecs.IMediaTypeWriter
  {
    #region IMediaTypeWriter Members

    public void WriteTo(object entity, OpenRasta.Web.IHttpEntity response, string[] codecParameters)
    {
      response.ContentType = new OpenRasta.Web.MediaType("text/plain");
      Ramone.Tests.Server.Configuration.FileDownload data = (Ramone.Tests.Server.Configuration.FileDownload)entity;

      using (StreamWriter w = new StreamWriter(response.Stream))
      {
        w.Write(data.Content);
      }
    }

    #endregion


    #region ICodec Members

    public object Configuration { get; set; }

    #endregion
  }
}
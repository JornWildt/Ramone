using System.IO;
using OpenRasta.Web;
using Ramone.Tests.Server.Blog.Resources;


namespace Ramone.Tests.Server.Blog.Codecs
{
  [OpenRasta.Codecs.MediaType("image/*")]
  public class ImageCodec : OpenRasta.Codecs.IMediaTypeWriter
  {
    #region IMediaTypeWriter Members

    public void WriteTo(object entity, IHttpEntity response, string[] codecParameters)
    {
      Image image = (Image)entity;
      image.Data.Seek(0, SeekOrigin.Begin);
      image.Data.WriteTo(response.Stream);
    }

    #endregion

    #region ICodec Members

    public object Configuration { get; set; }

    #endregion
  }
}
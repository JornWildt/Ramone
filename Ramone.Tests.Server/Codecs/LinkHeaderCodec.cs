using OpenRasta.Codecs;


namespace Ramone.Tests.Server.Codecs
{
  [MediaType("*/*")]
  public class LinkHeaderCodec : OpenRasta.Codecs.IMediaTypeWriter
  {
    #region IMediaTypeWriter Members

    public void WriteTo(object entity, OpenRasta.Web.IHttpEntity response, string[] codecParameters)
    {
      response.Headers["Link"] = @"<http://example.com/TheBook/chapter2>; rel=""previous""; title=""Previous chapter"", <http://example.com/TheBook/chapter4>; rel=""next""; title=""Next chapter""";
    }

    #endregion


    #region ICodec Members

    public object Configuration { get; set; }

    #endregion
  }
}
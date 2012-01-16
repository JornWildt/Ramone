using System.IO;
using System.Text;
using System.Web;
using OpenRasta.Codecs;
using Ramone.Tests.Common;
using Ramone.Utility;


namespace Ramone.Tests.Server.Codecs
{
  [MediaType("text/html")]
  [MediaType("application/xml")]
  [MediaType("application/json")]
  public class EncodingCodec : OpenRasta.Codecs.IMediaTypeWriter, OpenRasta.Codecs.IMediaTypeReader
  {
    #region IMediaTypeReader Members

    public object ReadFrom(OpenRasta.Web.IHttpEntity request, OpenRasta.TypeSystem.IType destinationType, string destinationName)
    {
      HttpContext context = HttpContext.Current;

      Encoding enc = Encoding.Default;
      string charset = request.ContentType.CharSet;
      if (charset == null)
        charset = "unknown";
      else
        enc = Encoding.GetEncoding(charset);
      context.Response.Headers.Add("X-request-charset", charset);

      using (StreamReader reader = new StreamReader(request.Stream, enc))
      {
        string data = reader.ReadToEnd();
        return new EncodingData { Data = data };
      }
    }

    #endregion


    #region IMediaTypeWriter Members

    public void WriteTo(object entity, OpenRasta.Web.IHttpEntity response, string[] codecParameters)
    {
      HttpContext context = HttpContext.Current;
      EncodingData data = (EncodingData)entity;

      string charset = context.Request.Headers["Accept-Charset"];
      context.Response.Headers.Add("X-accept-charset", charset);
      context.Response.Headers.Add("Content-Type", "text/html: charset=" + charset);
      Encoding enc = Encoding.GetEncoding(charset);

      using (StreamWriter writer = new StreamWriter(response.Stream, enc))
      {
        writer.Write(data.Data);
      }
    }

    #endregion


    #region ICodec Members

    public object Configuration { get; set; }

    #endregion
  }
}
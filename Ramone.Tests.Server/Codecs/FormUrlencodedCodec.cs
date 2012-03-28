using System.IO;
using OpenRasta.Codecs;
using Ramone.Utility;
using OpenRasta.Web;
using System.Text;


namespace Ramone.Tests.Server.Codecs
{
  [MediaType("application/x-www-form-urlencoded")]
  public class FormUrlencodedCodec : OpenRasta.Codecs.IMediaTypeWriter
  {
    public ICommunicationContext Context { get; set; }

    public void WriteTo(object entity, OpenRasta.Web.IHttpEntity response, string[] codecParameters)
    {
      using (TextWriter writer = new StreamWriter(response.Stream))
      {
        Encoding enc = null;
        if (Context.Request.Headers["Accept-Charset"] != null)
          enc = Encoding.GetEncoding(Context.Request.Headers["Accept-Charset"]);
        FormUrlEncodingSerializer serializer = new FormUrlEncodingSerializer(entity.GetType());
        serializer.Serialize(writer, entity);
      }
    }


    public object Configuration { get; set; }
  }
}
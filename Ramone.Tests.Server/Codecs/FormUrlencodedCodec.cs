using System.IO;
using OpenRasta.Codecs;
using Ramone.Utility;


namespace Ramone.Tests.Server.Codecs
{
  [MediaType("application/x-www-form-urlencoded")]
  public class FormUrlencodedCodec : OpenRasta.Codecs.IMediaTypeWriter
  {
    public void WriteTo(object entity, OpenRasta.Web.IHttpEntity response, string[] codecParameters)
    {
      using (TextWriter writer = new StreamWriter(response.Stream))
      {
        FormUrlEncodingSerializer serializer = new FormUrlEncodingSerializer(entity.GetType());
        serializer.Serialize(writer, entity);
      }
    }


    public object Configuration { get; set; }
  }
}
using System.IO;
using OpenRasta.Codecs;
using Ramone.Utility;
using OpenRasta.Web;
using System.Text;
using Ramone.Utility.ObjectSerialization;
using Ramone.Tests.Common;

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
        ObjectSerializerSettings settings = new ObjectSerializerSettings
        {
          Encoding = enc,
          IncludeNullValues = entity is ISerializeWithNullValues
        };
        FormUrlEncodingSerializer serializer = new FormUrlEncodingSerializer(entity.GetType());
        serializer.Serialize(writer, entity, settings);
      }
    }


    public object Configuration { get; set; }
  }
}
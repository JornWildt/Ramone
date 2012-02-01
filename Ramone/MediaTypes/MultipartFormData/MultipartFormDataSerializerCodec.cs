using System;
using System.IO;
using Ramone.Utility;
using System.Text;


namespace Ramone.MediaTypes.MultipartFormData
{
  public class MultipartFormDataSerializerCodec : IMediaTypeWriter
  {
    public void WriteTo(WriterContext context)
    {
      if (context.Data == null)
        return;

      Type t = context.Data.GetType();
      MultipartFormDataSerializer Serializer = new MultipartFormDataSerializer(t);

      Encoding enc = Encoding.Default;

      MediaType m = MediaTypeParser.ParseMediaType(context.Request.ContentType);
      if (m.Parameters.ContainsKey("charset"))
        enc = Encoding.GetEncoding(m.Parameters["charset"]);

      Serializer.Serialize(context.HttpStream, context.Data, enc, CodecArgument as string, context.Session.SerializerSettings);
    }


    #region IMediaTypeCodec

    public object CodecArgument { get; set; }

    #endregion
  }
}

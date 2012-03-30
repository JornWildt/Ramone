using System;
using System.IO;
using Ramone.Utility;
using System.Text;
using System.Net.Mime;


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

      string charset = context.CodecParameters["Charset"];

      Encoding enc = (charset != null ? Encoding.GetEncoding(charset) : context.Session.DefaultEncoding);

      Serializer.Serialize(context.HttpStream, context.Data, enc, CodecArgument as string, context.Session.SerializerSettings);
    }


    #region IMediaTypeCodec

    public object CodecArgument { get; set; }

    #endregion
  }
}

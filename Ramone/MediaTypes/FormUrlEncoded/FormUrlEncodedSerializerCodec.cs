using System;
using System.IO;
using Ramone.Utility;
using System.Text;


namespace Ramone.MediaTypes.FormUrlEncoded
{
  public class FormUrlEncodedSerializerCodec : IMediaTypeWriter
  {
    public void WriteTo(WriterContext context)
    {
      if (context.Data == null)
        return;

      Encoding enc = MediaTypeParser.GetEncodingFromCharset(context.Request.ContentType);

      Type t = context.Data.GetType();
      FormUrlEncodingSerializer Serializer = new FormUrlEncodingSerializer(t);

      using (TextWriter w = new StreamWriter(context.HttpStream, enc))
      {
        Serializer.Serialize(w, context.Data, context.Session.FormUrlEncodedSerializerSettings);
      }
    }


    #region IMediaTypeCodec

    public object CodecArgument { get; set; }

    #endregion
  }
}

using System;
using System.IO;
using Ramone.Utility;
using System.Text;
using System.Collections.Specialized;
using System.Web;


namespace Ramone.MediaTypes.FormUrlEncoded
{
  public class FormUrlEncodedSerializerCodec : TextCodecBase<object>  //IMediaTypeWriter, IMediaTypeReader
  {
    protected override void WriteTo(object item, TextWriter writer, WriterContext context)
    {
      if (item == null)
        return;

      FormUrlEncodingSerializer Serializer = new FormUrlEncodingSerializer(item.GetType());
      Serializer.Serialize(writer, item, context.Session.SerializerSettings);
    }


    protected override object ReadFrom(TextReader reader, ReaderContext context)
    {
      string data = reader.ReadToEnd();
      NameValueCollection values = HttpUtility.ParseQueryString(data);
      return values;
    }


#if false
    public void WriteTo(WriterContext context)
    {
      if (context.Data == null)
        return;

      Encoding enc = MediaTypeParser.GetEncodingFromCharset(context.Request.ContentType);

      Type t = context.Data.GetType();
      FormUrlEncodingSerializer Serializer = new FormUrlEncodingSerializer(t);

      using (TextWriter w = new StreamWriter(context.HttpStream, enc))
      {
        Serializer.Serialize(w, context.Data, context.Session.SerializerSettings);
      }
    }


    public object ReadFrom(ReaderContext context)
    {
      NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);
    }


    #region IMediaTypeCodec

    public object CodecArgument { get; set; }

    #endregion
#endif
  }
}

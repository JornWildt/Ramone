using System.IO;
using Ramone.Utility;


namespace Ramone.MediaTypes.FormUrlEncoded
{
  public class FormUrlEncodedSerializerCodec : TextCodecBase<object>
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
      FormUrlEncodingSerializer Serializer = new FormUrlEncodingSerializer(context.DataType);
      return Serializer.Deserialize(reader, context.Session.SerializerSettings);
    }
  }
}

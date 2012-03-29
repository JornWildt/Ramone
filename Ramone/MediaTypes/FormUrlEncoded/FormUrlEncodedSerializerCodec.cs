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

      FormUrlEncodingSerializer serializer = new FormUrlEncodingSerializer(item.GetType());
      serializer.Serialize(writer, item, context.Session.SerializerSettings);
    }


    protected override object ReadFrom(TextReader reader, ReaderContext context)
    {
      FormUrlEncodingSerializer serializer = new FormUrlEncodingSerializer(context.DataType);
      return serializer.Deserialize(reader, context.Session.SerializerSettings);
    }
  }
}

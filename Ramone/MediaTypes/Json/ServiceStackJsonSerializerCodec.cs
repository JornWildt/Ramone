using System;
using System.IO;
using ServiceStack.Text;


namespace Ramone.MediaTypes.Json
{
  public class ServiceStackJsonSerializerCodec : TextCodecBase<object>
  {
    protected override object ReadFrom(TextReader reader, ReaderContext context)
    {
      return JsonSerializer.DeserializeFromReader(reader, context.DataType);
    }


    protected override void WriteTo(object item, TextWriter writer, WriterContext context)
    {
      if (item == null)
        throw new ArgumentNullException("item");

      JsonSerializer.SerializeToWriter(item, writer);
    }
  }
}

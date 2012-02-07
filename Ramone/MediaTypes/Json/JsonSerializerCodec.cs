using System;
using System.IO;
using JsonFx.Json;


namespace Ramone.MediaTypes.Json
{
  public class JsonSerializerCodec : TextCodecBase<object>
  {
    protected override object ReadFrom(TextReader reader, ReaderContext context)
    {
      JsonReader jsr = new JsonReader();
      return jsr.Read(reader, context.DataType);
    }


    protected override void WriteTo(object item, TextWriter writer, WriterContext context)
    {
      if (item == null)
        throw new ArgumentNullException("item");

      JsonWriter jsw = new JsonWriter();
      jsw.Write(item, writer);
    }
  }
}

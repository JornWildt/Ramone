using System;
using System.IO;
using Newtonsoft.Json;
using System.Dynamic;


namespace Ramone.MediaTypes.Json
{
  public class JsonSerializerCodec : TextCodecBase<object>
  {
    protected override object ReadFrom(TextReader reader, ReaderContext context)
    {
      using (JsonReader jsr = new JsonTextReader(reader))
      {
        JsonSerializer serializer = new JsonSerializer();
        // Avoid JSON.NET wrapping result in JToken wrapper - return ExpandoObject for "object"
        Type t = (context.DataType == typeof(object) ? typeof(ExpandoObject) : context.DataType);
        return serializer.Deserialize(jsr, t);
      }
    }


    protected override void WriteTo(object item, TextWriter writer, WriterContext context)
    {
      if (item == null)
        throw new ArgumentNullException("item");

      using (JsonWriter jsw = new JsonTextWriter(writer))
      {
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(jsw, item);
      }
    }
  }
}

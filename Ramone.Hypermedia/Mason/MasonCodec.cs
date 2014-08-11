using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ramone;
using Ramone.MediaTypes;
using System;
using System.IO;


namespace Ramone.Hypermedia.Mason
{
  public class MasonCodec : TextCodecBase<MasonResource>
  {
    protected override MasonResource ReadFrom(TextReader reader, ReaderContext context)
    {
      using (JsonReader jsr = new JsonTextReader(reader))
      {
        object json = JToken.ReadFrom(jsr);

        if (json == null)
          return null;
        if (!(json is JObject))
          throw new InvalidOperationException(string.Format("Unexpected response type '{0}' - was looking for '{1}'.", json.GetType(), typeof(JObject)));

        return new Builder().Build((JObject)json);
      }
    }


    protected override void WriteTo(MasonResource item, TextWriter writer, WriterContext context)
    {
      throw new NotImplementedException();
    }
  }
}

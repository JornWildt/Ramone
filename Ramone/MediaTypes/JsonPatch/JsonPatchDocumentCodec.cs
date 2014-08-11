using System;
using System.IO;
using System.Text;


namespace Ramone.MediaTypes.JsonPatch
{
  public class JsonPatchDocumentCodec :  TextCodecBase<JsonPatchDocument>
  {
    protected override Encoding DefaultEncoding { get { return Encoding.UTF8; } }


    protected override void WriteTo(JsonPatchDocument patch, TextWriter writer, WriterContext context)
    {
      if (patch == null)
        throw new ArgumentNullException("patch");

      patch.Write(writer);
    }


    protected override JsonPatchDocument ReadFrom(TextReader reader, ReaderContext context)
    {
      return JsonPatchDocument.Read(reader);
    }
  }
}

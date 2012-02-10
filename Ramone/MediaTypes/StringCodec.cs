using System.IO;


namespace Ramone.MediaTypes
{
  public class StringCodec : TextCodecBase<string>
  {
    protected override string ReadFrom(TextReader reader, ReaderContext context)
    {
      return reader.ReadToEnd();
    }


    protected override void WriteTo(string text, TextWriter writer, WriterContext context)
    {
      writer.Write(text);
    }
  }
}

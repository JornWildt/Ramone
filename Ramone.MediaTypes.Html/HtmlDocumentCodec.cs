using System.IO;
using HtmlAgilityPack;


namespace Ramone.MediaTypes.Html
{
  public class HtmlDocumentCodec : TextCodecBase<HtmlDocument>
  {
    protected override HtmlDocument ReadFrom(TextReader reader, ReaderContext context)
    {
      HtmlDocument doc = new HtmlDocument();
      doc.Load(reader);
      return doc;
    }


    protected override void WriteTo(HtmlDocument doc, TextWriter writer, WriterContext context)
    {
      doc.Save(writer);
    }
  }
}

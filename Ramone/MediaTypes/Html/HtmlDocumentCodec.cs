using System.IO;
using HtmlAgilityPack;


namespace Ramone.MediaTypes.Html
{
  public class HtmlDocumentCodec : TextCodecBase<HtmlDocument>
  {
    protected override HtmlDocument ReadFrom(TextReader reader, ReaderContext context)
    {
      HtmlDocument doc = new HtmlDocument();

      HtmlNode.ElementsFlags.Remove("form");
      HtmlNode.ElementsFlags.Add("form", HtmlElementFlag.CanOverlap);

      doc.Load(reader);
      return doc;
    }


    protected override void WriteTo(HtmlDocument doc, TextWriter writer, WriterContext context)
    {
      doc.Save(writer);
    }
  }
}

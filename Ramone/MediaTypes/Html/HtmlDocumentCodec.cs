using System.IO;
using HtmlAgilityPack;


namespace Ramone.MediaTypes.Html
{
  public class HtmlDocumentCodec : TextCodecBase<HtmlDocument>
  {
    protected override HtmlDocument ReadFrom(TextReader reader)
    {
      HtmlDocument doc = new HtmlDocument();
      doc.Load(reader);
      return doc;
    }


    protected override void WriteTo(HtmlDocument doc, TextWriter writer)
    {
      doc.Save(writer);
    }
  }
}

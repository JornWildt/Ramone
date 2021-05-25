using HtmlAgilityPack;

namespace Ramone.MediaTypes.Html
{
  class HtmlInitializer
  {
    void RegisterStandardCodecs(ICodecManager cm)
    {
      HtmlNode.ElementsFlags.Remove("form");
      HtmlNode.ElementsFlags.Add("form", HtmlElementFlag.CanOverlap);

      cm.AddCodec<HtmlDocument, HtmlDocumentCodec>(MediaType.TextHtml);
      cm.AddCodec<HtmlDocument, HtmlDocumentCodec>(MediaType.TextXml);
      cm.AddCodec<HtmlDocument, HtmlDocumentCodec>(MediaType.ApplicationXHtml);
      cm.AddCodec<HtmlDocument, HtmlDocumentCodec>(MediaType.ApplicationXml);
    }

  }
}

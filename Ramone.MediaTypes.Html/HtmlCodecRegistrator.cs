using HtmlAgilityPack;

namespace Ramone.MediaTypes.Html
{
  public class HtmlCodecRegistrator : ICodecRegistrator
  {
    public void RegisterCodecs(ICodecManager cm)
    {
      cm.AddCodec<HtmlDocument, HtmlDocumentCodec>(MediaType.TextHtml);
      cm.AddCodec<HtmlDocument, HtmlDocumentCodec>(MediaType.TextXml);
      cm.AddCodec<HtmlDocument, HtmlDocumentCodec>(MediaType.ApplicationXHtml);
      cm.AddCodec<HtmlDocument, HtmlDocumentCodec>(MediaType.ApplicationXml);
    }
  }
}

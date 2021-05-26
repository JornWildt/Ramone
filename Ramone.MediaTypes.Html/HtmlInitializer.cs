using HtmlAgilityPack;

namespace Ramone.MediaTypes.Html
{
  public static class HtmlInitializer
  {
    public static void Initialize()
    {
      HtmlNode.ElementsFlags.Remove("form");
      HtmlNode.ElementsFlags.Add("form", HtmlElementFlag.CanOverlap);

      RamoneConfiguration.AddCodecRegistrator(new HtmlCodecRegistrator());
    }
  }
}

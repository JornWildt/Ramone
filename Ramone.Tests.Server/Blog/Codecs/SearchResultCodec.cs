using System;
using System.Xml;
using OpenRasta.Codecs;
using Ramone.Tests.Server.Blog.Resources;
using Ramone.Tests.Server.Codecs;


namespace Ramone.Tests.Server.Blog.Codecs
{
  [MediaType("application/atom+xml;q=0.9", "atom")]
  [MediaType("application/atom;q=0.9", "atom")]
  public class SearchResultCodec : XmlCodecBase<SearchResult>
  {
    protected override void WriteTo(SearchResult item, XmlWriter writer)
    {
      item.Result.SaveAsAtom10(writer);
    }


    protected override SearchResult ReadFrom(XmlReader reader)
    {
      throw new NotImplementedException();
    }
  }
}
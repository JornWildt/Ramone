using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using Ramone.Utility;


namespace Ramone.HyperMedia
{
  public static class WebLinkExtensions
  {
    public static IEnumerable<WebLink> Links(this Response response)
    {
      Condition.Requires(response, "response").IsNotNull();

      if (response.Headers["Link"] == null)
        return Enumerable.Empty<WebLink>();

      return WebLinkParser.ParseLinks(response.BaseUri, response.Headers["Link"]);
    }
  }
}

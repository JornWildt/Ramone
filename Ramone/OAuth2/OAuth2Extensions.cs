using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ramone.OAuth2
{
  public static class OAuth2Extensions
  {
    static void OAuth1Configure(this ISession session, OAuth2Settings settings)
    {
      //session.RequestInterceptors.Add("OAuth2", new OAuth1RequestInterceptor(settings));
    }
  }
}

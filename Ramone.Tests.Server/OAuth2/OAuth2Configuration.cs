using OpenRasta.Configuration;
using Ramone.Tests.Common.OAuth2;
using Ramone.Tests.Server.Codecs;
using Ramone.Tests.Server.OAuth2.Handlers;


namespace Ramone.Tests.Server.OAuth2
{
  public static class OAuth2Configuration
  {
    public static void Configure()
    {
      ResourceSpace.Has.ResourcesOfType<OAuth2AccessTokenResponse>()
          .AtUri(OAuth2TestConstants.TokenEndpointPath)
          .HandledBy<OAuth2TokenEndpointHandler>()
          .TranscodedBy<JsonSerializerCodec<OAuth2AccessTokenResponse>>();

      ResourceSpace.Has.ResourcesOfType<OAuth2Error>()
        .WithoutUri.TranscodedBy<JsonSerializerCodec<OAuth2Error>>();
    }
  }
}
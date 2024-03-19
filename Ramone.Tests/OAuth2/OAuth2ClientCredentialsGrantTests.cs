using System.Net;
using NUnit.Framework;
using Ramone.OAuth2;
using Ramone.Tests.Common.OAuth2;


namespace Ramone.Tests.OAuth2
{
  [TestFixture]
  public class OAuth2ClientCredentialsGrantTests : TestHelper
  {
    [Test]
    public void CanGetAccessTokenWithAdditionalParametersUsingOAuth2Extensions()
    {
      OAuth2AccessTokenResponse token =
        Session.OAuth2_Configure(GetSettings())
               .OAuth2_GetAccessTokenUsingClientCredentials();

      Assert.That(token, Is.Not.Null);
      Assert.That(token.access_token, Is.Not.Null.And.Not.Empty);
      Assert.That(token.expires_in, Is.EqualTo(199));
      Assert.That((string)token.AllParameters["additional_param"], Is.EqualTo("Special"));
    }
  }
}

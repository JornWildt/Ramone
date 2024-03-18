using NUnit.Framework;
using Ramone.Utility;


namespace Ramone.Tests.Utility
{
  [TestFixture]
  public class Base64UtilityTests : TestHelper
  {
    [Test]
    public void CanBase64UrlEncode()
    {
      // Example values from http://tools.ietf.org/html/draft-ietf-jose-json-web-signature-08#appendix-A.1
      // Arrange
      string jwtHeader = @"{""typ"":""JWT"",
 ""alg"":""HS256""}";
      string jwtPayload = @"{""iss"":""joe"",
 ""exp"":1300819380,
 ""http://example.com/is_root"":true}";

      // Act
      string encodedHeader = Base64Utility.UTF8UrlEncode(jwtHeader);
      string encodedPayload = Base64Utility.UTF8UrlEncode(jwtPayload);

      // Assert
      Assert.That(encodedHeader, Is.EqualTo("eyJ0eXAiOiJKV1QiLA0KICJhbGciOiJIUzI1NiJ9"));
      Assert.That(encodedPayload, Is.EqualTo("eyJpc3MiOiJqb2UiLA0KICJleHAiOjEzMDA4MTkzODAsDQogImh0dHA6Ly9leGFtcGxlLmNvbS9pc19yb290Ijp0cnVlfQ"));
    }
  }
}

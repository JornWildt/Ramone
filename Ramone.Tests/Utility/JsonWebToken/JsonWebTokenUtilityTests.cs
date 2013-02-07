using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using Ramone.Utility.JsonWebToken;


namespace Ramone.Tests.Utility.JsonWebToken
{
  [TestFixture]
  public class JsonWebTokenUtilityTests : TestHelper
  {
    string CertificatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Ramone.Tests\\SigningCertificate.pfx");
    X509Certificate2 Certificate;


    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      Certificate = new X509Certificate2(CertificatePath, "123456", X509KeyStorageFlags.Exportable);
    }


    [Test]
    public void CanSignUsing_SHA256()
    {
      // Example values from http://tools.ietf.org/html/draft-ietf-jose-json-web-signature-08#appendix-A.1
      // Arrange
      string claim = "eyJ0eXAiOiJKV1QiLA0KICJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJqb2UiLA0KICJleHAiOjEzMDA4MTkzODAsDQogImh0dHA6Ly9leGFtcGxlLmNvbS9pc19yb290Ijp0cnVlfQ";

      // Act
      string signature = JsonWebTokenUtility.HMAC_ASCII_SHA256_Base64Url(claim, SHA256Key);

      // Assert
      Assert.AreEqual("dBjftJeZ4CVP-mB92K27uhbUJU1p1r_wW1gFWFOEjXk", signature);
    }


    [Test]
    public void CanSignUsing_RSA1()
    {
      using (RSACryptoServiceProvider cp = (RSACryptoServiceProvider)Certificate.PrivateKey)
      {
        // Act
        string signature = JsonWebTokenUtility.HMAC_ASCII_RSASHA1_Base64Url("1234", cp);

        // Assert
        Assert.IsNotNull(signature);
      }
    }


    [Test]
    public void CanCreateJsonWebTokenWithSHA256_UsingSpecificHeaders()
    {
      // Example values from http://tools.ietf.org/html/draft-ietf-jose-json-web-signature-08#appendix-A.1
      // Arrange
      string header = @"{""typ"":""JWT"",
 ""alg"":""HS256""}";
      string payload = @"{""iss"":""joe"",
 ""exp"":1300819380,
 ""http://example.com/is_root"":true}";

      // Act
      string token = JsonWebTokenUtility.JWT_SHA256(header, payload, SHA256Key);

      // Assert
      Assert.AreEqual("eyJ0eXAiOiJKV1QiLA0KICJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJqb2UiLA0KICJleHAiOjEzMDA4MTkzODAsDQogImh0dHA6Ly9leGFtcGxlLmNvbS9pc19yb290Ijp0cnVlfQ.dBjftJeZ4CVP-mB92K27uhbUJU1p1r_wW1gFWFOEjXk", token);
    }


    [Test]
    public void CanCreateJsonWebTokenWithSHA256()
    {
      // Arrange
      string payload = @"{""iss"":""joe"",
 ""exp"":1300819380,
 ""http://example.com/is_root"":true}";

      // Act
      string token = JsonWebTokenUtility.JWT_SHA256(payload, SHA256Key);

      // Assert
      Assert.AreEqual("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJqb2UiLA0KICJleHAiOjEzMDA4MTkzODAsDQogImh0dHA6Ly9leGFtcGxlLmNvbS9pc19yb290Ijp0cnVlfQ.liUd5va9zeRHhgLXwSKoXqwwfdW_SQigE717KM69cMQ", token);
    }


    [Test]
    public void CanCreateJsonWebTokenWithRSASHA1()
    {
      // Arrange
      string payload = @"{""iss"":""joe"",
 ""exp"":1300819380,
 ""http://example.com/is_root"":true}";

      // Act
      using (RSACryptoServiceProvider cp = (RSACryptoServiceProvider)Certificate.PrivateKey)
      {
        // Act
        string token = JsonWebTokenUtility.JWT_RSASHA1(payload, cp);

        // Assert
        Assert.AreEqual("eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJpc3MiOiJqb2UiLA0KICJleHAiOjEzMDA4MTkzODAsDQogImh0dHA6Ly9leGFtcGxlLmNvbS9pc19yb290Ijp0cnVlfQ.T8hWkMST9Wnb4UxUU1OBiSJ_QvI2_POzHmeKmRKEQqjq8w8vwvk0Ge8wyRGCTdehpB6c4cPlbxH3-0CI0anZEMvQoCSSlGKxYeo-EGA0SkwGgE1ntFPdSc4tJd1KCQAmStbP_qP3Us7macwGH1M369zfp6l3P2AgEPD8sromgls", token);
      }
    }


    public static byte[] SHA256Key = new byte[] {
      3, 35, 53, 75, 43, 15, 165, 188, 131, 126, 6, 101, 119, 123, 166,
      143, 90, 179, 40, 230, 240, 84, 201, 40, 169, 15, 132, 178, 210, 80,
      46, 191, 211, 251, 90, 146, 210, 6, 71, 239, 150, 138, 180, 195, 119,
      98, 61, 34, 61, 46, 33, 114, 5, 46, 79, 8, 192, 205, 154, 245, 103,
      208, 128, 163 };
  }
}

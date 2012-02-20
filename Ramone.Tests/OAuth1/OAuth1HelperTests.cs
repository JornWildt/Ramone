using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Ramone.OAuth1;


namespace Ramone.Tests.OAuth1
{
  [TestFixture]
  public class OAuth1HelperTests : TestHelper
  {
    readonly Uri RequestUrl = new Uri("http://home.dk/set?name=J%c3%b8rn&oauth_token=abc&age=6");

    SignatureHelper Helper = new SignatureHelper(null, null);


    [Test]
    public void CanNormalizeParameters()
    {
      // Arrange
      List<QueryParameter> parameters = Helper.GetQueryParameters(RequestUrl.Query);

      // Act
      string normalizedParameters = Helper.NormalizeRequestParameters(parameters);

      // Assert
      Assert.AreEqual("age=6&name=J%C3%B8rn", normalizedParameters);
    }


    [Test]
    public void CanGenerateSignatureBase()
    {
      // Arrange
      string normalizedUrl;
      string normalizedRequestParameters;

      // Act
      List<QueryParameter> parameters = Helper.GetQueryParameters(RequestUrl.Query);
      string signatureBase = Helper.GenerateSignatureBase(RequestUrl, 
                                                          "xckey", 
                                                          "xcb", 
                                                          "xat", 
                                                          "xats", 
                                                          "post", 
                                                          "12345", 
                                                          "abcd",
                                                          SignatureHelper.HMACSHA1SignatureType, 
                                                          out normalizedUrl, 
                                                          out normalizedRequestParameters);

      // Assert
      Assert.AreEqual("POST&http%3A%2F%2Fhome.dk%2Fset&age%3D6%26name%3DJ%25C3%25B8rn%26oauth_callback%3Dxcb%26oauth_consumer_key%3Dxckey%26oauth_nonce%3Dabcd%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D12345%26oauth_token%3Dxat%26oauth_version%3D1.0", 
                      signatureBase);
    }


    [Test]
    public void CanGenerateSignatureBaseForSpecialChars()
    {
      // Arrange
      string normalizedUrl;
      string normalizedRequestParameters;

      // Act
      List<QueryParameter> parameters = Helper.GetQueryParameters(RequestUrl.Query);
      string signatureBase = Helper.GenerateSignatureBase(RequestUrl,
                                                          "xÆckey",
                                                          "xØcb",
                                                          "xÅat",
                                                          "xÅats",
                                                          "post",
                                                          "12345",
                                                          "abcd",
                                                          SignatureHelper.HMACSHA1SignatureType,
                                                          out normalizedUrl,
                                                          out normalizedRequestParameters);

      // Assert
      Assert.AreEqual("POST&http%3A%2F%2Fhome.dk%2Fset&age%3D6%26name%3DJ%25C3%25B8rn%26oauth_callback%3Dx%25C3%2598cb%26oauth_consumer_key%3Dx%25C3%2586ckey%26oauth_nonce%3Dabcd%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D12345%26oauth_token%3Dx%25C3%2585at%26oauth_version%3D1.0",
                      signatureBase);
    }


    [Test]
    public void CanGenerateSignature()
    {
      // Arrange
      string normalizedUrl;
      string normalizedRequestParameters;

      // Act
      List<QueryParameter> parameters = Helper.GetQueryParameters(RequestUrl.Query);
      string signature = Helper.GenerateSignature(RequestUrl,
                                                  "xckey",
                                                  "xcs",
                                                  "xcb",
                                                  "xat",
                                                  "xats",
                                                  "post",
                                                  "12345",
                                                  "abcd",
                                                  SignatureTypes.HMACSHA1,
                                                  out normalizedUrl,
                                                  out normalizedRequestParameters);

      // Assert
      Assert.AreEqual("V4iyFCIi0CtoAVdOA1UgpIOaRAw=", signature);
    }


    [Test]
    public void CanGenerateSignatureForSpecialChars()
    {
      // Arrange
      string normalizedUrl;
      string normalizedRequestParameters;

      // Act
      List<QueryParameter> parameters = Helper.GetQueryParameters(RequestUrl.Query);
      string signature = Helper.GenerateSignature(RequestUrl,
                                                  "xÆckey",
                                                  "xØcs",
                                                  "xØcb",
                                                  "xÅat",
                                                  "xüats",
                                                  "post",
                                                  "12345",
                                                  "abcd",
                                                  SignatureTypes.HMACSHA1,
                                                  out normalizedUrl,
                                                  out normalizedRequestParameters);

      // Assert
      Assert.AreEqual("JtsXpw8uYg4ZhaxPyGUHbhYG/Wg=", signature);
    }
  }
}

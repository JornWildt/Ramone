using NUnit.Framework;
using Ramone.MediaTypes.FormUrlEncoded;


namespace Ramone.Tests.MediaTypes.FormUrlEncoded
{
  [TestFixture]
  public class FormUrlEncodedCodecTests : TestHelper
  {
    [Test]
    public void CanPostUnregisteredFormUrlEncoded()
    {
      // Arrange
      var data = new { Name = "Pete", Age = 10 }; // Matches "MultipartData" class
      RamoneRequest formdataReq = Session.Bind(MultipartFormDataTemplate);

      // Act
      RamoneResponse<string> response = formdataReq.Accept("text/plain").ContentType("application/x-www-form-urlencoded").Post<string>(data);

      // Assert
      Assert.AreEqual("application/x-www-form-urlencoded", response.Headers["x-contenttype"]);
      Assert.AreEqual("Pete-10", response.Body);
    }


    [Test]
    public void CanPostUnregisteredFormUrlEncodedUsingShorthand()
    {
      // Arrange
      var data = new { Name = "Pete", Age = 10 }; // Matches "MultipartData" class
      RamoneRequest formdataReq = Session.Bind(MultipartFormDataTemplate);

      // Act
      RamoneResponse<string> response = formdataReq.Accept("text/plain").AsFormUrlEncoded().Post<string>(data);

      // Assert
      Assert.AreEqual("application/x-www-form-urlencoded", response.Headers["x-contenttype"]);
      Assert.AreEqual("Pete-10", response.Body);
    }

    
    class RegisteredData // Matches "MultipartData" class
    {
      public string Name { get; set; }
      public int Age { get; set; }
    }


    [Test]
    public void CanPostRegisteredFormUrlEncoded()
    {
      // Arrange
      Session.Service.CodecManager.AddCodec<RegisteredData>("application/x-www-form-urlencoded", new FormUrlEncodedSerializerCodec());
      RegisteredData data = new RegisteredData { Name = "Pete", Age = 10 };
      RamoneRequest formdataReq = Session.Bind(MultipartFormDataTemplate);

      // Act
      RamoneResponse<string> response = formdataReq.Accept("text/plain").Post<string>(data);

      // Assert
      Assert.AreEqual("application/x-www-form-urlencoded", response.Headers["x-contenttype"]);
      Assert.AreEqual("Pete-10", response.Body);
    }


    [Test]
    public void CanPostFormUrlEncodedWithEncoding()
    {
      // Arrange
      string charset = "iso-8859-1";
      var data = new { Name = "ÆØÅüî", Age = 10 }; // Matches "MultipartData" class
      RamoneRequest formdataReq = Session.Bind(MultipartFormDataTemplate);

      // Act
      RamoneResponse<string> response = formdataReq.Accept("text/plain")
                                                   .Charset(charset)
                                                   .AsFormUrlEncoded()
                                                   .Post<string>(data);

      // Assert
      Assert.AreEqual("application/x-www-form-urlencoded; charset="+charset, response.Headers["x-contenttype"]);
      Assert.AreNotEqual("ÆØÅüî-10", response.Body, "What a hack: OpenRasta always assume UTF-8, so if body is not identical to the expected it must mean that it was actually send in non-UTF-8!");
    }
  }
}

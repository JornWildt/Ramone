using System.Linq;
using System.Text;
using NUnit.Framework;
using Ramone.Tests.Common;

namespace Ramone.Tests.MediaTypes
{
  [TestFixture]
  public class StringCodecTests : TestHelper
  {
    [Test]
    public void CanGetStringFromOctetStream()
    {
      // Arrange
      Session.DefaultEncoding = Encoding.UTF8;
      Request stringReq = Session.Bind(FileTemplate);

      // Act
      using (var s = stringReq.Accept("application/octet-stream").Get<string>())
      {
        // Assert
        Assert.AreEqual("Hello ÆØÅ", s.Body);
      }
    }
    
    
    [Test]
    public void CanGetStringFromJavascript()
    {
      // Arrange
      Session.DefaultEncoding = Encoding.UTF8;
      Request stringReq = Session.Bind(CatTemplate, new { name = "Henry ÆØÅ" });

      // Act
      using (var s = stringReq.AcceptJson().Get<string>())
      {
        // Assert
        StringAssert.StartsWith("{\"Name\":\"Henry ÆØÅ\",\"DateOfBirth\":\"2012-11-24T09:11:13\"", s.Body);
      }
    }


    [Test]
    public void CanPostString()
    {
      // Arrange
      Request stringReq = Session.Bind(FileTemplate);

      // Act
      using (var s = stringReq.AsXml().Accept("application/xml").Post<string>("<?xml version=\"1.0\"?><Aaa>Anders</Aaa>"))
      {
        // Assert
        StringAssert.StartsWith("<?xml version=\"1.0\"?><Aaa>Anders</Aaa>", s.Body);
      }
    }


    [Test]
    public void CanPostStringWithDefaultMediaTypeTextPlain()
    {
      // Arrange
      Request fileReq = Session.Bind(Constants.HeaderEchoPath);

      // Act
      using (Response<HeaderList> response = fileReq.Post<HeaderList>("hello"))
      {
        HeaderList headers = response.Body;

        // Assert
        Assert.IsNotNull(headers);
        Assert.IsTrue(headers.Any(h => h == "Content-Type: text/plain"), "Must contain content type text/plain");
      }
    }
  }
}

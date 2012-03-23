using System.Text;
using NUnit.Framework;


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
      string s = stringReq.Accept("application/octet-stream").Get<string>().Body;

      // Assert
      Assert.AreEqual("Hello ÆØÅ", s);
    }
    
    
    [Test]
    public void CanGetStringFromJavascript()
    {
      // Arrange
      //Session.DefaultEncoding = Encoding.UTF8;
      Request stringReq = Session.Bind(CatTemplate, new { name = "Henry ÆØÅ" });

      // Act
      string s = stringReq.AcceptJson().Get<string>().Body;

      // Assert
      Assert.AreEqual("{\"Name\":\"Henry \\u00C6\\u00D8\\u00C5\",\"DateOfBirth\":\"2012-11-24T09:11:13.000\"}", s);
    }
  }
}

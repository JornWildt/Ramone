using HtmlAgilityPack;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests.MediaTypes.Html
{
  [TestFixture]
  public class HtmlDocumentCodecTests : TestHelper
  {
    [Test]
    public void CanReadHtmlDocument()
    {
      // Arrange
      Request req = Session.Bind(PersonTemplate, new { name = "Petrea" });

      // Act
      using (var r = req.Get<HtmlDocument>())
      {
        HtmlDocument doc = r.Body;

        // Assert
        HtmlNode personNode = doc.DocumentNode.SelectSingleNode("//*[@class=\"Person\"]");
        Assert.IsNotNull(personNode);

        HtmlNode nameNode = personNode.SelectSingleNode("//*[@class=\"Name\"]");
        Assert.IsNotNull(nameNode);
        Assert.AreEqual("Petrea", nameNode.InnerText);

        HtmlNode addressNode = personNode.SelectSingleNode("//*[@class=\"Address\"]");
        Assert.IsNotNull(addressNode);
        Assert.AreEqual("At home", addressNode.InnerText);
      }
    }


    [Test]
    public void CanReadHtmlDocumentWithEncoding(
      [Values("UTF-8", "Windows-1252", "iso-8859-1")] string charset)
    {
      // Arrange
      Request req = Session.Bind(EncodingTemplate, new { type = "html" });

      // Act
      using (var response = req.AcceptCharset(charset).Accept("text/html").Get<HtmlDocument>())
      {
        HtmlDocument doc = response.Body;

        // Assert
        HtmlNode nameNode = doc.DocumentNode.SelectSingleNode("/html/body");
        Assert.IsNotNull(nameNode);

        Assert.AreEqual(charset, response.WebResponse.Headers["X-accept-charset"]);
        Assert.AreEqual("ÆØÅúï´`'", nameNode.InnerText);
      }
    }
  }
}

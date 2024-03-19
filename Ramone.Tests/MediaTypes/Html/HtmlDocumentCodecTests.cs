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
        Assert.That(personNode, Is.Not.Null);

        HtmlNode nameNode = personNode.SelectSingleNode("//*[@class=\"Name\"]");
        Assert.That(nameNode, Is.Not.Null);
        Assert.That(nameNode.InnerText, Is.EqualTo("Petrea"));

        HtmlNode addressNode = personNode.SelectSingleNode("//*[@class=\"Address\"]");
        Assert.That(addressNode, Is.Not.Null);
        Assert.That(addressNode.InnerText, Is.EqualTo("At home"));
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
        Assert.That(nameNode, Is.Not.Null);

        Assert.That(response.WebResponse.Headers["X-accept-charset"], Is.EqualTo(charset));
        Assert.That(nameNode.InnerText, Is.EqualTo("ÆØÅúï´`'"));
      }
    }
  }
}

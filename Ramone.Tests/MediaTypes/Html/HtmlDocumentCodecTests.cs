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
      RamoneRequest req = Session.Bind(PersonTemplate, new { name = "Petrea" });

      // Act
      HtmlDocument doc = req.Get<HtmlDocument>().Body;

      // Assert
      HtmlNode personNode = doc.DocumentNode.SelectSingleNode("//*[@class=\"Person\"]");
      Assert.IsNotNull(personNode);

      HtmlNode nameNode = personNode.SelectSingleNode("//*[@class=\"Name\"]");
      Assert.IsNotNull(nameNode);

      Assert.AreEqual("Petrea", nameNode.InnerText);
    }
  }
}

using System.Xml;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests.MediaTypes.Xml
{
  [TestFixture]
  public class XmlDocumentCodecTests : TestHelper
  {
    [Test]
    public void CanReadXmlDocument()
    {
      // Arrange
      RamoneRequest req = Session.Bind(DossierTemplate, new { id = 5 });

      // Act
      XmlDocument doc = req.Get<XmlDocument>().Body;

      // Assert
      Assert.IsNotNull(doc.SelectSingleNode("//Dossier"));
      Assert.AreEqual("5", doc.SelectSingleNode("//Dossier/Id").InnerText);
    }
  }
}

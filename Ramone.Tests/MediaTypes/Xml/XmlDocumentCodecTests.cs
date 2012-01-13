using System.Xml;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;
using System;


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


    [Test]
    public void CanWriteXmlDocument()
    {
      // Arrange
      XmlDocument dossierDoc = new XmlDocument();
      dossierDoc.LoadXml("<Dossier><Title>My dossier</Title></Dossier>");

      RamoneRequest request = Session.Request(DossiersUrl);

      // Act
      RamoneResponse<Dossier> response = request.ContentType("application/xml").Post<Dossier>(dossierDoc);

      // Assert
      Dossier createdDossier = response.Body;

      Assert.IsNotNull(createdDossier);
      Assert.AreEqual("My dossier", createdDossier.Title);
    }


    [Test]
    public void CanReadXmlDocumentWithEncoding(
      [Values("UTF-8", "Windows-1252", "UTF-16", "iso-8859-1")] string charset)
    {
      // Arrange
      RamoneRequest req = Session.Bind(EncodingTemplate);

      // Act
      var response = req.AcceptCharset(charset).Get<XmlDocument>();
      XmlDocument doc = response.Body;

      // Assert
      XmlNode nameNode = doc.SelectSingleNode("/html/body");
      Assert.IsNotNull(nameNode);

      Assert.AreEqual(charset, response.Response.Headers["X-accept-charset"]);
      Assert.AreEqual("ÆØÅúï", nameNode.InnerText);
    }
  }
}

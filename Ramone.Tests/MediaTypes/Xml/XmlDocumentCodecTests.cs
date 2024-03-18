using System.Xml;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;
using System;
using Ramone.Tests.Common;
using Ramone.MediaTypes.Xml;


namespace Ramone.Tests.MediaTypes.Xml
{
  [TestFixture]
  public class XmlDocumentCodecTests : TestHelper
  {
    [Test]
    public void CanReadXmlDocument()
    {
      // Arrange
      Request req = Session.Bind(DossierTemplate, new { id = 5 });

      // Act
      using (var r = req.Get<XmlDocument>())
      {
        XmlDocument doc = r.Body;

        // Assert
        Assert.IsNotNull(doc.SelectSingleNode("//Dossier"));
        Assert.That(doc.SelectSingleNode("//Dossier/Id").InnerText, Is.EqualTo("5"));
      }
    }


    [Test]
    public void CanWriteXmlDocument()
    {
      // Arrange
      XmlDocument dossierDoc = new XmlDocument();
      dossierDoc.LoadXml("<Dossier><Title>My dossier</Title></Dossier>");

      Request request = Session.Request(DossiersUrl);

      // Act
      using (Response<Dossier> response = request.ContentType("application/xml").Post<Dossier>(dossierDoc))
      {
        // Assert
        Dossier createdDossier = response.Body;

        Assert.IsNotNull(createdDossier);
        Assert.That(createdDossier.Title, Is.EqualTo("My dossier"));
      }
    }


    [Test]
    public void CanReadXmlDocumentWithEncoding(
      [Values("UTF-8", "Windows-1252", "iso-8859-1")] string charset)
    {
      // Arrange
      Request req = Session.Bind(EncodingTemplate, new { type = "xml" });

      // Act
      using (var response = req.AcceptCharset(charset).Get<XmlDocument>())
      {
        XmlDocument doc = response.Body;

        // Assert
        XmlNode nameNode = doc.SelectSingleNode("/html/body");
        Assert.IsNotNull(nameNode);

        Assert.That(response.WebResponse.Headers["X-accept-charset"], Is.EqualTo(charset));
        Assert.That(nameNode.InnerText, Is.EqualTo("ÆØÅúï´`'"));
      }
    }


    [Test]
    public void CanWriteXmlDocumentWithEncoding(
      [Values("UTF-8", "Windows-1252", "iso-8859-1")] string charsetIn,
      [Values("UTF-8", "Windows-1252", "iso-8859-1")] string charsetOut)
    {
      // Arrange
      XmlDocument doc = new XmlDocument();
      doc.LoadXml("<Text>ÆØÅüî</Text>");

      Request request = Session.Bind(EncodingTemplate, new { type = "xml" });

      // Act
      using (Response<XmlDocument> response = request.AcceptCharset(charsetOut)
                                                      .ContentType("application/xml")
                                                      .Charset(charsetIn)
                                                      .Post<XmlDocument>(doc))
      {
        // Assert
        XmlDocument result = response.Body;

        Assert.IsNotNull(result);
        XmlNode textNode = result.SelectSingleNode("/Text");
        Assert.IsNotNull(textNode);

        Assert.That(response.WebResponse.Headers["X-request-charset"], Is.EqualTo(charsetIn));
        Assert.That(response.WebResponse.Headers["X-accept-charset"], Is.EqualTo(charsetOut));
        Assert.That(textNode.InnerText, Is.EqualTo("ÆØÅüî"));
      }
    }


    [Test]
    public void CanReadHtmlWithDOCTYPEAsXmlDocument()
    {
      // Arrange
      Request req = Session.Bind(Constants.HtmlPath);

      // Act
      using (var r = req.Get<XmlDocument>())
      {
        XmlDocument doc = r.Body;

        // Assert
        Assert.IsNotNull(doc);
      }
    }


    [Test]
    public void WhenReadingXmlItChecksXmlConfiguration()
    {
      // Arrange
      Request req = Session.Bind(Constants.HtmlPath);

      XmlReaderSettings settings = new XmlReaderSettings();
      settings.DtdProcessing = DtdProcessing.Prohibit;
      XmlConfiguration.XmlReaderSettings = settings;

      AssertThrows<InvalidOperationException>(() => { using (var resp = req.Get<XmlDocument>()) { resp.Body.Clone(); } });
    }
  }
}

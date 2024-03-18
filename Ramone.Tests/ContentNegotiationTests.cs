using NUnit.Framework;
using Ramone.Tests.Common;
using System;
using System.Xml;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests
{
  [TestFixture]
  public class ContentNegotiationTests : TestHelper
  {
    [Test]
    public void CanGetCatUsingDefaultContentType()
    {
      // Arrange
      Request catReq = Session.Bind(CatTemplate, new { name = "Fiona" });

      // Act
      using (var c = catReq.Get<Cat>())
      {
        // Assert
        Assert.That(c.Body.Name, Is.EqualTo("Fiona"));
      }
    }


    [Test]
    public void CanGetCatAsHtml()
    {
      // Arrange
      Request catReq = Session.Bind(CatTemplate, new { name = "Fiona" });

      // Act
      using (var c = catReq.Accept("text/html").Get<Cat>())
      {
        // Assert
        Assert.That(c.Body.Name, Is.EqualTo("<html><body><p>Fiona</p></body></html>"));
      }
    }


    [Test]
    public void CanGetCatAsHtml_AsyncEvent()
    {
      // Arrange
      Request catReq = Session.Bind(CatTemplate, new { name = "Fiona" });

      // Act
      bool? ok = null;
      TestAsyncEvent(wh =>
      {
        catReq.Accept("text/html").AsyncEvent().Get<Cat>(response => 
        {
          ok = ("<html><body><p>Fiona</p></body></html>" == response.Body.Name);
          wh.Set();
        });
      });

      // Assert
      Assert.That(ok, Is.EqualTo(true));
    }


    [Test]
    public void CanGetCatAsText()
    {
      // Arrange
      Request catReq = Session.Bind(CatTemplate, new { name = "Fiona" });

      // Act
      using (var c = catReq.Accept("text/plain").Get<Cat>())
      {
        // Assert
        Assert.That(c.Body.Name, Is.EqualTo("Fiona"));
      }
    }


    [Test]
    public void CanGetCatAsTextWithShorthandNotation()
    {
      // Arrange
      Request catReq = Session.Bind(CatTemplate, new { name = "Fiona" });

      // Act
      using (var c = catReq.Accept("text/plain").Get<Cat>())
      {
        // Assert
        Assert.That(c.Body.Name, Is.EqualTo("Fiona"));
      }
    }


    [Test]
    public void CanGetCatAsRawXml()
    {
      // Arrange
      Request catReq = Session.Bind(CatTemplate, new { name = "Fiona" });

      // Act
      using (var r = catReq.Accept("application/xml").Get<XmlDocument>())
      {
        XmlDocument c = r.Body;

        // Assert
        Assert.IsNotNull(c);
        XmlNode nameNode = c.SelectSingleNode("//Cat/Name");
        Assert.IsNotNull(nameNode);
        Assert.That(nameNode.InnerText, Is.EqualTo("Fiona"));
      }
    }


    [Test]
    public void WhenPostingWithoutSpecifyingContentTypeItSelectsRandomCodec()
    {
      // Arrange
      Cat c1 = new Cat { Name = "Monster Baby" };
      Request catReq = Session.Bind(CatTemplate, new { name = "Fiona" });

      // Act + Assert
      using (var c2 = catReq.Post<Cat>(c1))
        Assert.That(c2.Body.Name, Is.EqualTo("Fiona"));
    }


    [Test]
    public void CanPostCatAsText()
    {
      // Arrange
      Cat c = new Cat { Name = "Monster Baby" };
      Request catReq = Session.Bind(CatTemplate, new { name = "Fiona" });

      // Act + Assert
      catReq.ContentType("text/plain").Post<Cat>(c).Dispose();
    }


    [Test]
    public void CanPostCatAsHtml()
    {
      // Arrange
      Cat c = new Cat { Name = "Monster Baby" };
      Request catReq = Session.Bind(CatTemplate, new { name = "Fiona" });

      // Act + Assert
      catReq.ContentType("text/html").Post<Cat>(c).Dispose();
    }


    [Test]
    public void CanSpecifyAcceptAsGenericWithoutMediaType()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      using (var dossier = dossierReq.Accept<Dossier>().Get())
      {
        // Assert
        Assert.IsNotNull(dossier.Body);
      }
    }


    [Test]
    public void CanSpecifyAcceptAsGenericWithMediaType()
    {
      // Arrange
      Request catReq = Session.Bind(CatTemplate, new { name = "Fiona" });

      // Act
      using (var c = catReq.Accept<Cat>("text/plain").Get())
      {
        // Assert
        Assert.IsNotNull(c.Body);
      }
    }


    [Test]
    public void WhenNoReaderCodecExistsItThrowsInvalidOperation()
    {
      AssertThrows<InvalidOperationException>(() => Session.Request("http://knoiiukjkjh").Get<UnsupportClass>());
    }


    public class UnsupportClass
    {
    }
  }
}

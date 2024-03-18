using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.Tests.Common.CMS;
using Ramone.Tests.Common;

namespace Ramone.Tests
{
  [TestFixture]
  public class GetTests : TestHelper
  {
    Request DossierReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { id = 8, method = "GET" });
    }


    [Test]
    public void CanGetDossier()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      using (var dossier = dossierReq.Get<Dossier>())
      {
        // Assert
        Assert.That(dossier.Body.Id, Is.EqualTo(8));
        Assert.That(dossier.Body.Title, Is.EqualTo("Dossier no. 8"));
        Assert.IsNotNull(dossier.Body.Links);
      }
    }


    [Test]
    public void CanGetDossierWithDictionaryParams()
    {
      // Arrange
      Dictionary<string, string> p = new Dictionary<string, string>();
      p["id"] = "8";
      Request dossierReq = Session.Bind(DossierTemplate, p);

      // Act
      using (var dossier = dossierReq.Get<Dossier>())
      {
        // Assert
        Assert.That(dossier.Body.Id, Is.EqualTo(8));
        Assert.That(dossier.Body.Title, Is.EqualTo("Dossier no. 8"));
        Assert.IsNotNull(dossier.Body.Links);
      }
    }


    [Test]
    public void CanGetDocument()
    {
      // Arrange
      Request documentReq = Session.Bind(DocumentTemplate, new { id = 1 });

      // Act
      using (var document = documentReq.Get<Document>())
      {
        // Assert
        Assert.That(document.Body.Id, Is.EqualTo(1));
        Assert.That(document.Body.Title, Is.EqualTo("Document no. 1"));
      }
    }


    [Test]
    public void CanGetDossierDocuments()
    {
      // Arrange
      Request dossierDocumentsReq = Session.Bind(DossierDocumentsTemplate, new { id = 8 });

      // Act
      using (var documents = dossierDocumentsReq.Get<DossierDocumentList>())
      {
        // Assert
        Assert.IsNotNull(documents.Body);
        Assert.That(documents.Body.Count, Is.EqualTo(2));
      }
    }


    [Test]
    public void CanGetParty()
    {
      // Arrange
      Request partyReq = Session.Bind(PartyTemplate, new { id = 12 });

      // Act
      using (var party = partyReq.Get<Party>())
      {
        // Assert
        Assert.That(party.Body.Id, Is.EqualTo(12));
        Assert.That(party.Body.FullName, Is.EqualTo("Bart-12"));
        Assert.That(party.Body.EMail, Is.EqualTo("bart-12@foo.bar"));
      }
    }


    [Test]
    public void CanGetDocumentLinksInDossier()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });
      using (var r = dossierReq.Get<Dossier>())
      {
        Dossier dossier = r.Body;

        // Act
        ILink documentsLink = dossier.Links.Select(CMSConstants.DocumentsLinkRelType);

        // Assert
        Assert.IsNotNull(documentsLink);
        Assert.Contains(CMSConstants.DocumentsLinkRelType, documentsLink.RelationTypes.ToList());
      }
    }


    [Test]
    public void CanGetAndIgnoreReturnedBody()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      dossierReq.Get().Dispose();
    }


    [Test]
    public void WhenSpecifyingCharsetForGetItThrows()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act + Assert
      AssertThrows<InvalidOperationException>(() => dossierReq.Charset("utf-8").Get());
      AssertThrows<InvalidOperationException>(() => dossierReq.Charset("utf-8").Get<Dossier>());
    }


    [Test]
    public void WhenReadingBodyTwiceItOnlyReadsInputStreamOnce_Untyped()
    {
      // Arrange
      Request req = Session.Bind(CatTemplate, new { name = "Fiona" });

      // Act
      using (Response resp = req.AcceptJson().Get())
      {
        dynamic body1 = resp.Body;
        string name1 = body1.Name;
        dynamic body2 = resp.Body;
        string name2 = body2.Name;

        // Assert
        Assert.That(name1, Is.EqualTo("Fiona"));
        Assert.That(name2, Is.EqualTo("Fiona"));
      }
    }


    [Test]
    public void WhenReadingBodyTwiceItOnlyReadsInputStreamOnce_Typed()
    {
      // Arrange
      Request req = Session.Bind(CatTemplate, new { name = "Fiona" });

      // Act
      using (var resp = req.Get<Cat>())
      {
        Cat body1 = resp.Body;
        string name1 = body1.Name;
        Cat body2 = resp.Body;
        string name2 = body2.Name;

        // Assert
        Assert.That(name1, Is.EqualTo("Fiona"));
        Assert.That(name2, Is.EqualTo("Fiona"));
      }
    }
  }
}

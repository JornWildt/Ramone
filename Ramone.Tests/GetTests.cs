using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests
{
  [TestFixture]
  public class GetTests : TestHelper
  {
    [Test]
    public void CanGetDossier()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      using (var dossier = dossierReq.Get<Dossier>())
      {
        // Assert
        Assert.AreEqual(8, dossier.Body.Id);
        Assert.AreEqual("Dossier no. 8", dossier.Body.Title);
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
        Assert.AreEqual(8, dossier.Body.Id);
        Assert.AreEqual("Dossier no. 8", dossier.Body.Title);
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
        Assert.AreEqual(1, document.Body.Id);
        Assert.AreEqual("Document no. 1", document.Body.Title);
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
        Assert.AreEqual(2, documents.Body.Count);
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
        Assert.AreEqual(12, party.Body.Id);
        Assert.AreEqual("Bart-12", party.Body.FullName);
        Assert.AreEqual("bart-12@foo.bar", party.Body.EMail);
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
  }
}

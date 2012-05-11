using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests.HyperMedia
{
  [TestFixture]
  public class FollowTests : TestHelper
  {
    [Test]
    public void CanFollowRelativeLinksInContextDependentData()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });
      Dossier dossier = dossierReq.Get<Dossier>().Body;

      // Act
      Request documentsReq = dossier.Links.Select(CMSConstants.DocumentsLinkRelType).Follow();
      DossierDocumentList documents = documentsReq.Get<DossierDocumentList>().Body;

      // Assert
      Assert.IsNotNull(documents);
      Assert.AreEqual(2, documents.Count);
    }


    [Test]
    public void CanFollowLinksInGeneral()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });
      Dossier dossier = dossierReq.Get<Dossier>().Body;

      // Act
      DossierDocumentList documents1 = dossier.Links.Select(CMSConstants.DocumentsLinkRelType).Follow(Session).Get<DossierDocumentList>().Body;

      // Assert
      Assert.IsNotNull(documents1);
      Assert.AreEqual(2, documents1.Count);
    }
  }
}

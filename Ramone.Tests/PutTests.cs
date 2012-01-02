using System.ServiceModel.Syndication;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;
using System.Net;


namespace Ramone.Tests
{
  [TestFixture]
  public class PutTests : TestHelper
  {
    Dossier MyDossier = new Dossier
    {
      Id = 15,
      Title = "A new dossier"
    };

    RamoneRequest DossierReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossierReq = Session.Bind(DossierTemplate, new { id = MyDossier.Id });
    }


    [Test]
    public void CanPutAndIgnoreReturnedBody()
    {
      // Act
      RamoneResponse response = DossierReq.Put(MyDossier);

      // Assert
      Assert.IsNotNull(response);
    }


    [Test]
    public void CanPutAndGetResult()
    {
      // Act
      RamoneResponse<Dossier> response = DossierReq.Put<Dossier>(MyDossier);
      Dossier newDossier = response.Body;

      // Assert
      Assert.IsNotNull(newDossier);
    }


    [Test]
    public void CanPutAndGetResultWithAccept()
    {
      // Act
      Dossier newDossier = DossierReq.Accept<Dossier>().Put(MyDossier).Body;

      // Assert
      Assert.IsNotNull(newDossier);
    }


    [Test]
    public void CanPutAndGetResultWithAcceptMediaType()
    {
      // Act
      Dossier newDossier = DossierReq.Accept<Dossier>(CMSConstants.CMSContentType).Put(MyDossier).Body;

      // Assert
      Assert.IsNotNull(newDossier);
    }
  }
}

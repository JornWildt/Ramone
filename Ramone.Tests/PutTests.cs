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
      Dossier newDossier = DossierReq.Accept<Dossier>(CMSConstants.CMSMediaType).Put(MyDossier).Body;

      // Assert
      Assert.IsNotNull(newDossier);
    }


    [Test]
    public void CanPutEmptyBody_Typed()
    {
      // Arrange
      RamoneRequest request = Session.Bind(AnyEchoTemplate);

      // Act
      RamoneResponse<string> response = request.Accept("text/plain").ContentType("application/x-www-url-formencoded").Put<string>();

      // Assert
      Assert.AreEqual(null, response.Body);
    }


    [Test]
    public void CanPutEmptyBody_Untyped()
    {
      // Arrange
      RamoneRequest request = Session.Bind(AnyEchoTemplate);

      // Act
      RamoneResponse response = request.Accept("text/plain").ContentType("application/x-www-url-formencoded").Put();

      // Assert
      Assert.AreEqual(null, response.Body);
    }
  }
}

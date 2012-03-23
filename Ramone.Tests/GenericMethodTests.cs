using System.Net;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests
{
  [TestFixture]
  public class GenericMethodTests : TestHelper
  {
    [Test]
    public void CanExecuteGetWithGenericResult()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      Dossier dossier1 = dossierReq.Execute<Dossier>("GET").Body;
      Dossier dossier2 = dossierReq.Accept<Dossier>().Execute("GET").Body;

      // Make sure method is actually taken from parameter
      AssertThrows<WebException>(() => dossierReq.Execute<Dossier>("UNKNOWN"));

      // Assert
      Assert.AreEqual(8, dossier1.Id);
      Assert.AreEqual(8, dossier2.Id);
    }


    [Test]
    public void CanExecuteGetWithNonGenericResult()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      Resource response1 = dossierReq.Execute("GET");

      // Make sure method is actually taken from parameter
      AssertThrows<WebException>(() => dossierReq.Execute("UNKNOWN"));

      // Assert
      Assert.AreEqual(8, response1.Decode<Dossier>().Id);
    }


    Dossier MyDossier = new Dossier
    {
      Title = "A new dossier"
    };


    [Test]
    public void CanExecutePostWithGenericResult()
    {
      // Arrange
      Request dossiersReq = Session.Request(DossiersUrl);

      // Act
      Dossier dossier1 = dossiersReq.Execute<Dossier>("POST", MyDossier).Body;
      Dossier dossier2 = dossiersReq.Accept<Dossier>().Execute("POST", MyDossier).Body;

      // Make sure method is actually taken from parameter
      AssertThrows<WebException>(() => dossiersReq.Execute<Dossier>("UNKNOWN", MyDossier));

      // Assert
      Assert.AreEqual(999, dossier1.Id);
      Assert.AreEqual(999, dossier2.Id);
    }


    [Test]
    public void CanExecutePostWithNonGenericResult()
    {
      // Arrange
      Request dossiersReq = Session.Request(DossiersUrl);

      // Act
      Resource response = dossiersReq.Execute("POST", MyDossier);
      // Make sure method is actually taken from parameter
      AssertThrows<WebException>(() => dossiersReq.Execute("UNKNOWN", MyDossier));

      // Assert
      Assert.AreEqual(999, response.Decode<Dossier>().Id);
    }
  }
}

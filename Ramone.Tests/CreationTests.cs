using NUnit.Framework;
using Ramone.Tests.Common.CMS;
using System;


namespace Ramone.Tests
{
  [TestFixture]
  public class CreationTests : TestHelper
  {
    [Test]
    public void CanGetCreatedLocationAndBody()
    {
      // Arrange
      Dossier dossier = new Dossier
      {
        Title = "A new dossier"
      };

      RamoneRequest request = Session.Request(DossiersUrl);

      // Act
      RamoneResponse<Dossier> response = request.Post<Dossier>(dossier);
      
      // Assert
      Uri createdDossierLocation = response.CreatedLocation();
      Dossier createdDossier = response.Body;

      Assert.IsNotNull(createdDossierLocation);
      Assert.IsNotNull(createdDossier);
      Assert.AreEqual("A new dossier", createdDossier.Title);
      Assert.AreEqual(999, createdDossier.Id);
    }


    [Test]
    public void WhenCreatedHasNoBodyItFollowsLocation()
    {
      // Arrange
      Dossier dossier = new Dossier
      {
        Title = "Do not return body" // magic string!
      };

      RamoneRequest request = Session.Request(DossiersUrl);

      // Act
      RamoneResponse<Dossier> response = request.Post<Dossier>(dossier);

      // Assert that server does as expected
      Uri createdDossierLocation = response.CreatedLocation();
      Dossier createdDossier = response.Body;

      Assert.IsNotNull(createdDossierLocation);
      Assert.Null(createdDossier);

      // Assert that client does as expected
      createdDossier = response.Created();
      Assert.IsNotNull(createdDossier);
    }
  }
}

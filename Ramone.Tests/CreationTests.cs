using NUnit.Framework;
using Ramone.Tests.Common.CMS;
using System;


namespace Ramone.Tests
{
  [TestFixture]
  public class CreationTests : TestHelper
  {
    [Test]
    public void CanCreateNewDossier()
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
      Uri createdDossierLocation = response.Created();
      Dossier createdDossier = response.Body;

      Assert.IsNotNull(createdDossierLocation);
      Assert.IsNotNull(createdDossier);
      Assert.AreEqual("A new dossier", createdDossier.Title);
      Assert.AreEqual(999, createdDossier.Id);
    }
  }
}

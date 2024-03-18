using System;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;

namespace Ramone.Tests.Core
{
  [TestFixture]
  public class PostCoreTests : TestHelper
  {

    // This test verifies that a POST actually passes data to the server.
    // It turns out that .Net Core does *not* flush the HTTP request stream 
    // when closing it! So by running an already existing test under .Net Core,
    // we can prove and fix that issue.
    [Test]
    public void CanPostSerializedData()
    {
      // Arrange
      Dossier dossier = new Dossier
      {
        Title = "A Core dossier"
      };

      Request request = Session.Request(DossiersUrl);

      // Act
      using (Response<Dossier> response = request.Post<Dossier>(dossier))
      {
        // Assert
        Uri createdDossierLocation = response.CreatedLocation;
        Dossier createdDossier = response.Body;

        Assert.IsNotNull(createdDossierLocation);
        Assert.IsNotNull(createdDossier);
        Assert.That(createdDossier.Title, Is.EqualTo("A Core dossier"));
        Assert.That(createdDossier.Id, Is.EqualTo(999));
      }
    }
  }
}

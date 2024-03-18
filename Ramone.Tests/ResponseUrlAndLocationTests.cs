using System;
using NUnit.Framework;
using Ramone.MediaTypes.Json;
using Ramone.Tests.Common.CMS;

namespace Ramone.Tests
{
  [TestFixture]
  public class ResponseUrlAndLocationTests : TestHelper
  {
    [Test]
    public void CanGetLocationOrResponseUrl()
    {
      // Arrange
      Dossier dossier = new Dossier
      {
        Title = "A new dossier"
      };

      Request request = Session.Request(DossiersUrl);

      // Act
      using (Response<Dossier> response = request.Post<Dossier>(dossier))
      {
        Uri newDossierUrl = new Uri(BaseUrl, CMSConstants.DossierPath.Replace("{id}", response.Body.Id.ToString()));

        // Assert
        Uri location = response.Location;
        Uri responseUrl = response.ResponseUri;
        Uri createdLocation = response.CreatedLocation;

        Assert.That(responseUrl, Is.EqualTo(DossiersUrl));
        Assert.That(location, Is.EqualTo(newDossierUrl));
        Assert.That(createdLocation, Is.EqualTo(newDossierUrl));
      }
    }
  }
}

using System;
using NUnit.Framework;
using Ramone.MediaTypes.Json;
using Ramone.Tests.Common.CMS;

namespace Ramone.Tests
{
  [TestFixture]
  public class CreationTests : TestHelper
  {
    [Test]
    public void CanGetCreatedLocationAndBody_Generic()
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
        // Assert
        Uri createdDossierLocation = response.CreatedLocation;
        Dossier createdDossier = response.Body;

        Assert.That(createdDossierLocation, Is.Not.Null);
        Assert.That(createdDossier, Is.Not.Null);
        Assert.That(createdDossier.Title, Is.EqualTo("A new dossier"));
        Assert.That(createdDossier.Id, Is.EqualTo(999));
      }
    }


    [Test]
    public void CanGetCreatedLocationAndBody_Dynamic()
    {
      // Arrange
      Dossier dossier = new Dossier
      {
        Title = "A new dossier"
      };

      IService service = SetupFixture.CreateDefaultService();
      ISession session = service.NewSession();

      session.Service.CodecManager.AddCodec<object, JsonSerializerCodec>(CMSConstants.CMSMediaType);
      Request request = session.Request(DossiersUrl).AcceptJson();

      // Act
      using (Response response = request.Post(dossier))
      {
        // Assert
        Uri createdDossierLocation = response.CreatedLocation;
        dynamic createdDossier = response.Body;

        Assert.That(createdDossierLocation, Is.Not.Null);
        Assert.That(createdDossier, Is.Not.Null);
        Assert.That("A new dossier", Is.EqualTo(createdDossier.Title));
      }
    }


    [Test]
    public void CanGetCreatedLocationAndBody_AsyncEvent()
    {
      // Arrange
      Dossier dossier = new Dossier
      {
        Title = "A new dossier"
      };

      Request request = Session.Request(DossiersUrl);

      // Act
      TestAsyncEvent(wh =>
        {
          request.AsyncEvent().Post<Dossier>(dossier, response =>
            {
              // Assert
              Uri createdDossierLocation = response.CreatedLocation;
              Dossier createdDossier = response.Body;

              Assert.That(createdDossierLocation, Is.Not.Null);
              Assert.That(createdDossier, Is.Not.Null);
              Assert.That(createdDossier.Title, Is.EqualTo("A new dossier"));
              Assert.That(createdDossier.Id, Is.EqualTo(999));
              wh.Set();
            });
        });
    }


    [Test]
    public void WhenCreatedHasNoBodyItFollowsLocation()
    {
      // Arrange
      Dossier dossier = new Dossier
      {
        Title = "Do not return body" // magic string!
      };

      Request request = Session.Request(DossiersUrl);

      // Act
      using (Response<Dossier> response = request.Post<Dossier>(dossier))
      {
        // Assert that server does as expected
        Uri createdDossierLocation = response.CreatedLocation;
        Dossier createdDossier = response.Body;

        Assert.That(createdDossierLocation, Is.Not.Null);
        Assert.That(createdDossier, Is.Null);

        // Assert that client does as expected
        createdDossier = response.Created();
        Assert.That(createdDossier, Is.Not.Null);
      }
    }
  }
}

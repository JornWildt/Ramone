using NUnit.Framework;
using Ramone.Tests.Common.CMS;
using Ramone.Tests.Common;
using System;


namespace Ramone.Tests
{
  [TestFixture]
  public class PostTests : TestHelper
  {
    Dossier MyDossier = new Dossier
    {
      Title = "A new dossier"
    };

    Request DossierReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { method = "POST", id = 8 });
    }


    [Test]
    public void CanPostAndIgnoreReturnedBody()
    {
      // Act
      using (Response response = DossierReq.Post(MyDossier))
      {
        // Assert
        Assert.That(response, Is.Not.Null);
      }
    }


    [Test]
    public void CanPostAndGetResult()
    {
      // Act
      using (Response<Dossier> response = DossierReq.Post<Dossier>(MyDossier))
      {
        Dossier newDossier = response.Body;

        // Assert
        Assert.That(newDossier, Is.Not.Null);
      }
    }


    [Test]
    public void CanPostAndGetResultWithAccept()
    {
      // Act
      using (var newDossier = DossierReq.Accept<Dossier>().Post(MyDossier))
      {
        // Assert
        Assert.That(newDossier.Body, Is.Not.Null);
      }
    }


    [Test]
    public void CanPostAndGetResultWithAcceptMediaType()
    {
      // Act
      using (var newDossier = DossierReq.Accept<Dossier>(CMSConstants.CMSMediaType).Post(MyDossier))
      {
        // Assert
        Assert.That(newDossier.Body, Is.Not.Null);
      }
    }


    [Test]
    public void CanPostEmptyBody_Typed()
    {
      // Arrange
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      using (Response<string> response = request.Accept("text/plain").ContentType("application/octet-stream").Post<string>())
      {
        // Assert
        Assert.That(response.Body, Is.Null);
      }
    }


    [Test]
    public void CanPostEmptyBodyWhenNoDefaultMediaTypeIsSpecified_Typed()
    {
      // Arrange
      Session.DefaultRequestMediaType = null;
      Session.DefaultResponseMediaType = null;
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      using (Response<string> response = request.Post<string>())
      {
        // Assert
        Assert.That(response.Body, Is.Null);
      }
    }


    [Test]
    public void CanPostEmptyBody_Untyped()
    {
      // Arrange
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      using (Response response = request.Accept("text/plain").ContentType("application/octet-stream").Post())
      {
        // Assert
        Assert.That(response.Body, Is.Null);
      }
    }


    [Test]
    public void CanPostEmptyBodyWhenNoDefaultMediaTypeIsSpecified_Untyped()
    {
      // Arrange
      Session.DefaultRequestMediaType = null;
      Session.DefaultResponseMediaType = null;
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      using (Response response = request.Post())
      {
        // Assert
        Assert.That(response.Body, Is.Null);
      }
    }
  }
}

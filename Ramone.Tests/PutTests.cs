﻿using NUnit.Framework;
using Ramone.Tests.Common.CMS;


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

    Request DossierReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { id = MyDossier.Id, method = "PUT" });
    }


    [Test]
    public void CanPutAndIgnoreReturnedBody()
    {
      // Act
      using (Response response = DossierReq.Put(MyDossier))
      {
        // Assert
        Assert.That(response, Is.Not.Null);
      }
    }


    [Test]
    public void CanPutAndGetResult()
    {
      // Act
      using (Response<Dossier> response = DossierReq.Put<Dossier>(MyDossier))
      {
        Dossier newDossier = response.Body;

        // Assert
        Assert.That(newDossier, Is.Not.Null);
      }
    }


    [Test]
    public void CanPutAndGetResultWithAccept()
    {
      // Act
      using (var newDossier = DossierReq.Accept<Dossier>().Put(MyDossier))
      {
        // Assert
        Assert.That(newDossier.Body, Is.Not.Null);
      }
    }


    [Test]
    public void CanPutAndGetResultWithAcceptMediaType()
    {
      // Act
      using (var newDossier = DossierReq.Accept<Dossier>(CMSConstants.CMSMediaType).Put(MyDossier))
      {
        // Assert
        Assert.That(newDossier.Body, Is.Not.Null);
      }
    }


    [Test]
    public void CanPutEmptyBody_Typed()
    {
      // Arrange
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      using (Response<string> response = request.Accept("text/plain").ContentType("application/x-www-url-formencoded").Put<string>())
      {
        // Assert
        Assert.That(response.Body, Is.Null);
      }
    }


    [Test]
    public void CanPutEmptyBody_Untyped()
    {
      // Arrange
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      using (Response response = request.Accept("text/plain").ContentType("application/x-www-url-formencoded").Put())
      {
        // Assert
        Assert.That(response.Body, Is.Null);
      }
    }
  }
}

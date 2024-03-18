﻿using NUnit.Framework;
using System;


namespace Ramone.Tests
{
  [TestFixture]
  public class AdditionalMethodsTests : TestHelper
  {
    [Test]
    public void CanDoHead()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      using (Response response = dossierReq.Head())
      {
        // Assert
        Assert.IsNotNull(response);
        Assert.That(response.Headers["X-ExtraHeader"], Is.EqualTo("1"));
      }
    }


    [Test]
    public void CanDoHead_AsyncEvent()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      TestAsyncEvent(wh =>
      {
        // Act
        dossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .Head(response =>
          {
            // Assert
            Assert.That(response.Headers["X-ExtraHeader"], Is.EqualTo("1"));
            wh.Set();
          });
      });
    }


    [Test]
    public void CanDoAsyncEventHeadWithEmptyHandler()
    {
      // Arrange
      Request dossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { id = 8, method = "HEAD" });

      TestAsyncEvent(wh =>
      {
        // Act
        dossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() => wh.Set())
          .Head();
      });
    }


    [Test]
    public void CanDoAsyncEventHeadWithNullHandler()
    {
      // Arrange
      Request dossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { id = 8, method = "HEAD" });

      TestAsyncEvent(wh =>
      {
        // Act
        dossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() => wh.Set())
          .Head(null);
      });
    }
  }
}

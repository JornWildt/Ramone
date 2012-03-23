using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.Tests.Common.CMS;
using System.Collections.Generic;
using System;


namespace Ramone.Tests
{
  [TestFixture]
  public class SubmitTests : TestHelper
  {
    [Test]
    public void CanRememberGetForNextSubmit_generic()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      Dossier dossier = dossierReq.Method("Get").Submit<Dossier>().Body;

      // Assert
      Assert.AreEqual(8, dossier.Id);
    }


    [Test]
    public void WhenNoMethodIsSetThenSubmitThrows_generic()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      AssertThrows<InvalidOperationException>(() => dossierReq.Submit<Dossier>());
    }


    [Test]
    public void CanRememberGetForNextSubmit_untyped()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      Dossier dossier = dossierReq.Method("Get").Submit().Decode<Dossier>();

      // Assert
      Assert.AreEqual(8, dossier.Id);
    }


    [Test]
    public void WhenNoMethodIsSetThenSubmitThrows_untyped()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      AssertThrows<InvalidOperationException>(() => dossierReq.Submit());
    }
  }
}

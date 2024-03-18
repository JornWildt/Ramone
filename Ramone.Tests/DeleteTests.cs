using System;
using System.Net;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests
{
  [TestFixture]
  public class DeleteTests : TestHelper
  {
    Request DossierReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { id = 8, method = "DELETE" });
    }


    [Test]
    public void CanDeleteAndIgnoreReturnedBody()
    {
      // Act
      using (Response response = DossierReq.Delete())
      {
        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
      }
    }


    [Test]
    public void CanDeleteAndGetResult()
    {
      // Act
      using (Response<string> response = DossierReq.Delete<string>())
      {
        // Assert
        Assert.That(response.Body, Is.EqualTo("Deleted, yup!"));
      }
    }


    [Test]
    public void CanDeleteAndGetResultWithAccept()
    {
      // Act
      using (var r = DossierReq.Accept<string>().Delete())
      {
        // Assert
        Assert.That(r.Body, Is.EqualTo("Deleted, yup!"));
      }
    }


    [Test]
    public void CanDeleteAndGetResultWithAcceptMediaType()
    {
      // Act
      using (var r = DossierReq.Accept<string>("text/plain").Delete())
      {
        // Assert
        Assert.That(r.Body, Is.EqualTo("Deleted, yup!"));
      }
    }


    [Test]
    public void WhenSpecifyingCharsetForDeleteItThrows()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act + Assert
      AssertThrows<InvalidOperationException>(() => dossierReq.Charset("utf-8").Delete());
      AssertThrows<InvalidOperationException>(() => dossierReq.Charset("utf-8").Delete<Dossier>());
    }
  }
}

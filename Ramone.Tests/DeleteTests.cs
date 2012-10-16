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
      DossierReq = Session.Bind(DossierTemplate, new { id = 8 });
    }


    [Test]
    public void CanDeleteAndIgnoreReturnedBody()
    {
      // Act
      using (Response response = DossierReq.Delete())
      {
        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
      }
    }


    [Test]
    public void CanDeleteAndGetResult()
    {
      // Act
      using (Response<string> response = DossierReq.Delete<string>())
      {
        // Assert
        Assert.AreEqual("Deleted, yup!", response.Body);
      }
    }


    [Test]
    public void CanDeleteAndGetResultWithAccept()
    {
      // Act
      using (var r = DossierReq.Accept<string>().Delete())
      {
        // Assert
        Assert.AreEqual("Deleted, yup!", r.Body);
      }
    }


    [Test]
    public void CanDeleteAndGetResultWithAcceptMediaType()
    {
      // Act
      using (var r = DossierReq.Accept<string>("text/plain").Delete())
      {
        // Assert
        Assert.AreEqual("Deleted, yup!", r.Body);
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

using System;
using System.Net;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests
{
  [TestFixture]
  public class DeleteTests : TestHelper
  {
    RamoneRequest DossierReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossierReq = Session.Bind(DossierTemplate, new { id = 8 });
    }


    [Test]
    public void CanDeleteAndIgnoreReturnedBody()
    {
      // Act
      RamoneResponse response = DossierReq.Delete();

      // Assert
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }


    [Test]
    public void CanDeleteAndGetResult()
    {
      // Act
      RamoneResponse<string> response = DossierReq.Delete<string>();

      // Assert
      Assert.AreEqual("Deleted, yup!", response.Body);
    }


    [Test]
    public void CanDeleteAndGetResultWithAccept()
    {
      // Act
      string text = DossierReq.Accept<string>().Delete().Body;

      // Assert
      Assert.AreEqual("Deleted, yup!", text);
    }


    [Test]
    public void CanDeleteAndGetResultWithAcceptMediaType()
    {
      // Act
      string text = DossierReq.Accept<string>("text/plain").Delete().Body;

      // Assert
      Assert.AreEqual("Deleted, yup!", text);
    }


    [Test]
    public void WhenSpecifyingCharsetForDeleteItThrows()
    {
      // Arrange
      RamoneRequest dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act + Assert
      AssertThrows<InvalidOperationException>(() => dossierReq.Charset("utf-8").Delete());
      AssertThrows<InvalidOperationException>(() => dossierReq.Charset("utf-8").Delete<Dossier>());
    }
  }
}

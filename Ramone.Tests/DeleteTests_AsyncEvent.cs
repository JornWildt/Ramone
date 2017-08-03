using System;
using System.Net;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests
{
  [TestFixture]
  public class DeleteTests_AsyncEvent : TestHelper
  {
    Request DossierReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { id = 8, method = "DELETE" });
    }


    [Test]
    public void CanDeleteAndGetResult_Typed_AsyncEvent()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .Delete<string>(
          r =>
          {
            // Assert
            Assert.AreEqual("Deleted, yup!", r.Body);
            wh.Set();
          });
      });
    }


    [Test]
    public void CanDeleteAndGetResult_Untyped_AsyncEvent()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .Delete(
          r =>
          {
            // Assert
            Assert.AreEqual("Deleted, yup!", r.Decode<string>());
            wh.Set();
          });
      });
    }


    #region DELETE with empty/null callbacks

    [Test]
    public void CanDeleteAsyncEventWithoutHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Delete();
      });
    }


    [Test]
    public void CanDeleteAsyncEventWithoutHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Delete<string>();
      });
    }


    [Test]
    public void CanDeleteAsyncEventWithNullHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Delete(null);
      });
    }


    [Test]
    public void CanDeleteAsyncEventWithNullHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Delete<string>(null);
      });
    }

    #endregion
  }
}

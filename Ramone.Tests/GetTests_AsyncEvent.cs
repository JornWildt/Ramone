using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Ramone.Tests.Common;
using Ramone.Tests.Common.CMS;

namespace Ramone.Tests
{
  [TestFixture]
  public class GetTests_AsyncEvent : TestHelper
  {
    Request DossierReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { id = 8, method = "GET" });
    }


    [Test]
    public void WhenGettingAsyncEventTheRequestIsInFactAsync()
    {
      // Arrange
      Request request = Session.Bind(Constants.SlowPath);
      TimeSpan asyncTime = TimeSpan.MaxValue;
      TimeSpan syncTime = TimeSpan.MinValue;
      SlowResource result = null;

      TestAsyncEvent(wh =>
      {
        DateTime t1 = DateTime.Now;

        // Act
        request.AsyncEvent()
          .OnComplete(() => wh.Set())
          .Get(response =>
          {
            syncTime = DateTime.Now - t1;
            result = response.Decode<SlowResource>();
          });

        asyncTime = DateTime.Now - t1;
      });

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Time, Is.EqualTo(4));
      Assert.That(syncTime, Is.GreaterThan(TimeSpan.FromSeconds(3)), "Request takes at least 4 seconds - 3 should be a safe test");
      Assert.That(asyncTime, Is.LessThan(TimeSpan.FromSeconds(1)), "Async should be instantaneous - 1 second should be safe");
    }


    [Test]
    public void CanGetDossier_Typed_AsyncEvent()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      TestAsyncEvent(wh =>
      {
        // Act
        dossierReq.AsyncEvent().Get<Dossier>(dossier =>
        {
          Assert.That(dossier.Body.Id, Is.EqualTo(8));
          Assert.That(dossier.Body.Title, Is.EqualTo("Dossier no. 8"));
          Assert.That(dossier.Body.Links, Is.Not.Null);
          wh.Set();
        });
      });
    }


    [Test]
    public void CanGetDossier_Untyped_AsyncEvent()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      TestAsyncEvent(wh =>
      {
        // Act
        dossierReq.AsyncEvent().Get(response =>
        {
          Dossier dossier = response.Decode<Dossier>();
          Assert.That(dossier.Id, Is.EqualTo(8));
          Assert.That(dossier.Title, Is.EqualTo("Dossier no. 8"));
          Assert.That(dossier.Links, Is.Not.Null);
          wh.Set();
        });
      });
    }


    #region GET with empty/null callbacks

    [Test]
    public void CanGetAsyncEventWithoutHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Get();
      });
    }


    [Test]
    public void CanGetAsyncEventWithoutHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Get<string>();
      });
    }


    [Test]
    public void CanGetAsyncEventWithNullHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Get(null);
      });
    }


    [Test]
    public void CanGetAsyncEventWithNullHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Get<string>(null);
      });
    }

    #endregion
  }
}

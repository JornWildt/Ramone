using NUnit.Framework;
using Ramone.Tests.Common.CMS;
using Ramone.Tests.Common;
using System;
using System.Threading.Tasks;

namespace Ramone.Tests
{
  [TestFixture]
  public class PostTests_AsyncEvent : TestHelper
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
    public void WhenPostingEmptyDataAsyncEventTheRequestIsInFactAsync()
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
          .OnError(error => Assert.Fail())
          .OnComplete(() => wh.Set())
          .Post(response =>
          {
            syncTime = DateTime.Now - t1;
            result = response.Decode<SlowResource>();
          });

        asyncTime = DateTime.Now - t1;
      });

      // Assert
      Assert.IsNotNull(result);
      Assert.That(result.Time, Is.EqualTo(4));
      Assert.Greater(syncTime, TimeSpan.FromSeconds(3), "Request takes at least 4 seconds - 3 should be a safe test");
      Assert.Less(asyncTime, TimeSpan.FromSeconds(1), "Async should be instantaneous - 1 second should be safe");
    }


    [Test]
    public void WhenPostingAsyncEventTheRequestIsInFactAsync()
    {
      // Arrange
      Request request = Session.Bind(Constants.SlowPath).AsJson();
      TimeSpan asyncTime = TimeSpan.MaxValue;
      TimeSpan syncTime = TimeSpan.MinValue;
      SlowResource input = new SlowResource { Time = 10 };
      SlowResource result = null;

      TestAsyncEvent(wh =>
      {
        DateTime t1 = DateTime.Now;

        // Act
        request.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() => wh.Set())
          .Post<SlowResource>(input, response =>
          {
            syncTime = DateTime.Now - t1;
            result = response.Body;
          });

        asyncTime = DateTime.Now - t1;
      });

      // Assert
      Assert.IsNotNull(result);
      Assert.That(result.Time, Is.EqualTo(input.Time));
      Assert.Greater(syncTime, TimeSpan.FromSeconds(3), "Request takes at least 4 seconds - 3 should be a safe test");
      Assert.Less(asyncTime, TimeSpan.FromSeconds(1), "Async should be instantaneous - 1 second should be safe");
    }


    [Test]
    public void CanPostEmptyBody_Typed_AsyncEvent()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.ContentType("application/octet-stream").AsyncEvent()
          .OnError(error => Assert.Fail())
          .Post<string>(r =>
          {
            wh.Set();
          });
      });
    }


    [Test]
    public void CanPostEmptyBody_Untyped_AsyncEvent()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.ContentType("application/octet-stream").AsyncEvent()
          .OnError(error => Assert.Fail())
          .Post(r =>
          {
            wh.Set();
          });
      });
    }


    [Test]
    public void CanPostAsyncEventWithoutHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Post(MyDossier);
      });
    }


    [Test]
    public void CanPostEmptyAsyncEventWithoutHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Post();
      });
    }


    [Test]
    public void CanPostAsyncEventWithoutHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Post<Dossier>(MyDossier);
      });
    }


    [Test]
    public void CanPostEmptyAsyncEventWithoutHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Post<Dossier>();
      });
    }


    [Test]
    public void CanPostAsyncEventWithNullHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Post(MyDossier, null);
      });
    }


    [Test]
    public void CanPostEmptyAsyncEventWithNullHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Post(null);
      });
    }


    [Test]
    public void CanPostAsyncEventWithNullHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Post<Dossier>(MyDossier, null);
      });
    }


    [Test]
    public void CanPostEmptyAsyncEventWithNullHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Post<Dossier>(null);
      });
    }
  }
}

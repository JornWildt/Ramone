using NUnit.Framework;
using Ramone.Tests.Common.CMS;

namespace Ramone.Tests
{
  [TestFixture]
  public class PutTests_AsyncEvent : TestHelper
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
    public void CanPutAndGetResult_AsyncEvent()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent().Put<Dossier>(MyDossier, response =>
        {
          Dossier newDossier = response.Body;

          // Assert
          Assert.That(newDossier, Is.Not.Null);
          wh.Set();
        });
      });
    }


    [Test]
    public void CanPutEmptyBody_Typed_AsyncEvent()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.ContentType("application/octet-stream").AsyncEvent()
          .OnError(error => Assert.Fail())
          .Put<string>(r =>
          {
            wh.Set();
          });
      });
    }


    [Test]
    public void CanPutEmptyBody_Untyped_AsyncEvent()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.ContentType("application/octet-stream").AsyncEvent()
          .OnError(error => Assert.Fail())
          .Put<string>(r =>
          {
            wh.Set();
          });
      });
    }


    #region Tests with empty/null callback handlers

    [Test]
    public void CanPutAsyncEventWithoutHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Put(MyDossier);
      });
    }


    [Test]
    public void CanPutEmptyAsyncEventWithoutHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Put();
      });
    }


    [Test]
    public void CanPutAsyncEventWithoutHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Put<Dossier>(MyDossier);
      });
    }


    [Test]
    public void CanPutEmptyAsyncEventWithoutHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Put<Dossier>();
      });
    }


    [Test]
    public void CanPutAsyncEventWithNullHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Put(MyDossier, null);
      });
    }


    [Test]
    public void CanPutEmptyAsyncEventWithNullHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Put(null);
      });
    }


    [Test]
    public void CanPutAsyncEventWithNullHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Put<Dossier>(MyDossier, null);
      });
    }


    [Test]
    public void CanPutEmptyAsyncEventWithNullHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Put<Dossier>(null);
      });
    }

    #endregion
  }
}

using NUnit.Framework;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests
{
  [TestFixture]
  public class PatchTests : TestHelper
  {
    Request DossierReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { id = 15, method = "PATCH" });
    }


    [Test]
    public void CanPatch_Untyped()
    {
      // Act
      using (Response response = DossierReq.AsFormUrlEncoded().Patch(new { title = "Duh" }))
      {
        // Assert
        Assert.That(response.Decode<Dossier>().Title, Is.EqualTo("Duh: ok"));
      }
    }


    [Test]
    public void CanPatch_Untyped_AsyncEvent()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsFormUrlEncoded().AsyncEvent().Patch(new { title = "Duh" }, response =>
        {
          // Assert
          Assert.That(response.Decode<Dossier>().Title, Is.EqualTo("Duh: ok"));
          wh.Set();
        });
      });
    }


    [Test]
    public void CanPatchAndGetResult_Typed()
    {
      // Act
      using (Response<Dossier> response = DossierReq.AsFormUrlEncoded().Patch<Dossier>(new { title = "Duh" }))
      {
        // Assert
        Assert.That(response.Body.Title, Is.EqualTo("Duh: ok"));
      }
    }


    [Test]
    public void CanPatchAndGetResult_Typed_AsyncEvent()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsFormUrlEncoded().AsyncEvent().Patch<Dossier>(new { title = "Duh" }, response =>
        {
          // Assert
          Assert.That(response.Body.Title, Is.EqualTo("Duh: ok"));
          wh.Set();
        });
      });
    }


    [Test]
    public void CanPatchAndGetResultWithAccept()
    {
      // Act
      using (var title = DossierReq.AsFormUrlEncoded().Accept<Dossier>().Patch(new { title = "Duh" }))
      {
        // Assert
        Assert.That(title.Body.Title, Is.EqualTo("Duh: ok"));
      }
    }


    [Test]
    public void CanPatchAndGetResultWithAcceptMediaType()
    {
      // Act
      using (var title = DossierReq.AsFormUrlEncoded().Accept<Dossier>(CMSConstants.CMSMediaType).Patch(new { title = "Duh" }))
      {
        // Assert
        Assert.That(title.Body.Title, Is.EqualTo("Duh: ok"));
      }
    }


    #region PATCH with null/empty callback handlers

    [Test]
    public void CanPatchAsyncEventWithoutHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsFormUrlEncoded().AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Patch(new { title = "Duh" });
      });
    }


    [Test]
    public void CanPatchEmptyAsyncEventWithoutHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Patch();
      });
    }


    [Test]
    public void CanPatchAsyncEventWithoutHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsFormUrlEncoded().AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Patch<Dossier>(new { title = "Duh" });
      });
    }


    [Test]
    public void CanPatchEmptyAsyncEventWithoutHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Patch<Dossier>();
      });
    }


    [Test]
    public void CanPatchAsyncEventWithNullHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsFormUrlEncoded().AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Patch(new { title = "Duh" }, null);
      });
    }


    [Test]
    public void CanPatchEmptyAsyncEventWithNullHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Patch(null);
      });
    }


    [Test]
    public void CanPatchAsyncEventWithNullHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsFormUrlEncoded().AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Patch<Dossier>(new { title = "Duh" }, null);
      });
    }


    [Test]
    public void CanPatchEmptyAsyncEventWithNullHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Patch<Dossier>(null);
      });
    }

    #endregion
  }
}

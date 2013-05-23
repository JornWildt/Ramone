using System.ServiceModel.Syndication;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;
using System.Net;


namespace Ramone.Tests
{
  [TestFixture]
  public class PutTests : TestHelper
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
    public void CanPutAndIgnoreReturnedBody()
    {
      // Act
      using (Response response = DossierReq.Put(MyDossier))
      {
        // Assert
        Assert.IsNotNull(response);
      }
    }


    [Test]
    public void CanPutAndGetResult()
    {
      // Act
      using (Response<Dossier> response = DossierReq.Put<Dossier>(MyDossier))
      {
        Dossier newDossier = response.Body;

        // Assert
        Assert.IsNotNull(newDossier);
      }
    }


    [Test]
    public void CanPutAndGetResult_Async()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async().Put<Dossier>(MyDossier, response =>
        {
          Dossier newDossier = response.Body;

          // Assert
          Assert.IsNotNull(newDossier);
          wh.Set();
        });
      });
    }


    [Test]
    public void CanPutAndGetResultWithAccept()
    {
      // Act
      using (var newDossier = DossierReq.Accept<Dossier>().Put(MyDossier))
      {
        // Assert
        Assert.IsNotNull(newDossier.Body);
      }
    }


    [Test]
    public void CanPutAndGetResultWithAcceptMediaType()
    {
      // Act
      using (var newDossier = DossierReq.Accept<Dossier>(CMSConstants.CMSMediaType).Put(MyDossier))
      {
        // Assert
        Assert.IsNotNull(newDossier.Body);
      }
    }


    [Test]
    public void CanPutEmptyBody_Typed()
    {
      // Arrange
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      using (Response<string> response = request.Accept("text/plain").ContentType("application/x-www-url-formencoded").Put<string>())
      {
        // Assert
        Assert.IsNull(response.Body);
      }
    }


    [Test]
    public void CanPutEmptyBody_Typed_Async()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.ContentType("application/octet-stream").Async()
          .OnError(error => Assert.Fail())
          .Put<string>(r =>
          {
            wh.Set();
          });
      });
    }


    [Test]
    public void CanPutEmptyBody_Untyped()
    {
      // Arrange
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      using (Response response = request.Accept("text/plain").ContentType("application/x-www-url-formencoded").Put())
      {
        // Assert
        Assert.IsNull(response.Body);
      }
    }


    [Test]
    public void CanPutEmptyBody_Untyped_Async()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.ContentType("application/octet-stream").Async()
          .OnError(error => Assert.Fail())
          .Put<string>(r =>
          {
            wh.Set();
          });
      });
    }


    #region Tests with empty/null callback handlers

    [Test]
    public void CanPutAsyncWithoutHandler()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Put(MyDossier);
      });
    }


    [Test]
    public void CanPutEmptyAsyncWithoutHandler()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Put();
      });
    }


    [Test]
    public void CanPutAsyncWithoutHandler_Typed()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Put<Dossier>(MyDossier);
      });
    }


    [Test]
    public void CanPutEmptyAsyncWithoutHandler_Typed()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Put<Dossier>();
      });
    }


    [Test]
    public void CanPutAsyncWithNullHandler()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Put(MyDossier, null);
      });
    }


    [Test]
    public void CanPutEmptyAsyncWithNullHandler()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Put(null);
      });
    }


    [Test]
    public void CanPutAsyncWithNullHandler_Typed()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Put<Dossier>(MyDossier, null);
      });
    }


    [Test]
    public void CanPutEmptyAsyncWithNullHandler_Typed()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
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

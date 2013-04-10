using System.Net;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests
{
  [TestFixture]
  public class GenericMethodTests : TestHelper
  {
    Request DossierReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { method = "POST", id = 8 });
    }

    
    [Test]
    public void CanExecuteGetWithGenericResult()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      using (var dossier1 = dossierReq.Execute<Dossier>("GET"))
      using (var dossier2 = dossierReq.Accept<Dossier>().Execute("GET"))
      {
        // Make sure method is actually taken from parameter
        AssertThrows<WebException>(() => dossierReq.Execute<Dossier>("UNKNOWN"));

        // Assert
        Assert.AreEqual(8, dossier1.Body.Id);
        Assert.AreEqual(8, dossier2.Body.Id);
      }
    }


    [Test]
    public void CanExecuteGetWithGenericResult_Async()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      TestAsync(wh =>
      {
        // Act
        dossierReq.Async()
          .OnComplete(() => wh.Set())
          .Execute<Dossier>("GET", response =>
        {
          // Assert
          Assert.AreEqual(8, response.Body.Id);
        });
      });
    }


    [Test]
    public void CanExecuteGetWithNonGenericResult()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      using (Response response1 = dossierReq.Execute("GET"))
      {
        // Make sure method is actually taken from parameter
        AssertThrows<WebException>(() => dossierReq.Execute("UNKNOWN"));

        // Assert
        Assert.AreEqual(8, response1.Decode<Dossier>().Id);
      }
    }


    [Test]
    public void CanExecuteGetWithNonGenericResult_Async()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      TestAsync(wh =>
      {
        // Act
        dossierReq.Async()
          .OnComplete(() => wh.Set())
          .Execute("GET", response =>
          {
            // Assert
            Assert.AreEqual(8, response.Decode<Dossier>().Id);
          });
      });
    }


    Dossier MyDossier = new Dossier
    {
      Title = "A new dossier"
    };


    [Test]
    public void CanExecutePostWithGenericResult()
    {
      // Arrange
      Request dossiersReq = Session.Request(DossiersUrl);

      // Act
      using (var r1 = dossiersReq.Execute<Dossier>("POST", MyDossier))
      using (var r2 = dossiersReq.Accept<Dossier>().Execute("POST", MyDossier))
      {
        Dossier dossier1 = r1.Body;
        Dossier dossier2 = r2.Body;

        // Make sure method is actually taken from parameter
        AssertThrows<WebException>(() => dossiersReq.Execute<Dossier>("UNKNOWN", MyDossier));

        // Assert
        Assert.AreEqual(999, dossier1.Id);
        Assert.AreEqual(999, dossier2.Id);
      }
    }


    [Test]
    public void CanExecutePostWithGenericResult_Async()
    {
      // Arrange
      Request dossiersReq = Session.Request(DossiersUrl);

      TestAsync(wh =>
      {
        // Act
        dossiersReq.Async()
          .OnComplete(() => wh.Set())
          .Execute<Dossier>("POST", MyDossier, response =>
          {
            // Assert
            Assert.AreEqual(999, response.Body.Id);
          });
      });
    }


    [Test]
    public void CanExecutePostWithNonGenericResult()
    {
      // Arrange
      Request dossiersReq = Session.Request(DossiersUrl);

      // Act
      using (Response response = dossiersReq.Execute("POST", MyDossier))
      {
        // Make sure method is actually taken from parameter
        AssertThrows<WebException>(() => dossiersReq.Execute("UNKNOWN", MyDossier));

        // Assert
        Assert.AreEqual(999, response.Decode<Dossier>().Id);
      }
    }


    [Test]
    public void CanExecutePostWithNonGenericResult_Async()
    {
      // Arrange
      Request dossiersReq = Session.Request(DossiersUrl);

      TestAsync(wh =>
      {
        // Act
        dossiersReq.Async()
          .OnComplete(() => wh.Set())
          .Execute("POST", MyDossier, response =>
          {
            // Assert
            Assert.AreEqual(999, response.Decode<Dossier>().Id);
          });
      });
    }


    #region EXECUTE-ANY with null/empty callback handlers

    [Test]
    public void CanExecuteAsyncWithoutHandler()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Execute("POST", MyDossier);
      });
    }


    [Test]
    public void CanExecuteEmptyAsyncWithoutHandler()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Execute("POST");
      });
    }


    [Test]
    public void CanExecuteAsyncWithoutHandler_Typed()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Execute<Dossier>("POST", MyDossier);
      });
    }


    [Test]
    public void CanExecuteEmptyAsyncWithoutHandler_Typed()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Execute<Dossier>("POST");
      });
    }


    [Test]
    public void CanExecuteAsyncWithNullHandler()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Execute("POST", MyDossier, null);
      });
    }


    [Test]
    public void CanExecuteEmptyAsyncWithNullHandler()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Execute("POST", null);
      });
    }


    [Test]
    public void CanExecuteAsyncWithNullHandler_Typed()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Execute<Dossier>("POST", MyDossier, null);
      });
    }


    [Test]
    public void CanExecuteEmptyAsyncWithNullHandler_Typed()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Execute<Dossier>("POST", null);
      });
    }

    #endregion
  }
}

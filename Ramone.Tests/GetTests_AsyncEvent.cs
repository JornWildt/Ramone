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
      Assert.IsNotNull(result);
      Assert.AreEqual(4, result.Time);
      Assert.Greater(syncTime, TimeSpan.FromSeconds(3), "Request takes at least 4 seconds - 3 should be a safe test");
      Assert.Less(asyncTime, TimeSpan.FromSeconds(1), "Async should be instantaneous - 1 second should be safe");
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
          Assert.AreEqual(8, dossier.Body.Id);
          Assert.AreEqual("Dossier no. 8", dossier.Body.Title);
          Assert.IsNotNull(dossier.Body.Links);
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
          Assert.AreEqual(8, dossier.Id);
          Assert.AreEqual("Dossier no. 8", dossier.Title);
          Assert.IsNotNull(dossier.Links);
          wh.Set();
        });
      });
    }
  }
}

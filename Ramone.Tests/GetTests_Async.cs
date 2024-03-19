using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Ramone.Tests.Common;
using Ramone.Tests.Common.CMS;

namespace Ramone.Tests
{
  [TestFixture]
  public class GetTests_Async : TestHelper
  {
    Request DossierReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { id = 8, method = "GET" });
    }


    [Test]
    public async Task WhenGettingAsyncTheRequestIsInFactAsync()
    {
      // Arrange
      Request request = Session.Bind(Constants.SlowPath);

      // The (initiation of) GET itself takes zero time - but is expected to work async in the background
      DateTime t1 = DateTime.Now;
      var task1 = request.Async().Get<SlowResource>();
      TimeSpan getTime = DateTime.Now - t1;

      // Wait for the request to finish on the server - the Delay(4) simulates work done in the mean time
      await Task.Delay(TimeSpan.FromSeconds(4));

      // Now await response - this should not take any time at this point as the request should have finished already
      DateTime t2 = DateTime.Now;
      using (var response = await task1)
      {
        var result = response.Body;

        TimeSpan responseTime = DateTime.Now - t2;
        TimeSpan totalTime = DateTime.Now - t1;

        Assert.That(getTime.TotalMilliseconds, Is.LessThan(1000));
        Assert.That(responseTime.TotalMilliseconds, Is.LessThan(1000));
        Assert.That(totalTime.TotalMilliseconds, Is.GreaterThan(4000));
      }
    }




    [Test]
    public async Task CanGetDossier_async()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      using (var dossier = await dossierReq.Async().Get<Dossier>())
      {
        // Assert
        Assert.That(dossier.Body.Id, Is.EqualTo(8));
        Assert.That(dossier.Body.Title, Is.EqualTo("Dossier no. 8"));
        Assert.That(dossier.Body.Links, Is.Not.Null);
      }
    }


    [Test]
    public async Task CanGetDossierWithDictionaryParams_async()
    {
      // Arrange
      Dictionary<string, string> p = new Dictionary<string, string>();
      p["id"] = "8";
      Request dossierReq = Session.Bind(DossierTemplate, p);

      // Act
      using (var dossier = await dossierReq.Async().Get<Dossier>())
      {
        // Assert
        Assert.That(dossier.Body.Id, Is.EqualTo(8));
        Assert.That(dossier.Body.Title, Is.EqualTo("Dossier no. 8"));
        Assert.That(dossier.Body.Links, Is.Not.Null);
      }
    }


    [Test]
    public async Task WhenSpecifyingCharsetForGetItThrows()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act + Assert
      await AssertThrowsAsync<InvalidOperationException>(async () => await dossierReq.Charset("utf-8").Async().Get());
      await AssertThrowsAsync<InvalidOperationException>(async () => await dossierReq.Charset("utf-8").Async().Get<Dossier>());
    }
  }
}

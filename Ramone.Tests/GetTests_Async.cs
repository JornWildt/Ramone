using System;
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

        Assert.Less(getTime.TotalMilliseconds, 1000);
        Assert.Less(responseTime.TotalMilliseconds, 1000);
        Assert.GreaterOrEqual(totalTime.TotalMilliseconds, 4000);
      }
    }
  }
}

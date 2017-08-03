using System.Threading.Tasks;
using NUnit.Framework;
using Ramone.Tests.Common;

namespace Ramone.Tests
{
  [TestFixture]
  public class PutTests_Async : TestHelper
  {
    [Test]
    public async Task WhenPuttingEmptyDataAsyncTheRequestIsInFactAsync()
    {
      await VerifyIsAsync(
        async req => await req.Async().Put<SlowResource>(),
        resp =>
        {
          Assert.IsNotNull(resp.Body);
          Assert.AreEqual(4, resp.Body.Time);
        });
    }


    [Test]
    public async Task WhenPuttingDataAsyncTheRequestIsInFactAsync()
    {
      await VerifyIsAsync(
        async req => await req.AsJson().Async().Put<SlowResource>(new SlowResource { Time = 10 }),
        resp =>
        {
          Assert.IsNotNull(resp.Body);
          Assert.AreEqual(10, resp.Body.Time);
        });
    }
  }
}


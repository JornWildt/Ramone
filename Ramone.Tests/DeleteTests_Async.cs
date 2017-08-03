using System.Threading.Tasks;
using NUnit.Framework;
using Ramone.Tests.Common;

namespace Ramone.Tests
{
  [TestFixture]
  public class DeleteTests_Async : TestHelper
  {
    [Test]
    public async Task WhenDeletingAsyncTheRequestIsInFactAsync()
    {
      await VerifyIsAsync(
        async req => await req.Async().Delete<SlowResource>(),
        resp =>
        {
          Assert.IsNotNull(resp.Body);
          Assert.AreEqual(4, resp.Body.Time);
        });
    }
  }
}


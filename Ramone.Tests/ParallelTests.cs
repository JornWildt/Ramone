using System.Threading.Tasks;
using NUnit.Framework;
using Ramone.Tests.Common;

namespace Ramone.Tests
{
  [TestFixture]
  public class ParallelTests : TestHelper
  {
    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
    }


    [Test]
    public void CanRunMultipleRequestsInParallel()
    {
      // Sometime this would fail with a null reference in ConnectionStatistics because of multi threaded access to a dictionary
      Parallel.Invoke(DoRequest, DoRequest, DoRequest, DoRequest, DoRequest, DoRequest, DoRequest, DoRequest, DoRequest, DoRequest);
    }


    private void DoRequest()
    {
      Request req = Session.Bind(Constants.SlowPath, new { sec = 1 });
      using (var respo = req.Get())
      {
      }
    }
  }
}

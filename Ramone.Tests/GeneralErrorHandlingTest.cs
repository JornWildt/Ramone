using NUnit.Framework;
using System.Net;


namespace Ramone.Tests
{
  [TestFixture]
  public class GeneralErrorHandlingTest : TestHelper
  {
    [Test]
    public void WhenServerReturns500ItThrows()
    {
      AssertThrows<RamoneException>(
        () => ErrorEndpoint.Get<string>(new { code = 500, description = "Internal Server Error" }),
        (e) => e.Response.StatusCode == HttpStatusCode.InternalServerError);
    }
  }
}

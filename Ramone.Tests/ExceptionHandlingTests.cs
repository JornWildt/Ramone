using HtmlAgilityPack;
using NUnit.Framework;


namespace Ramone.Tests
{
  [TestFixture]
  public class ExceptionHandlingTests : TestHelper
  {
    [Test]
    public void WhenCatchingAuthorizationExceptionItAllowsToDeserializeContent()
    {
      try
      {
        Session.Request(BasicAuthUrl).Get<string>();
        Assert.Fail("Missing exception.");
      }
      catch (NotAuthorizedException ex)
      {
        HtmlDocument error = ex.Response.AsRamoneResponse<HtmlDocument>(Session).Body;
        Assert.IsNotNull(error);
      }
    }
  }
}

using System.Linq;
using NUnit.Framework;
using Ramone.Tests.Common;


namespace Ramone.Tests
{
  [TestFixture]
  public class UserAgentTests : TestHelper
  {
    [Test]
    public void WhenSendingRequestItPassesUserAgent()
    {
      // Arrange
      Session.UserAgent = "AgentTest/007";
      Request req = Session.Request(HeaderListUrl);

      // Act
      using (var r = req.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers, Is.Not.Null);
        Assert.That(headers.Exists(h => h == "User-Agent: AgentTest/007"), Is.True);
      }
    }
  }
}

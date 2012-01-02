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
      RamoneRequest req = Session.Request(HeaderListUrl);

      // Act
      HeaderList headers = req.Get<HeaderList>().Body;

      // Assert
      Assert.IsNotNull(headers);
      Assert.IsTrue(headers.Exists(h => h == "User-Agent: AgentTest/007"));
    }
  }
}

using System;
using System.Linq;
using NUnit.Framework;
using Ramone.Implementation;
using Ramone.Tests.Codecs;
using Ramone.Tests.Common;


namespace Ramone.Tests
{
  [TestFixture]
  public class CustomHeaderTests : TestHelper
  {
    [Test]
    public void CanAddCustomerHeader()
    {
      // Arrange
      Request request = Session.Request(HeaderListUrl);

      // Act
      using (var r = request.Header("X-Ramone", "123").Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.IsTrue(headers.Any(h => h == "X-Ramone: 123"), "Must contain customer header");
      }
    }
  }
}

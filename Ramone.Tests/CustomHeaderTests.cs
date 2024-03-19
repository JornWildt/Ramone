using System.Linq;
using NUnit.Framework;
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
        Assert.That(headers.Any(h => h == "X-Ramone: 123"), Is.True, "Must contain customer header");
      }
    }


    [Test]
    public void CanAddCustomerHeader_AsyncEvent()
    {
      // Arrange
      Request request = Session.Request(HeaderListUrl);

      // Act
      TestAsyncEvent(wh =>
        {
          request.Header("X-Ramone", "123").AsyncEvent().Get<HeaderList>(response =>
            {
              HeaderList headers = response.Body;

              // Assert
              Assert.That(headers.Any(h => h == "X-Ramone: 123"), Is.True, "Must contain customer header");

              wh.Set();
            });
        });
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;
using Ramone.Tests.Common;


namespace Ramone.Tests
{
  [TestFixture]
  public class IfModifiedSinceTests : TestHelper
  {
    [Test]
    public void CanSetIfModifiedDirectly()
    {
      // Arrange
      DateTime since = new DateTime(2013, 5, 1, 10, 22, 11);
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.IfModifiedSince(since);

      // Act
      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "If-Modified-Since: Wed, 01 May 2013 08:22:11 GMT"), Is.True, "Must contain If-Modified-Since header");
      }
    }


    [Test]
    public void CanSetIfModifiedViaHeader()
    {
      // Arrange
      DateTime since = new DateTime(2013, 5, 1, 10, 22, 11);
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.Header("If-Modified-Since", since.ToUniversalTime().ToString("r"));

      // Act
      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "If-Modified-Since: Wed, 01 May 2013 08:22:11 GMT"), Is.True, "Must contain If-Modified-Since header");
      }
    }


    [Test]
    public void CanSetIfModifiedDirectly_AsyncEvent()
    {
      // Arrange
      DateTime since = new DateTime(2013, 5, 1, 10, 22, 11);
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.IfModifiedSince(since);

      // Act
      TestAsyncEvent(wh =>
      {
        request.AsyncEvent()
          .Get<HeaderList>(r =>
          {
            HeaderList headers = r.Body;

            // Assert
            Assert.That(headers.Any(h => h == "If-Modified-Since: Wed, 01 May 2013 08:22:11 GMT"), Is.True, "Must contain If-Modified-Since header");
            wh.Set();
          });
      });
    }


    [Test]
    public void CanSetIfModifiedViaHeader_AsyncEvent()
    {
      // Arrange
      DateTime since = new DateTime(2013, 5, 1, 10, 22, 11);
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.Header("If-Modified-Since", since.ToUniversalTime().ToString("r"));

      // Act
      TestAsyncEvent(wh =>
      {
        request.AsyncEvent()
          .Get<HeaderList>(r =>
          {
            HeaderList headers = r.Body;

            // Assert
            Assert.That(headers.Any(h => h == "If-Modified-Since: Wed, 01 May 2013 08:22:11 GMT"), Is.True, "Must contain If-Modified-Since header");
            wh.Set();
          });
      });
    }
  }
}

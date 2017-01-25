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
  public class IfUnmodifiedSinceTests : TestHelper
  {
    [Test]
    public void CanSetIfUnmodifiedDirectly()
    {
      // Arrange
      DateTime since = new DateTime(2013, 5, 1, 10, 22, 11);
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.IfUnmodifiedSince(since);

      // Act
      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.IsTrue(headers.Any(h => h == "If-Unmodified-Since: Wed, 01 May 2013 08:22:11 GMT"), "Must contain If-Unmodified-Since header");
      }
    }


    [Test]
    public void CanSetIfUnmodifiedViaHeader()
    {
      // Arrange
      DateTime since = new DateTime(2013, 5, 1, 10, 22, 11);
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.Header("If-Unmodified-Since", since.ToUniversalTime().ToString("r"));

      // Act
      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.IsTrue(headers.Any(h => h == "If-Unmodified-Since: Wed, 01 May 2013 08:22:11 GMT"), "Must contain If-Unmodified-Since header");
      }
    }


    [Test]
    public void CanSetIfUnmodifiedDirectly_AsyncEvent()
    {
      // Arrange
      DateTime since = new DateTime(2013, 5, 1, 10, 22, 11);
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.IfUnmodifiedSince(since);

      // Act
      TestAsyncEvent(wh =>
      {
        request.AsyncEvent()
          .Get<HeaderList>(r =>
          {
            HeaderList headers = r.Body;

            // Assert
            Assert.IsTrue(headers.Any(h => h == "If-Unmodified-Since: Wed, 01 May 2013 08:22:11 GMT"), "Must contain If-Unmodified-Since header");
            wh.Set();
          });
      });
    }


    [Test]
    public void CanSetIfUnmodifiedViaHeader_AsyncEvent()
    {
      // Arrange
      DateTime since = new DateTime(2013, 5, 1, 10, 22, 11);
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.Header("If-Unmodified-Since", since.ToUniversalTime().ToString("r"));

      // Act
      TestAsyncEvent(wh =>
        {
          request.AsyncEvent()
            .Get<HeaderList>(r =>
            {
              HeaderList headers = r.Body;

              // Assert
              Assert.IsTrue(headers.Any(h => h == "If-Unmodified-Since: Wed, 01 May 2013 08:22:11 GMT"), "Must contain If-Unmodified-Since header");
              wh.Set();
            });
        });
    }
  }
}

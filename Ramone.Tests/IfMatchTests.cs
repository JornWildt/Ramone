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
  public class IfMatchTests : TestHelper
  {
    [Test]
    public void CanSetIfMatchDirectly()
    {
      // Arrange
      string tag = "ab12";
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.IfMatch(tag);

      // Act
      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.IsTrue(headers.Any(h => h == "If-Match: ab12"), "Must contain If-Match header");
      }
    }


    [Test]
    public void CanSetIfMatchViaHeader()
    {
      // Arrange
      string tag = "ab12";
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.Header("If-Match", tag);

      // Act
      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.IsTrue(headers.Any(h => h == "If-Match: ab12"), "Must contain If-Match header");
      }
    }


    [Test]
    public void CanSetIfMatchWithStarDirectly()
    {
      // Arrange
      string tag = "*";
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.IfMatch(tag);

      // Act
      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.IsTrue(headers.Any(h => h == "If-Match: *"), "Must contain If-Match header");
      }
    }


    [Test]
    public void CanSetIfMatchWithListDirectly()
    {
      // Arrange
      string tag = "\"ab\", \"qw\"";
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.IfMatch(tag);

      // Act
      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.IsTrue(headers.Any(h => h == "If-Match: \"ab\", \"qw\""), "Must contain If-Match header");
      }
    }


    [Test]
    public void CanSetIfMatchDirectly_AsyncEvent()
    {
      // Arrange
      string tag = "ab12";
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.IfMatch(tag);

      // Act
      TestAsyncEvent(wh =>
      {
        request.AsyncEvent()
          .Get<HeaderList>(r =>
          {
            HeaderList headers = r.Body;

            // Assert
            Assert.IsTrue(headers.Any(h => h == "If-Match: ab12"), "Must contain If-Match header");
            wh.Set();
          });
      });
    }


    [Test]
    public void CanSetIfMatchViaHeader_AsyncEvent()
    {
      // Arrange
      string tag = "ab12";
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.Header("If-Match", tag);

      // Act
      TestAsyncEvent(wh =>
      {
        request.AsyncEvent()
          .Get<HeaderList>(r =>
          {
            HeaderList headers = r.Body;

            // Assert
            Assert.IsTrue(headers.Any(h => h == "If-Match: ab12"), "Must contain If-Match header");
            wh.Set();
          });
      });
    }
  }
}

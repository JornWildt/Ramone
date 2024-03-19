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
  public class IfNoneMatchTests : TestHelper
  {
    [Test]
    public void CanSetIfNoneMatchDirectly()
    {
      // Arrange
      string tag = "ab12";
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.IfNoneMatch(tag);

      // Act
      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "If-None-Match: ab12"), Is.True, "Must contain If-None-Match header");
      }
    }


    [Test]
    public void CanSetIfNoneMatchViaHeader()
    {
      // Arrange
      string tag = "ab12";
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.Header("If-None-Match", tag);

      // Act
      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "If-None-Match: ab12"), Is.True, "Must contain If-None-Match header");
      }
    }


    [Test]
    public void CanSetIfNoneMatchWithStarDirectly()
    {
      // Arrange
      string tag = "*";
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.IfNoneMatch(tag);

      // Act
      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "If-None-Match: *"), Is.True, "Must contain If-None-Match header");
      }
    }


    [Test]
    public void CanSetIfNoneMatchWithListDirectly()
    {
      // Arrange
      string tag = "\"ab\", \"qw\"";
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.IfNoneMatch(tag);

      // Act
      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "If-None-Match: \"ab\", \"qw\""), Is.True, "Must contain If-None-Match header");
      }
    }


    [Test]
    public void CanSetIfNoneMatchDirectly_AsyncEvent()
    {
      // Arrange
      string tag = "ab12";
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.IfNoneMatch(tag);

      // Act
      TestAsyncEvent(wh =>
      {
        request.AsyncEvent()
          .Get<HeaderList>(r =>
          {
            HeaderList headers = r.Body;

            // Assert
            Assert.That(headers.Any(h => h == "If-None-Match: ab12"), Is.True, "Must contain If-None-Match header");
            wh.Set();
          });
      });
    }


    [Test]
    public void CanSetIfNoneMatchViaHeader_Async()
    {
      // Arrange
      string tag = "ab12";
      Request request = Session.Request(HeaderListUrl);

      // Act
      request.Header("If-None-Match", tag);

      // Act
      TestAsyncEvent(wh =>
      {
        request.AsyncEvent()
          .Get<HeaderList>(r =>
          {
            HeaderList headers = r.Body;

            // Assert
            Assert.That(headers.Any(h => h == "If-None-Match: ab12"), Is.True, "Must contain If-None-Match header");
            wh.Set();
          });
      });
    }
  }
}

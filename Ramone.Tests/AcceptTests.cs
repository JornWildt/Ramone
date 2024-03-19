using System.Linq;
using NUnit.Framework;
using Ramone.Tests.Common;
using System;


namespace Ramone.Tests
{
  [TestFixture]
  public class AcceptTests : TestHelper
  {
    ISession AcceptSession;


    protected override void SetUp()
    {
      base.SetUp();
      // Do not include the normal codecs
      AcceptSession = RamoneConfiguration.NewSession(BaseUrl);
    }


    [Test]
    public void WhenNoAcceptHeaderIsSetItFails()
    {
      // Arrange
      Request request = AcceptSession.Bind(HeaderListUrl);

      AssertThrows<InvalidOperationException>(() => request.Get<HeaderList>());
    }


    [Test]
    public void CanSetAcceptHeaderFromString()
    {
      // Arrange
      Request request = AcceptSession.Bind(HeaderListUrl);

      // Act
      request.Accept("application/xml");

      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "Accept: application/xml"), Is.True, "Must contain Accept header");
      }
    }


    [Test]
    public void CanSetMultipleAcceptHeaderFromString()
    {
      // Arrange
      Request request = AcceptSession.Bind(HeaderListUrl);

      // Act
      request.Accept("application/xml").Accept("application/json");

      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "Accept: application/xml, application/json"), Is.True, "Must contain Accept header");
      }
    }


    [Test]
    public void CanSetAcceptHeaderWithQValueFromString()
    {
      // Arrange
      Request request = AcceptSession.Bind(HeaderListUrl);

      // Act
      request.Accept("application/xml", 0.34);

      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "Accept: application/xml; q=0.34"), Is.True, "Must contain Accept header with q-value");
      }
    }


    [Test]
    public void CanSetMultipleAcceptHeaderWithQValueFromString()
    {
      // Arrange
      Request request = AcceptSession.Bind(HeaderListUrl);

      // Act
      request.Accept("application/xml", 0.34).Accept("application/json", 0.12);

      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "Accept: application/xml; q=0.34, application/json; q=0.12"), Is.True, "Must contain Accept header with q-value");
      }
    }


    [Test]
    public void CanSetAcceptHeaderFromMediaType()
    {
      // Arrange
      Request request = AcceptSession.Bind(HeaderListUrl);

      // Act
      request.Accept(MediaType.ApplicationXml);

      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "Accept: application/xml"), Is.True, "Must contain Accept header");
      }
    }


    [Test]
    public void CanSetMultipleAcceptHeaderFromMediaType()
    {
      // Arrange
      Request request = AcceptSession.Bind(HeaderListUrl);

      // Act
      request.Accept(MediaType.ApplicationXml).Accept(MediaType.ApplicationXHtml);

      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "Accept: application/xml, application/xhtml+xml"), Is.True, "Must contain Accept header");
      }
    }


    [Test]
    public void CanSetAcceptHeaderWithQValueFromMediaType()
    {
      // Arrange
      Request request = AcceptSession.Bind(HeaderListUrl);

      // Act
      request.Accept(MediaType.ApplicationXml, 0.34);

      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "Accept: application/xml; q=0.34"), Is.True, "Must contain Accept header with q-value");
      }
    }


    [Test]
    public void CanSetMultipleAcceptHeaderWithQValueFromMediaType()
    {
      // Arrange
      Request request = AcceptSession.Bind(HeaderListUrl);

      // Act
      request.Accept(MediaType.ApplicationXml, 0.89).Accept(MediaType.ApplicationXHtml, 1);

      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "Accept: application/xml; q=0.89, application/xhtml+xml; q=1.00"), Is.True, "Must contain Accept header");
      }
    }


    [Test]
    public void CanSetTypedAcceptHeaderWithQValueFromString()
    {
      // Act
      var request = AcceptSession.Bind(HeaderListUrl).Accept<HeaderList>("application/xml", 0.34);

      using (var r = request.Get())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "Accept: application/xml; q=0.34"), Is.True, "Must contain Accept header with q-value");
      }
    }


    [Test]
    public void ItAlwaysUsesDefaultAcceptFromSession()
    {
      // Arrange
      AcceptSession.AlwaysAccept(MediaType.ApplicationXml);

      // Act
      var request = AcceptSession.Bind(HeaderListUrl);

      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "Accept: application/xml"), Is.True, "Must contain Accept header with q-value");
      }
    }


    [Test]
    public void ItUsesMultipleDefaultAcceptFromSessionWithAdditionalAcceptFromRequest()
    {
      // Arrange
      AcceptSession.AlwaysAccept(MediaType.ApplicationXml, 0.88)
                   .AlwaysAccept(MediaType.TextPlain);

      // Act
      var request = AcceptSession.Bind(HeaderListUrl).Accept(MediaType.ApplicationJson).Accept("text/csv", 0.21);

      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "Accept: application/json, text/csv; q=0.21, application/xml; q=0.88, text/plain"), Is.True, "Must contain Accept header with q-value");
      }
    }


    [Test]
    public void ItUsesMultipleDefaultAcceptFromServiceWithAdditionalAcceptFromRequest()
    {
      // Arrange
      IService service = RamoneConfiguration.NewService(BaseUrl)
                                            .AlwaysAccept(MediaType.ApplicationXml, 0.88)
                                            .AlwaysAccept(MediaType.TextPlain);
      
      ISession session = service.NewSession().AlwaysAccept(MediaType.TextHtml);
      
      // Act
      var request = session.Bind(HeaderListUrl).Accept(MediaType.ApplicationJson).Accept("text/csv", 0.21);

      using (var r = request.Get<HeaderList>())
      {
        HeaderList headers = r.Body;

        // Assert
        Assert.That(headers.Any(h => h == "Accept: application/json, text/csv; q=0.21, application/xml; q=0.88, text/plain, text/html"), Is.True, "Must contain Accept header with q-value");
      }
    }
  }
}

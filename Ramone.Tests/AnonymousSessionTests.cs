using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.MediaTypes.Atom;
using Ramone.MediaTypes.OpenSearch;
using Ramone.Tests.Common;
using System;


namespace Ramone.Tests
{
  [TestFixture]
  public class AnonymousSessionTests : TestHelper
  {
    [Test]
    public void CanCreateRequestFromUriWithoutExplicitSession()
    {
      // Arrange
      Uri uri = ResolveTestUrl(Constants.CatPath);

      // Act
      using (var response = new Request(uri).AcceptJson().Get())
      {
        var body = response.Body;

        // Assert
        Assert.IsNotNull(body);
      }
    }


    [Test]
    public void CanCreateRequestFromStringWithoutExplicitSession()
    {
      // Arrange
      string url = ResolveTestUrl(Constants.CatPath).AbsoluteUri;

      // Act
      using (var response = new Request(url).AcceptJson().Get())
      {
        var body = response.Body;

        // Assert
        Assert.IsNotNull(body);
      }
    }


    [Test]
    public void CanBindRequestFromUriWithoutExplicitSession()
    {
      // Arrange
      Uri uri = ResolveTestUrl(Constants.CatPath);

      // Act
      using (var response = uri.Bind(new { name = "Petra" }).AcceptJson().Get<Cat>())
      {
        var body = response.Body;

        // Assert
        Assert.IsNotNull(body);
        Assert.AreEqual("Petra", body.Name);
      }
    }


    [Test]
    public void CanBindRequestFromStringWithoutExplicitSession()
    {
      // Arrange
      string template = ResolveTestUrl(Constants.CatPath).AbsoluteUri;

      // Act
      using (var response = template.Bind(new { name = "Petra" }).AcceptJson().Get<Cat>())
      {
        var body = response.Body;

        // Assert
        Assert.IsNotNull(body);
        Assert.AreEqual("Petra", body.Name);
      }
    }


    [Test]
    public void CanFollowLinkWithoutExplicitSession()
    {
      // Arrange
      string url = ResolveTestUrl(Constants.CatsPath).AbsoluteUri;
      ILink link = new AtomLink(url, "self", MediaType.ApplicationJson, "Test");

      // Act
      using (var response = link.Follow().AcceptJson().Get<Cat>())
      {
        var body = response.Body;

        // Assert
        Assert.IsNotNull(body);
        Assert.AreEqual("MIAUW", body.Name);
      }
    }


    [Test]
    public void CanBindRequestFromLinkTemplateWithoutExplicitSession()
    {
      // Arrange
      string url = ResolveTestUrl(Constants.CatPath).AbsoluteUri;
      ILinkTemplate template = new OpenSearchUrl
      {
        Template = url,
        MediaType = "application/json",
        RelationType = "results"
      };

      // Act
      using (var response = template.Bind(new { name = "Petra" }).AcceptJson().Get<Cat>())
      {
        var body = response.Body;

        // Assert
        Assert.IsNotNull(body);
        Assert.AreEqual("Petra", body.Name);
      }
    }


    [Test]
    public void CanBindRequestFromUriTemplateWithoutExplicitSession()
    {
      // Arrange
      UriTemplate template = new UriTemplate(Constants.CatPath);

      // Act
      using (var response = template.Bind(BaseUrl, new { name = "Petra" }).AcceptJson().Get<Cat>())
      {
        var body = response.Body;

        // Assert
        Assert.IsNotNull(body);
        Assert.AreEqual("Petra", body.Name);
      }
    }
  }
}

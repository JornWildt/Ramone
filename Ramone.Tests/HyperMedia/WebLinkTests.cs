using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Text;
using Ramone.HyperMedia;

namespace Ramone.Tests.HyperMedia
{
  [TestFixture]
  public class WebLinkTests : TestHelper
  {
    [Test]
    public void WhenConstructingWebLinkItInitializesParameters()
    {
      // Act
      WebLink l = new WebLink();

      // Assert
      Assert.IsNotNull(l.Parameters);
    }


    [Test]
    public void CanConstructWithParameters()
    {
      // Act
      WebLink l1 = new WebLink(new Uri("http://dr.dk"), "http://dr.dk", "abc", "app/x", "hello");

      // Assert
      Assert.That(l1.Parameters["href"], Is.EqualTo("http://dr.dk/"));
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://dr.dk/"));
      Assert.That(l1.Parameters["rel"], Is.EqualTo("abc"));
      Assert.That(l1.RelationType, Is.EqualTo("abc"));
      Assert.That(l1.Parameters["type"], Is.EqualTo("app/x"));
      Assert.That((string)l1.MediaType, Is.EqualTo("app/x"));
      Assert.That(l1.Parameters["title"], Is.EqualTo("hello"));
      Assert.That(l1.Title, Is.EqualTo("hello"));
    }


    [Test]
    public void CanAssignParameters()
    {
      // Arrange
      WebLink l1 = new WebLink();

      // Act
      l1.Parameters["href"] = "http://svt.se/";
      l1.Parameters["rel"] = "xyz";
      l1.Parameters["type"] = "app/y";
      l1.Parameters["title"] = "bonsoir";

      // Assert
      Assert.That(l1.Parameters["href"], Is.EqualTo("http://svt.se/"));
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://svt.se/"));
      Assert.That(l1.Parameters["rel"], Is.EqualTo("xyz"));
      Assert.That(l1.RelationType, Is.EqualTo("xyz"));
      Assert.That(l1.Parameters["type"], Is.EqualTo("app/y"));
      Assert.That((string)l1.MediaType, Is.EqualTo("app/y"));
      Assert.That(l1.Parameters["title"], Is.EqualTo("bonsoir"));
      Assert.That(l1.Title, Is.EqualTo("bonsoir"));
    }


    [Test]
    public void ItDoesIncludeBaseUrlWhenCreatingRelativeLinks()
    {
      // Act
      WebLink l = new WebLink(new Uri("http://dr.dk"), "/xxx/yyy?z=1", "abc", "app/x", "hello");

      // Assert
      Assert.That(l.Parameters["href"], Is.EqualTo("http://dr.dk/xxx/yyy?z=1"));
      Assert.That(l.HRef.AbsoluteUri, Is.EqualTo("http://dr.dk/xxx/yyy?z=1"));
    }


    [Test]
    public void WhenBaseUrlIsNullItHandlesAbsoluteLinks()
    {
      // Act
      WebLink l = new WebLink(null, "http://dr.dk", "abc", "app/x", "hello");

      // Assert
      Assert.That(l.Parameters["href"], Is.EqualTo("http://dr.dk/"));
      Assert.That(l.HRef.AbsoluteUri, Is.EqualTo("http://dr.dk/"));
    }


    [Test]
    public void CanUseLinksForRelValues()
    {
      // Act
      WebLink l1 = new WebLink(new Uri("http://dr.dk"), "http://elfisk.dk next http://example.com prev", "app/x", "hello");

      // Assert
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://dr.dk/"));
      Assert.That(l1.RelationType, Is.EqualTo("http://elfisk.dk next http://example.com prev"));
      Assert.That(l1.RelationTypes.Count(), Is.EqualTo(4));
      Assert.Contains("http://elfisk.dk", l1.RelationTypes.ToList());
      Assert.Contains("next", l1.RelationTypes.ToList());
      Assert.Contains("prev", l1.RelationTypes.ToList());
      Assert.Contains("http://example.com", l1.RelationTypes.ToList());
    }
  }
}

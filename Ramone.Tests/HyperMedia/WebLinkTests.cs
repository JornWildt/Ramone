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
      Assert.AreEqual("http://dr.dk/", l1.Parameters["href"]);
      Assert.AreEqual("http://dr.dk/", l1.HRef);
      Assert.AreEqual("abc", l1.Parameters["rel"]);
      Assert.AreEqual("abc", l1.RelationType);
      Assert.AreEqual("app/x", l1.Parameters["type"]);
      Assert.AreEqual("app/x", (string)l1.MediaType);
      Assert.AreEqual("hello", l1.Parameters["title"]);
      Assert.AreEqual("hello", l1.Title);
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
      Assert.AreEqual("http://svt.se/", l1.Parameters["href"]);
      Assert.AreEqual("http://svt.se/", l1.HRef);
      Assert.AreEqual("xyz", l1.Parameters["rel"]);
      Assert.AreEqual("xyz", l1.RelationType);
      Assert.AreEqual("app/y", l1.Parameters["type"]);
      Assert.AreEqual("app/y", (string)l1.MediaType);
      Assert.AreEqual("bonsoir", l1.Parameters["title"]);
      Assert.AreEqual("bonsoir", l1.Title);
    }


    [Test]
    public void ItDoesIncludeBaseUrlWhenCreatingRelativeLinks()
    {
      // Act
      WebLink l = new WebLink(new Uri("http://dr.dk"), "/xxx/yyy?z=1", "abc", "app/x", "hello");

      // Assert
      Assert.AreEqual("http://dr.dk/xxx/yyy?z=1", l.Parameters["href"]);
      Assert.AreEqual("http://dr.dk/xxx/yyy?z=1", l.HRef);
    }


    [Test]
    public void WhenBaseUrlIsNullItHandlesAbsoluteLinks()
    {
      // Act
      WebLink l = new WebLink(null, "http://dr.dk", "abc", "app/x", "hello");

      // Assert
      Assert.AreEqual("http://dr.dk/", l.Parameters["href"]);
      Assert.AreEqual("http://dr.dk/", l.HRef);
    }


    [Test]
    public void CanUseLinksForRelValues()
    {
      // Act
      WebLink l1 = new WebLink(new Uri("http://dr.dk"), "http://elfisk.dk next http://example.com prev", "app/x", "hello");

      // Assert
      Assert.AreEqual("http://dr.dk/", l1.HRef);
      Assert.AreEqual("http://elfisk.dk next http://example.com prev", l1.RelationType);
      Assert.AreEqual(4, l1.RelationTypes.Count());
      Assert.Contains("http://elfisk.dk", l1.RelationTypes.ToList());
      Assert.Contains("next", l1.RelationTypes.ToList());
      Assert.Contains("prev", l1.RelationTypes.ToList());
      Assert.Contains("http://example.com", l1.RelationTypes.ToList());
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.ServiceModel.Syndication;
using Ramone.MediaTypes.Atom;
using Ramone.HyperMedia;

namespace Ramone.Tests.HyperMedia.Atom
{
  [TestFixture]
  public class AtomLinkTests : TestHelper
  {
    SyndicationFeed Feed;


    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      SyndicationItem item1 = new SyndicationItem("Item 1", "Blah", new Uri("http://link1"));
      SyndicationItem item2 = new SyndicationItem("Item 2", "Blah", new Uri("http://link2"));
      List<SyndicationItem> items = new List<SyndicationItem> { item1, item2 };
      Feed = new SyndicationFeed("My Feed", "Blah", new Uri("http://feed"), items);
      Feed.Links.Add(new SyndicationLink(new Uri("http://edit"), "edit", "Edit feed", "text/html", 0));
    }


    [Test]
    public void CanGetLinksFromFeed()
    {
      // Act
      List<AtomLink> links = Feed.Links().ToList();

      // Assert
      Assert.AreEqual(2, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://feed/", l1.HRef);
      Assert.IsNull(l1.Title);

      ILink l2 = links[1];
      Assert.AreEqual("http://edit/", l2.HRef);
      Assert.AreEqual("Edit feed", l2.Title);
      Assert.Contains("edit", l2.RelationTypes.ToList());
    }


    [Test]
    public void CanGetLinksFromItem()
    {
      // Act
      List<AtomLink> links = Feed.Items.First().Links().ToList();

      // Assert
      Assert.AreEqual(1, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://link1/", l1.HRef);
      Assert.IsNull(l1.Title);
    }


    [Test]
    public void CanGetAtomLinkFromSyndicationLink()
    {
      // Act
      SyndicationLink slink = new SyndicationLink(new Uri("http://edit"), "edit", "Edit feed", "text/html", 0);
      AtomLink link = slink.Link();

      // Assert
      Assert.IsNotNull(link);

      Assert.AreEqual("http://edit/", link.HRef);
      Assert.AreEqual("Edit feed", link.Title);
      Assert.AreEqual("edit", link.RelationType);
      Assert.AreEqual("text/html", (string)link.MediaType);
    }


    [Test]
    public void CanGetAtomLinksFromSyndicationLinks()
    {
      // Act
      List<AtomLink> links = Feed.Links.Links().ToList();

      // Assert
      Assert.AreEqual(2, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://feed/", l1.HRef);
      Assert.IsNull(l1.Title);

      ILink l2 = links[1];
      Assert.AreEqual("http://edit/", l2.HRef);
      Assert.AreEqual("Edit feed", l2.Title);
      Assert.Contains("edit", l2.RelationTypes.ToList());
    }


    [Test]
    public void ItReturnsEmptyListForNullFeed()
    {
      // Act
      List<AtomLink> links = ((SyndicationItem)null).Links().ToList();

      // Assert
      Assert.IsNotNull(links);
    }


    [Test]
    public void ItReturnsEmptyListForNullItem()
    {
      // Act
      List<AtomLink> links = ((SyndicationItem)null).Links().ToList();

      // Assert
      Assert.IsNotNull(links);
    }


    [Test]
    public void CanSelectSyndicationLinks()
    {
      // Act
      ILink link = Feed.Links.Select("edit");

      // Assert
      Assert.IsNotNull(link);
      Assert.AreEqual("http://edit/", link.HRef);
      Assert.AreEqual("Edit feed", link.Title);
      Assert.Contains("edit", link.RelationTypes.ToList());
    }


    [Test]
    public void CanFollowSyndicationLink()
    {
      // Act
      Request request = Feed.Links.First().Follow(Session);

      // Assert
      Assert.IsNotNull(request);
      Assert.AreEqual("http://feed/", request.Url.AbsoluteUri);
    }


    //[Test]
    //public void CanFollowSyndicationLinks()
    //{
    //  // Act
    //  Request request = Feed.Links.Follow(Session, "edit");

    //  // Assert
    //  Assert.IsNotNull(request);
    //  Assert.AreEqual("http://edit/", request.Url.AbsoluteUri);
    //}
  }
}

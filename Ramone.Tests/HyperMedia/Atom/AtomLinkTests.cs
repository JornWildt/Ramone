using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.MediaTypes.Atom;


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
      Assert.That(links.Count, Is.EqualTo(2));

      ILink l1 = links[0];
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://feed/"));
      Assert.IsNull(l1.Title);

      ILink l2 = links[1];
      Assert.That(l2.HRef.AbsoluteUri, Is.EqualTo("http://edit/"));
      Assert.That(l2.Title, Is.EqualTo("Edit feed"));
      Assert.Contains("edit", l2.RelationTypes.ToList());
    }


    [Test]
    public void CanGetLinksFromItem()
    {
      // Act
      List<AtomLink> links = Feed.Items.First().Links().ToList();

      // Assert
      Assert.That(links.Count, Is.EqualTo(1));

      ILink l1 = links[0];
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://link1/"));
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

      Assert.That(link.HRef.AbsoluteUri, Is.EqualTo("http://edit/"));
      Assert.That(link.Title, Is.EqualTo("Edit feed"));
      Assert.That(link.RelationType, Is.EqualTo("edit"));
      Assert.That((string)link.MediaType, Is.EqualTo("text/html"));
    }


    [Test]
    public void CanGetAtomLinksFromSyndicationLinks()
    {
      // Act
      List<AtomLink> links = Feed.Links.Links().ToList();

      // Assert
      Assert.That(links.Count, Is.EqualTo(2));

      ILink l1 = links[0];
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://feed/"));
      Assert.IsNull(l1.Title);

      ILink l2 = links[1];
      Assert.That(l2.HRef.AbsoluteUri, Is.EqualTo("http://edit/"));
      Assert.That(l2.Title, Is.EqualTo("Edit feed"));
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
      ILink link = Feed.Links().Select("edit");

      // Assert
      Assert.IsNotNull(link);
      Assert.That(link.HRef.AbsoluteUri, Is.EqualTo("http://edit/"));
      Assert.That(link.Title, Is.EqualTo("Edit feed"));
      Assert.Contains("edit", link.RelationTypes.ToList());
    }


    [Test]
    public void CanFollowSyndicationLink()
    {
      // Act
      Request request = Feed.Links.First().Follow(Session);

      // Assert
      Assert.IsNotNull(request);
      Assert.That(request.Url.AbsoluteUri, Is.EqualTo("http://feed/"));
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

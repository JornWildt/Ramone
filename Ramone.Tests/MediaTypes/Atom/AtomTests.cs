using System.Linq;
using System.ServiceModel.Syndication;
using NUnit.Framework;
using Ramone.MediaTypes.Atom;
using Ramone.HyperMedia;


namespace Ramone.Tests.MediaTypes.Atom
{
  [TestFixture]
  public class AtomTests : TestHelper
  {
    [Test]
    public void CanGetAtomFeed()
    {
      // Arrange
      RamoneRequest feedReq = Session.Bind(AtomFeedTemplate, new { name = "Mamas feed" });

      // Act
      SyndicationFeed feed = feedReq.Get<SyndicationFeed>().Body;

      // Assert
      Assert.IsNotNull(feed);
      Assert.AreEqual("Mamas feed", feed.Title.Text);
    }


    [Test]
    public void CanGetAtomItem()
    {
      // Arrange
      RamoneRequest itemReq = Session.Bind(AtomItemTemplate, new { feedname = "Mamas feed", itemname = "No. 1" });

      // Act
      SyndicationItem item = itemReq.Get<SyndicationItem>().Body;

      // Assert
      Assert.IsNotNull(item);
      Assert.AreEqual("No. 1", item.Title.Text);
    }


    [Test]
    public void CanFollowAtomLinkList()
    {
      // Arrange
      MyResource r = new MyResource();
      r.Links.Add(new AtomLink("http://dr.dk", "test", "text/html", "DR"));

      // Act
      RamoneRequest request = r.Links.Follow(Session, "test");

      // Assert
      Assert.IsNotNull(request);
      Assert.AreEqual("http://dr.dk/", request.Url.AbsoluteUri);
    }


    class MyResource
    {
      public AtomLinkList Links { get; set; }

      public MyResource()
      {
        Links = new AtomLinkList();
      }
    }
  }
}

using System.Linq;
using System.ServiceModel.Syndication;
using NUnit.Framework;


namespace Ramone.Tests
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
  }
}

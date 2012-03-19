using System.Linq;
using System.ServiceModel.Syndication;
using NUnit.Framework;
using Ramone.MediaTypes.Atom;
using Ramone.HyperMedia;
using System.Xml.Serialization;
using System.IO;
using System;


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


    [Test]
    public void CanSerializeAtomLinksToCorrectFormat()
    {
      // Arrange
      string correctXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<MyResource xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <link href=""http://dr.dk"" rel=""test"" type=""text/html"" title=""DR"" xmlns=""http://www.w3.org/2005/Atom"" />
</MyResource>";

      XmlSerializer serializer = new XmlSerializer(typeof(MyResource));
      MyResource r = new MyResource();
      r.Links.Add(new AtomLink("http://dr.dk", "test", "text/html", "DR"));

      // Act
      using (StringWriter w = new StringWriter())
      {
        serializer.Serialize(w, r);

        string xml = w.ToString();
        Console.Write(xml);
        Assert.AreEqual(correctXml, xml);
      }
    }


    public class MyResource
    {
      [XmlElement(ElementName="link", Namespace=AtomConstants.AtomNamespace)]
      public AtomLinkList Links { get; set; }

      public MyResource()
      {
        Links = new AtomLinkList();
      }
    }
  }
}

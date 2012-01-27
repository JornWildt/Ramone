using System;
using System.IO;
using System.Xml.Serialization;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.HyperMedia.Atom;


namespace Ramone.Tests.HyperMedia
{
  [TestFixture]
  public class LinkTests : TestHelper
  {
    AtomLink Link1;
    AtomLink Link2;
    AtomLinkList Links;
    Resource MyResource;


    protected override void SetUp()
    {
      base.SetUp();
      Link1 = new AtomLink("http://dr.dk/", "tv", "text/html", "Danish Television");
      Link2 = new AtomLink("http://elfisk.dk/", "home", "text/html", "Jorns website");
      Links = new AtomLinkList();
      Links.Add(Link1);
      Links.Add(Link2);
      MyResource = new Resource { Links = Links };
    }


    [Test]
    public void CanGetLinkFromLinkList()
    {
      // Act
      ILink l1 = Links.Link(Link1.RelationshipType);
      ILink l2 = Links.Link(Link2.RelationshipType);

      // Assert
      Assert.IsNotNull(l1);
      Assert.IsNotNull(l2);
      Assert.AreEqual(Link1.HRef, l1.HRef);
      Assert.AreEqual(Link2.HRef, l2.HRef);
    }


    [Test]
    public void CanFollowSimpleLink()
    {
      // Arrange
      ILink link = new AtomLink("http://dr.dk/", "home", "text/html", "Danish Television");

      // Act
      RamoneRequest request = link.Follow(Session);

      // Assert
      Assert.IsNotNull(request);
      Assert.AreEqual(link.HRef, request.Url.AbsoluteUri);
    }


    [Test]
    public void CanFollowFromLinkList()
    {
      // Act
      RamoneRequest r1 = Links.Follow(Session, Link1.RelationshipType);
      RamoneRequest r2 = Links.Follow(Session, Link2.RelationshipType);

      // Assert
      Assert.IsNotNull(r1);
      Assert.IsNotNull(21);
      Assert.AreEqual(Link1.HRef, r1.Url.AbsoluteUri);
      Assert.AreEqual(Link2.HRef, r2.Url.AbsoluteUri);
    }


    //[Test]
    //public void CanFollowSomethingHavingLinks()
    //{
    //  // Act
    //  RamoneRequest r1 = MyResource.Follow(Session, Link1.RelationshipType);
    //  RamoneRequest r2 = MyResource.Follow(Session, Link2.RelationshipType);

    //  // Assert
    //  Assert.IsNotNull(r1);
    //  Assert.IsNotNull(21);
    //  Assert.AreEqual(Link1.HRef, r1.Url.AbsoluteUri);
    //  Assert.AreEqual(Link2.HRef, r2.Url.AbsoluteUri);
    //}


    [Test]
    // Make sure that the hyper-media utilities/interfaces/classes doesn't break the ability to serialize a list of links
    // Also demo how to serialize atom link lists
    public void CanXmlSerializeResourceWithLinks()
    {
      // Arrange
      XmlSerializer serializer = new XmlSerializer(typeof(Resource));
      
      XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
      xmlns.Add("atom", HyperMediaNamespaces.Atom);

      Resource r = new Resource { Links = new AtomLinkList() };
      r.Links.Add(Link1);
      r.Links.Add(Link2);

      // Act
      using (TextWriter w = new StringWriter())
      {
        serializer.Serialize(w, r, xmlns);

        Assert.AreEqual(@"<?xml version=""1.0"" encoding=""utf-16""?>
<Resource xmlns:atom=""http://www.w3.org/2005/Atom"">
  <atom:link href=""http://dr.dk/"" rel=""tv"" type=""text/html"" title=""Danish Television"" />
  <atom:link href=""http://elfisk.dk/"" rel=""home"" type=""text/html"" title=""Jorns website"" />
</Resource>", w.ToString());
      }

      // Success - no exceptions
    }


    public class Resource
    {
      [XmlElement("link", Namespace = HyperMediaNamespaces.Atom)]
      public AtomLinkList Links { get; set; }
    }
  }
}

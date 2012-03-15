using System.IO;
using System.Xml.Serialization;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.MediaTypes.Atom;


namespace Ramone.Tests.HyperMedia
{
  [TestFixture]
  public class LinkTests : TestHelper
  {
    AtomLink Link1;
    AtomLink Link2;
    AtomLink Link3;
    AtomLink Link4;
    AtomLinkList Links;
    Resource MyResource;


    protected override void SetUp()
    {
      base.SetUp();
      Link1 = new AtomLink("http://dr.dk/", "tv", "text/html", "Danish Television");
      Link2 = new AtomLink("http://elfisk.dk/", "home", "text/html", "Jorns website");
      Link3 = new AtomLink("http://dr.dk/atom", "tv", "application/atom+xml", "Danish Television feed");
      Link4 = new AtomLink("http://elfisk.dk/atom", "home", "application/atom+xml", "Jorns website feed");
      Links = new AtomLinkList();
      Links.Add(Link1);
      Links.Add(Link2);
      Links.Add(Link3);
      Links.Add(Link4);
      MyResource = new Resource { Links = Links };
    }


    [Test]
    public void CanGetLinkFromLinkList()
    {
      // Act
      ILink l1a = Links.Select(Link1.RelationshipType);
      ILink l2a = Links.Select(Link2.RelationshipType);
      ILink l1b = Links.Select(Link1.RelationshipType, "text/html");
      ILink l2b = Links.Select(Link2.RelationshipType, "text/html");
      ILink l3 = Links.Select(Link3.RelationshipType, "application/atom+xml");
      ILink l4 = Links.Select(Link4.RelationshipType, "application/atom+xml");

      // Assert
      Assert.IsNotNull(l1a);
      Assert.IsNotNull(l2a);
      Assert.IsNotNull(l1b);
      Assert.IsNotNull(l2b);
      Assert.IsNotNull(l3);
      Assert.IsNotNull(l4);
      Assert.AreEqual(Link1.HRef, l1a.HRef);
      Assert.AreEqual(Link2.HRef, l2a.HRef);
      Assert.AreEqual(Link1.HRef, l1b.HRef);
      Assert.AreEqual(Link2.HRef, l2b.HRef);
      Assert.AreEqual(Link3.HRef, l3.HRef);
      Assert.AreEqual(Link4.HRef, l4.HRef);
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
      RamoneRequest r1a = Links.Follow(Session, Link1.RelationshipType);
      RamoneRequest r2a = Links.Follow(Session, Link2.RelationshipType);
      RamoneRequest r1b = Links.Follow(Session, Link1.RelationshipType, Link1.MediaType);
      RamoneRequest r2b = Links.Follow(Session, Link2.RelationshipType, Link2.MediaType);
      RamoneRequest r3 = Links.Follow(Session, Link3.RelationshipType, Link3.MediaType);
      RamoneRequest r4 = Links.Follow(Session, Link4.RelationshipType, Link4.MediaType);


      // Assert
      Assert.IsNotNull(r1a);
      Assert.IsNotNull(r2a);
      Assert.IsNotNull(r1b);
      Assert.IsNotNull(r2b);
      Assert.IsNotNull(r3);
      Assert.IsNotNull(r4);
      Assert.AreEqual(Link1.HRef, r1a.Url.AbsoluteUri);
      Assert.AreEqual(Link2.HRef, r2a.Url.AbsoluteUri);
      Assert.AreEqual(Link1.HRef, r1b.Url.AbsoluteUri);
      Assert.AreEqual(Link2.HRef, r2b.Url.AbsoluteUri);
      Assert.AreEqual(Link3.HRef, r3.Url.AbsoluteUri);
      Assert.AreEqual(Link4.HRef, r4.Url.AbsoluteUri);
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
      xmlns.Add("atom", AtomConstants.AtomNamespace);

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
      [XmlElement("link", Namespace = AtomConstants.AtomNamespace)]
      public AtomLinkList Links { get; set; }
    }
  }
}

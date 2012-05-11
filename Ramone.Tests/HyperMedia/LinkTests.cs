using System;
using System.IO;
using System.Linq;
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
    AtomLink Link5;
    AtomLink Link6;
    AtomLink Link7;
    AtomLinkList Links;
    Resource MyResource;


    protected override void SetUp()
    {
      base.SetUp();
      Uri baseUrl = new Uri("http://unused");
      Link1 = new AtomLink(baseUrl, "http://dr.dk/", "tv", "text/html", "Danish Television");
      Link2 = new AtomLink(baseUrl, "http://elfisk.dk/", "home", "text/html", "Jorns website");
      Link3 = new AtomLink(baseUrl, "http://dr.dk/atom", "tv", "application/atom+xml", "Danish Television feed");
      Link4 = new AtomLink(baseUrl, "http://elfisk.dk/atom", "home", "application/atom+xml", "Jorns website feed");
      Link5 = new AtomLink(baseUrl, "http://elfisk.dk/abc", "search previous first", "text/html", "Blah 1");
      Link6 = new AtomLink(baseUrl, "http://elfisk.dk/def", "search previous first", "application/atom+xml", "Blah 2");
      Link7 = new AtomLink(baseUrl, "http://elfisk.dk/123", "UPPERCASE lowercase", "text/html", "Blah 3");
      Links = new AtomLinkList();
      Links.Add(Link1);
      Links.Add(Link2);
      Links.Add(Link3);
      Links.Add(Link4);
      Links.Add(Link5);
      Links.Add(Link6);
      Links.Add(Link7);
      MyResource = new Resource { Links = Links };
    }


    [Test]
    public void CanSelectLinkFromLinkList()
    {
      // Act
      ILink l1a = Links.Select(Link1.RelationType);
      ILink l2a = Links.Select(Link2.RelationType);
      ILink l1b = Links.Select(Link1.RelationType, "text/html");
      ILink l2b = Links.Select(Link2.RelationType, "text/html");
      ILink l3 = Links.Select(Link3.RelationType, "application/atom+xml");
      ILink l4 = Links.Select(Link4.RelationType, "application/atom+xml");

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
    public void CanSelectLinkFromMultiRelLinkList()
    {
      // Act
      ILink l5a = Links.Select("search");
      ILink l5b = Links.Select("previous");
      ILink l5c = Links.Select("first");
      ILink l5d = Links.Select("unused");
      ILink l6a = Links.Select("previous", "application/atom+xml");
      ILink l6b = Links.Select("first", "text/html");
      ILink l6c = Links.Select("unused", "application/atom+xml");
      ILink l6d = Links.Select("unused", "text/html");

      // Assert
      Assert.IsNotNull(l5a);
      Assert.IsNotNull(l5b);
      Assert.IsNotNull(l5c);
      Assert.IsNull(l5d);
      Assert.IsNotNull(l6a);
      Assert.IsNotNull(l6b);
      Assert.IsNull(l6c);
      Assert.IsNull(l6d);
      Assert.AreEqual(Link5.HRef, l5a.HRef);
      Assert.AreEqual(Link6.HRef, l6a.HRef);
      Assert.AreEqual(3, Link5.RelationTypes.Count());
      Assert.AreEqual(3, Link6.RelationTypes.Count());
    }


    [Test]
    public void ItComparesCaseInsensitiveWhenLookingUpLink()
    {
      // Act
      ILink l7a = Links.Select("UppERCase");
      ILink l7b = Links.Select("LOWERcasE");

      // Assert
      Assert.IsNotNull(l7a);
      Assert.IsNotNull(l7b);
      Assert.AreEqual(Link7.HRef, l7a.HRef);
      Assert.AreEqual(Link7.HRef, l7b.HRef);
    }


    [Test]
    public void CanFollowSimpleLink()
    {
      // Arrange
      ILink link = new AtomLink(new Uri("http://dr.dk"), "http://dr.dk/", "home", "text/html", "Danish Television");

      // Act
      Request request = link.Follow(Session);

      // Assert
      Assert.IsNotNull(request);
      Assert.AreEqual(link.HRef, request.Url.AbsoluteUri);
    }


    //[Test]
    //public void CanFollowFromLinkList()
    //{
    //  // Act
    //  Request r1a = Links.Follow(Session, Link1.RelationTypes.First());
    //  Request r2a = Links.Follow(Session, Link2.RelationTypes.First());
    //  Request r1b = Links.Follow(Session, Link1.RelationTypes.First(), Link1.MediaType);
    //  Request r2b = Links.Follow(Session, Link2.RelationTypes.First(), Link2.MediaType);
    //  Request r3 = Links.Follow(Session, Link3.RelationTypes.First(), Link3.MediaType);
    //  Request r4 = Links.Follow(Session, Link4.RelationTypes.First(), Link4.MediaType);


    //  // Assert
    //  Assert.IsNotNull(r1a);
    //  Assert.IsNotNull(r2a);
    //  Assert.IsNotNull(r1b);
    //  Assert.IsNotNull(r2b);
    //  Assert.IsNotNull(r3);
    //  Assert.IsNotNull(r4);
    //  Assert.AreEqual(Link1.HRef, r1a.Url.AbsoluteUri);
    //  Assert.AreEqual(Link2.HRef, r2a.Url.AbsoluteUri);
    //  Assert.AreEqual(Link1.HRef, r1b.Url.AbsoluteUri);
    //  Assert.AreEqual(Link2.HRef, r2b.Url.AbsoluteUri);
    //  Assert.AreEqual(Link3.HRef, r3.Url.AbsoluteUri);
    //  Assert.AreEqual(Link4.HRef, r4.Url.AbsoluteUri);
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

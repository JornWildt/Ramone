using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Ramone.MediaTypes.Atom;
using Ramone.Utility;

namespace Ramone.Tests.Utility
{
  [TestFixture]
  public class ContextRegistratorTests : TestHelper
  {
    class Item : IHaveContext
    {
      public Uri BaseUri { get; set; }

      public void RegisterContext(ISession session, Uri baseUrl)
      {
        BaseUri = baseUrl;
      }
    }


    class Record : IContextContainer
    {
      public int X { get; set; }

      public Record R1 { get; set; }

      public List<Record> RecordList { get; set; }

      public Record[] RecordArray { get; set; }

      public Item A { get; set; }

      public AtomLinkList Links { get; set; }
    }


    [Test]
    public void CanRegisterContext()
    {
      // Arrange
      Record src = new Record
      {
        X = 10,
        A = new Item(),
        Links = new AtomLinkList
        {
          new AtomLink("./path", "self", MediaType.ApplicationXml, "Link 1")
        },
        R1 = new Record
        {
          X = 5,
          R1 = null,
          A = new Item(),
          Links = new AtomLinkList
          {
            new AtomLink("./other", "self", MediaType.ApplicationXml, "Link 2")
          }
        },
        RecordList = new List<Record>
        {
          new Record
          {
            X = 1,
            R1 = new Record
            {
              Links = new AtomLinkList
              {
                new AtomLink("./new", "self", MediaType.ApplicationXml, "Link 3")
              }
            },
            A = new Item(),
            Links = new AtomLinkList
            {
              new AtomLink("./sub", "self", MediaType.ApplicationXml, "Link 4")
            }
          }
        },
        RecordArray = new Record[1]
        {
          new Record
          {
            R1 = new Record
            {
              Links = new AtomLinkList
              {
                new AtomLink("./array1", "self", MediaType.ApplicationXml, "Link 4")
              }
            },
            A = new Item(),
            Links = new AtomLinkList
            {
              new AtomLink("./array2", "self", MediaType.ApplicationXml, "Link 6")
            }
          }
        }
      };

      Uri baseUrl = new Uri("http://elfisk.dk");

      // Act
      ContextRegistrator.RegisterContext(Session, baseUrl, src);

      // Assert
      Assert.That(src.X, Is.EqualTo(10));
      Assert.That(src.A.BaseUri, Is.EqualTo(baseUrl));
      Assert.That(src.Links[0].HRef, Is.EqualTo(new Uri(baseUrl, "path")));
      Assert.That(src.R1.A.BaseUri, Is.EqualTo(baseUrl));
      Assert.That(src.R1.Links[0].HRef, Is.EqualTo(new Uri(baseUrl, "other")));
      Assert.That(src.RecordList[0].A.BaseUri, Is.EqualTo(baseUrl));
      Assert.That(src.RecordList[0].Links[0].HRef, Is.EqualTo(new Uri(baseUrl, "sub")));
      Assert.That(src.RecordList[0].R1.Links[0].HRef, Is.EqualTo(new Uri(baseUrl, "new")));
      Assert.That(src.RecordArray[0].A.BaseUri, Is.EqualTo(baseUrl));
      Assert.That(src.RecordArray[0].Links[0].HRef, Is.EqualTo(new Uri(baseUrl, "array2")));
      Assert.That(src.RecordArray[0].R1.Links[0].HRef, Is.EqualTo(new Uri(baseUrl, "array1")));
    }
  }
}

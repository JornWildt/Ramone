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
      Assert.AreEqual(10, src.X);
      Assert.AreEqual(baseUrl, src.A.BaseUri);
      Assert.AreEqual(new Uri(baseUrl, "path"), src.Links[0].HRef);
      Assert.AreEqual(baseUrl, src.R1.A.BaseUri);
      Assert.AreEqual(new Uri(baseUrl, "other"), src.R1.Links[0].HRef);
      Assert.AreEqual(baseUrl, src.RecordList[0].A.BaseUri);
      Assert.AreEqual(new Uri(baseUrl, "sub"), src.RecordList[0].Links[0].HRef);
      Assert.AreEqual(new Uri(baseUrl, "new"), src.RecordList[0].R1.Links[0].HRef);
      Assert.AreEqual(baseUrl, src.RecordArray[0].A.BaseUri);
      Assert.AreEqual(new Uri(baseUrl, "array2"), src.RecordArray[0].Links[0].HRef);
      Assert.AreEqual(new Uri(baseUrl, "array1"), src.RecordArray[0].R1.Links[0].HRef);
    }
  }
}

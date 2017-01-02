using System.Linq;
using System.Text;
using NUnit.Framework;
using Ramone.MediaTypes.Xml;
using Ramone.Tests.Codecs;
using Ramone.Tests.Common;
using Ramone.Tests.Common.CMS;
using Ramone.MediaTypes;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;

namespace Ramone.Tests
{
  [SetUpFixture]
  class SetupFixture
  {
    [OneTimeSetUp]
    public void Setup()
    {
      Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(SetupFixture)).Location);
      TestHelper.TestService = CreateDefaultService();
    }


    public static IService CreateDefaultService()
    {
      IService service = RamoneConfiguration.NewService(TestHelper.BaseUrl);

      service.DefaultEncoding = Encoding.GetEncoding("iso-8859-1");

      ICodecManager cm = service.CodecManager;

      cm.AddCodec<Dossier, XmlSerializerCodec>(CMSConstants.CMSMediaType);
      cm.AddCodec<DossierDocumentList, XmlSerializerCodec>(CMSConstants.CMSMediaType);
      cm.AddCodec<Document, XmlSerializerCodec>(CMSConstants.CMSMediaType);
      cm.AddCodec<Party, XmlSerializerCodec>(CMSConstants.CMSMediaType);

      cm.AddCodec<Cat, CatAsTextCodec>(MediaType.TextPlain);
      cm.AddCodec<Cat, CatAsHtmlCodec>(MediaType.TextHtml);

      cm.AddCodec<Dog1, XmlSerializerCodec>(new MediaType("application/vnd.dog+xml"));
      cm.AddCodec<Dog2, XmlSerializerCodec>(new MediaType("application/vnd.dog+xml"));

      cm.AddCodec<HeaderList, XmlSerializerCodec>(MediaType.ApplicationXml);

      cm.AddCodec<RegisteredClass, XmlSerializerCodec>(MediaType.ApplicationXml);

      return service;
    }


    [OneTimeTearDown]
    public void TearDown()
    {
      IList<ConnectionStatistics.ConnectionInfo> connections = ConnectionStatistics.GetOpenConnections().ToList();
      if (connections.Count > 0)
      {
        foreach (ConnectionStatistics.ConnectionInfo c in connections)
          Console.WriteLine("Open connection to {0} ({1}).", c.Url, c.Method);
        Assert.AreEqual(0, connections.Count, "All connections must have been closed (showing currently number of open connections).");
      }
    }
  }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using Ramone.MediaTypes.Atom;
using Ramone.MediaTypes.Html;
using Ramone.MediaTypes.Xml;
using Ramone.Tests.Common;
using Ramone.Tests.Common.CMS;
using Ramone.Tests.Common.Codecs;

namespace Ramone.Tests.Common
{
  public class SetupFixture
  {
    [OneTimeSetUp]
    public void Setup()
    {
      Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(SetupFixture)).Location);
      AtomInitializer.Initialize();
      HtmlInitializer.Initialize();
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
        Assert.That(connections.Count, Is.EqualTo(0), "All connections must have been closed (showing currently number of open connections).");
      }
    }
  }
}

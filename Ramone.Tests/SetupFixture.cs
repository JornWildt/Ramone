using System.ServiceModel.Syndication;
using System.Xml;
using NUnit.Framework;
using Ramone.MediaTypes.Atom;
using Ramone.MediaTypes.Xml;
using Ramone.Tests.Codecs;
using Ramone.Tests.Common;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests
{
  [SetUpFixture]
  class SetupFixture
  {
    [SetUp]
    public void Setup()
    {
      ICodecManager cm = TestHelper.Settings.CodecManager;

      cm.AddCodec<Dossier>(CMSConstants.CMSContentType, new XmlSerializerCodec<Dossier>());
      cm.AddCodec<DossierDocumentList>(CMSConstants.CMSContentType, new XmlSerializerCodec<DossierDocumentList>());
      cm.AddCodec<Document>(CMSConstants.CMSContentType, new XmlSerializerCodec<Document>());
      cm.AddCodec<Party>(CMSConstants.CMSContentType, new XmlSerializerCodec<Party>());
      cm.AddCodec<HalDossier>("application/hal+xml", new XmlSerializerCodec<HalDossier>());
      
      //cm.AddCodec<string>("application/hal+xml", new StringCodec()());

      cm.AddCodec<Cat>("text/plain", new CatAsTextCodec());
      cm.AddCodec<Cat>("text/html", new CatAsHtmlCodec());

      cm.AddCodec<Dog1>("application/vnd.dog+xml", new XmlSerializerCodec<Dog1>());
      cm.AddCodec<Dog2>("application/vnd.dog+xml", new XmlSerializerCodec<Dog2>());

      cm.AddCodec<HeaderList>("application/xml", new XmlSerializerCodec<HeaderList>());

      cm.AddCodec<XmlDocument>("application/xml", new XmlDocumentCodec());

      cm.AddCodec<string>("text/plain", new TextCodec());
      cm.AddCodec<string>("text/html", new TextCodec());
      cm.AddCodec<string>("text/xml", new TextCodec());

      cm.AddCodec<SyndicationFeed>("application/atom+xml", new AtomFeedCodec());
      cm.AddCodec<SyndicationItem>("application/atom+xml", new AtomItemCodec());
    }
  }
}

using System.ServiceModel.Syndication;
using System.Xml;
using NUnit.Framework;
using Ramone.MediaTypes.Atom;
using Ramone.MediaTypes.Xml;
using Ramone.Tests.Codecs;
using Ramone.Tests.Common;
using Ramone.Tests.Common.CMS;
using Ramone.MediaTypes.MultipartFormData;


namespace Ramone.Tests
{
  [SetUpFixture]
  class SetupFixture
  {
    [SetUp]
    public void Setup()
    {
      ICodecManager cm = TestHelper.TestService.CodecManager;

      cm.AddCodec<Dossier>(CMSConstants.CMSMediaType, new XmlSerializerCodec());
      cm.AddCodec<DossierDocumentList>(CMSConstants.CMSMediaType, new XmlSerializerCodec());
      cm.AddCodec<Document>(CMSConstants.CMSMediaType, new XmlSerializerCodec());
      cm.AddCodec<Party>(CMSConstants.CMSMediaType, new XmlSerializerCodec());
      
      cm.AddCodec<Cat>(MediaType.TextPlain, new CatAsTextCodec());
      cm.AddCodec<Cat>(MediaType.TextHtml, new CatAsHtmlCodec());

      cm.AddCodec<Dog1>(new MediaType("application/vnd.dog+xml"), new XmlSerializerCodec());
      cm.AddCodec<Dog2>(new MediaType("application/vnd.dog+xml"), new XmlSerializerCodec());

      cm.AddCodec<HeaderList>(MediaType.ApplicationXml, new XmlSerializerCodec());

      cm.AddCodec<RegisteredClass>(MediaType.ApplicationXml, new XmlSerializerCodec());

      cm.AddCodec<string>(MediaType.TextPlain, new TextCodec());
      cm.AddCodec<string>(MediaType.TextHtml, new TextCodec());
      cm.AddCodec<string>(MediaType.TextXml, new TextCodec());
    }
  }
}

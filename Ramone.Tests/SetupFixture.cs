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

      // FIXME: not needed any more, can just be removed
      cm.AddCodec<string, TextCodec>(MediaType.TextPlain);
      cm.AddCodec<string, TextCodec>(MediaType.TextHtml);
      cm.AddCodec<string, TextCodec>(MediaType.TextXml);
    }
  }
}

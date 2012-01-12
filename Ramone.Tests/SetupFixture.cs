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

      cm.AddCodec<Dossier>(CMSConstants.CMSMediaType, new XmlSerializerCodec<Dossier>());
      cm.AddCodec<DossierDocumentList>(CMSConstants.CMSMediaType, new XmlSerializerCodec<DossierDocumentList>());
      cm.AddCodec<Document>(CMSConstants.CMSMediaType, new XmlSerializerCodec<Document>());
      cm.AddCodec<Party>(CMSConstants.CMSMediaType, new XmlSerializerCodec<Party>());
      
      cm.AddCodec<Cat>("text/plain", new CatAsTextCodec());
      cm.AddCodec<Cat>("text/html", new CatAsHtmlCodec());

      cm.AddCodec<Dog1>("application/vnd.dog+xml", new XmlSerializerCodec<Dog1>());
      cm.AddCodec<Dog2>("application/vnd.dog+xml", new XmlSerializerCodec<Dog2>());

      cm.AddCodec<HeaderList>("application/xml", new XmlSerializerCodec<HeaderList>());

      cm.AddCodec<MultipartData>("multipart/form-data", new MultipartFormDataSerializerCodec<MultipartData>());
      cm.AddCodec<Ramone.Tests.MediaTypes.MultipartFormData.MultipartFormDataTests.MultipartDataFile>("multipart/form-data", new MultipartFormDataSerializerCodec<Ramone.Tests.MediaTypes.MultipartFormData.MultipartFormDataTests.MultipartDataFile>());

      cm.AddCodec<string>("text/plain", new TextCodec());
      cm.AddCodec<string>("text/html", new TextCodec());
      cm.AddCodec<string>("text/xml", new TextCodec());
    }
  }
}

using System.Collections.Generic;
using System.ServiceModel.Syndication;
using OpenRasta.Codecs;
using OpenRasta.Codecs.WebForms;
using OpenRasta.Configuration;
using Ramone.Tests.Common;
using Ramone.Tests.Common.CMS;
using Ramone.Tests.Server.Codecs;
using Ramone.Tests.Server.Codecs.CMS;
using Ramone.Tests.Server.Handlers;
using Ramone.Tests.Server.Handlers.CMS;
using OpenRasta.IO;
using System.Xml;
using System.IO;


namespace Ramone.Tests.Server
{
  public class Configuration : IConfigurationSource
  {
    public class MyFileResource { }

    public class XmlEcho { public XmlDocument Doc { get; set; } }

    public class AnyEcho { public Stream S { get; set; } }


    public void Configure()
    {
      using (OpenRastaConfiguration.Manual)
      {
        ResourceSpace.Has.ResourcesOfType<SyndicationFeed>()
            .AtUri(Constants.AtomFeedPath)
            .HandledBy<FeedHandler>()
            .TranscodedBy<AtomFeedCodec>();

        ResourceSpace.Has.ResourcesOfType<SyndicationItem>()
            .AtUri(Constants.AtomItemPath)
            .HandledBy<FeedHandler>()
            .TranscodedBy<AtomItemCodec>();

        ResourceSpace.Has.ResourcesOfType<string>()
            .AtUri("/texts/plain")
            .HandledBy<TextHandler>()
            .TranscodedBy<TextCodec>()
            .ForMediaType("text/plain");

        ResourceSpace.Has.ResourcesOfType<Cat>()
            .AtUri(Constants.CatPath)
            .And.AtUri(Constants.CatsPath)
            .HandledBy<CatHandler>()
            .TranscodedBy<CatAsTextCodec>()
            .And.TranscodedBy<CatAsHtmlCodec>()
            .And.TranscodedBy<CatAsXmlCodec>()
            .And.TranscodedBy<CatAsJsonCodec>();

        ResourceSpace.Has.ResourcesOfType<Dog2>()
            .AtUri(Constants.DogPath)
            .HandledBy<DogHandler>()
            .TranscodedBy<Dog2AsXmlCodec>();

        ResourceSpace.Has.ResourcesOfType<Person>()
            .AtUri(Constants.PersonPath)
            .HandledBy<PersonHandler>()
            .RenderedByAspx("~/Views/Person.aspx");

        ResourceSpace.Has.ResourcesOfType<EncodingData>()
            .AtUri(Constants.EncodingPath)
            .HandledBy<EncodingHandler>()
            .TranscodedBy<EncodingCodec>();

        ResourceSpace.Has.ResourcesOfType<MyFileResource>()
            .AtUri(Constants.FilePath)
            .HandledBy<FileHandler>()
            .TranscodedBy<ApplicationOctetStreamCodec>();

        ResourceSpace.Has.ResourcesOfType<MultipartData>()
            .AtUri(Constants.MultipartFormDataPath).Named("SimpleData")
            .And.AtUri(Constants.MultipartFormDataFilePath).Named("FileData")
            .HandledBy<MultipartFormDataHandler>();

        ResourceSpace.Has.ResourcesOfType<FormUrlEncodedData>()
            .AtUri(Constants.FormUrlEncodedPath)
            .HandledBy<FormUrlEncodedHandler>()
            .TranscodedBy<FormUrlencodedCodec>();

        ResourceSpace.Has.ResourcesOfType<HeaderList>()
            .AtUri(Constants.HeaderEchoPath)
            .HandledBy<HeaderEchoHandler>()
            .TranscodedBy<HeaderEchoCodec>();

        ResourceSpace.Has.ResourcesOfType<XmlEcho>()
            .AtUri(Constants.XmlEchoPath)
            .HandledBy<XmlEchoHandler>()
            .TranscodedBy<XmlEchoCodec>();

        ResourceSpace.Has.ResourcesOfType<AnyEcho>()
            .AtUri(Constants.AnyEchoPath)
            .HandledBy<AnyEchoHandler>()
            .TranscodedBy<AnyEchoCodec>();

        ResourceSpace.Has.ResourcesOfType<ComplexClassForOpenRastaSerializationTests>()
            .AtUri(Constants.ComplexClassPath)
            .HandledBy<ComplexClassHandler>()
            .TranscodedBy<TextCodec>()
            .ForMediaType("application/x-www-form-urlencoded");

        ConfigureCMS();
      }
    }


    private void ConfigureCMS()
    {
      ResourceSpace.Has.ResourcesOfType<Dossier>()
          .AtUri(CMSConstants.DossierPath)
          .And.AtUri(CMSConstants.DossiersPath)
          .HandledBy<DossiersHandler>()
          .TranscodedBy<DossierCodec>();

      ResourceSpace.Has.ResourcesOfType<DossierDocumentList>()
          .AtUri(CMSConstants.DossierDocumentsPath)
          .HandledBy<DossierDocumentsHandler>()
          .TranscodedBy<DossierDocumentsCodec>()
          .ForMediaType(CMSConstants.CMSMediaType.MediaType);

      ResourceSpace.Has.ResourcesOfType<Document>()
          .AtUri(CMSConstants.DocumentPath)
          .HandledBy<DocumentHandler>()
          .TranscodedBy<DocumentCodec>()
          .ForMediaType(CMSConstants.CMSMediaType.MediaType);

      ResourceSpace.Has.ResourcesOfType<Party>()
          .AtUri(CMSConstants.PartyPath)
          .HandledBy<PartyHandler>()
          .TranscodedBy<PartyCodec>()
          .ForMediaType(CMSConstants.CMSMediaType.MediaType);
    }
  }
}

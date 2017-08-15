using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;
using OpenRasta.Codecs;
using OpenRasta.Configuration;
using Ramone.MediaTypes.JsonPatch;
using Ramone.Tests.Common;
using Ramone.Tests.Server.Blog;
using Ramone.Tests.Server.Blog.Data;
using Ramone.Tests.Server.CMS;
using Ramone.Tests.Server.Codecs;
using Ramone.Tests.Server.Handlers;
using Ramone.Tests.Server.OAuth2;


namespace Ramone.Tests.Server
{
  public class Configuration : IConfigurationSource
  {
    public class MyFileResource { }

    public class XmlEcho { public XmlDocument Doc { get; set; } }

    public class AnyEcho { public Stream S { get; set; } }

    public class FileDownload { public string Content { get; set; } }

    public class LinkHeader { }

    public class Patch { }


    public void Configure()
    {
      BlogDB.Reset();

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
            .And.TranscodedBy<CatAsJsonCodec>()
            .And.TranscodedBy<CatsAsJsonCodec>();

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
            .HandledBy<MultipartFormDataHandler>()
            .TranscodedBy<EncodingCodec>();

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

        ResourceSpace.Has.ResourcesOfType<TestForm>()
                     .AtUri(Constants.FormPath)
                     .And.AtUri(Constants.FormSimplePath)
                     .HandledBy<FormHandler>()
                     .RenderedByAspx("~/Views/Form.aspx")
                     .And.TranscodedBy<FormUrlencodedCodec>();

        ResourceSpace.Has.ResourcesOfType<FileDownload>()
            .AtUri(Constants.FileDownloadPath)
            .HandledBy<FileDownloadHandler>()
            .TranscodedBy<FileDownloadCodec>();

        ResourceSpace.Has.ResourcesOfType<LinkHeader>()
            .AtUri(Constants.LinkHeaderPath)
            .HandledBy<LinkHeaderHandler>()
            .TranscodedBy<LinkHeaderCodec>();

        ResourceSpace.Has.ResourcesOfType<JsonPatchDocument>()
            .AtUri(Constants.PatchPath)
            .HandledBy<PatchHandler>()
            .TranscodedBy<Ramone.Tests.Server.Codecs.JsonPatchDocumentCodec>();


        ResourceSpace.Has.ResourcesOfType<SlowResource>()
            .AtUri(Constants.SlowPath)
            .HandledBy<SlowHandler>()
            .TranscodedBy<JsonSerializerCodec<SlowResource>>();

        CMSConfiguration.Configure();
        ResourceSpace.Has.ResourcesOfType<RedirectArgs>()
            .AtUri(Constants.RedirectPath)
            .HandledBy<RedirectHandler>()
            .TranscodedBy<FormUrlencodedCodec>();

        ResourceSpace.Has.ResourcesOfType<HtmlPageResource>()
            .AtUri(Constants.HtmlPath)
            .HandledBy<HtmlHandler>()
            .RenderedByAspx("~/Views/Html.aspx");

        ResourceSpace.Has.ResourcesOfType<ApplicationError>()
            .AtUri(Constants.ApplicationErrorPath)
            .HandledBy<ApplicationErrorHandler>()
            .TranscodedBy<JsonSerializerCodec<ApplicationError>>();

        BlogConfiguration.Configure();
        OAuth2Configuration.Configure();
      }
    }
  }
}

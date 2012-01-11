using System.ServiceModel.Syndication;
using System.Xml;
using NUnit.Framework;
using HtmlAgilityPack;
using System.IO;


namespace Ramone.Tests
{
  [TestFixture]
  public class ConfigurationTests : TestHelper
  {
    [Test]
    public void CanAddStandardCodecs()
    {
      // Act
      IRamoneService service = RamoneConfiguration.NewService(BaseUrl)
                                                  .WithStandardCodecs();

      // Assert (a few of them)
      Assert.IsNotNull(service.CodecManager.GetReader(typeof(XmlDocument), "application/xml"));
      Assert.IsNotNull(service.CodecManager.GetReader(typeof(object), "application/json"));
      Assert.IsNotNull(service.CodecManager.GetReader(typeof(SyndicationFeed), "application/atom+xml"));

      // Streams can read/write any media-type
      Assert.IsNotNull(service.CodecManager.GetReader(typeof(Stream), "application/octet"));
      Assert.IsNotNull(service.CodecManager.GetReader(typeof(Stream), "image/jpeg"));
      Assert.IsNotNull(service.CodecManager.GetReader(typeof(Stream), "unknown/other"));

      // Writers
      Assert.IsNotNull(service.CodecManager.GetWriter(typeof(Stream), "application/octet"));
      Assert.IsNotNull(service.CodecManager.GetWriter(typeof(Stream), "image/jpeg"));
      Assert.IsNotNull(service.CodecManager.GetWriter(typeof(Stream), "unknown/other"));
    }
  }
}

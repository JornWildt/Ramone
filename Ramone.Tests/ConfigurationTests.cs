using System.ServiceModel.Syndication;
using System.Xml;
using NUnit.Framework;
using Ramone.MediaTypes.Hal;


namespace Ramone.Tests
{
  [TestFixture]
  public class ConfigurationTests : TestHelper
  {
    [Test]
    public void CanAddStandardCodecs()
    {
      // Act
      ISettings settings = RamoneConfiguration.NewSettings()
                                                  .WithStandardCodecs();

      // Assert
      Assert.IsNotNull(settings.CodecManager.GetReader(typeof(XmlDocument), "application/xml"));
      Assert.IsNotNull(settings.CodecManager.GetReader(typeof(SyndicationItem), "application/atom+xml"));
      Assert.IsNotNull(settings.CodecManager.GetReader(typeof(SyndicationFeed), "application/atom+xml"));
      Assert.IsNotNull(settings.CodecManager.GetReader(typeof(HalResource), "application/hal+xml"));
    }
  }
}

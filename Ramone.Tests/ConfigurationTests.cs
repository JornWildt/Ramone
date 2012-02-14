using System;
using System.Globalization;
using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;
using NUnit.Framework;


namespace Ramone.Tests
{
  [TestFixture]
  public class ConfigurationTests : TestHelper
  {
    [Test]
    public void HasExpectedDefaultSettings()
    {
      // Act
      IRamoneService service = RamoneConfiguration.NewService(BaseUrl);

      // Assert
      Assert.AreEqual("Ramone/1.0", service.UserAgent);
      Assert.AreEqual("{0}[{1}]", service.SerializerSettings.ArrayFormat);
      Assert.AreEqual("{0}[{1}]", service.SerializerSettings.DictionaryFormat);
      Assert.AreEqual("{0}.{1}", service.SerializerSettings.PropertyFormat);
      Assert.AreEqual("s", service.SerializerSettings.DateTimeFormat);
      Assert.AreEqual(CultureInfo.InvariantCulture.Name, service.SerializerSettings.Culture.Name);
    }


    [Test]
    public void HasStandardCodec()
    {
      // Act
      IRamoneService service = RamoneConfiguration.NewService(BaseUrl);

      // Assert (a few of them)
      Assert.IsNotNull(service.CodecManager.GetReader(typeof(XmlDocument), MediaType.ApplicationXml));
      Assert.IsNotNull(service.CodecManager.GetReader(typeof(object), MediaType.ApplicationJson));
      Assert.IsNotNull(service.CodecManager.GetReader(typeof(SyndicationFeed), MediaType.ApplicationAtom));

      // Streams can read/write any media-type
      Assert.IsNotNull(service.CodecManager.GetReader(typeof(Stream), MediaType.ApplicationOctetStream));
      Assert.IsNotNull(service.CodecManager.GetReader(typeof(Stream), new MediaType("image/jpeg")));
      Assert.IsNotNull(service.CodecManager.GetReader(typeof(Stream), new MediaType("unknown/other")));

      // Writers
      Assert.IsNotNull(service.CodecManager.GetWriter(typeof(Stream), MediaType.ApplicationOctetStream));
      Assert.IsNotNull(service.CodecManager.GetWriter(typeof(Stream), new MediaType("image/jpeg")));
      Assert.IsNotNull(service.CodecManager.GetWriter(typeof(Stream), new MediaType("unknown/other")));
    }


    [Test]
    public void CanAvoidAddingStandardCodecs()
    {
      // Act
      RamoneConfiguration.UseStandardCodecs = false;
      IRamoneService service = RamoneConfiguration.NewService(BaseUrl);

      // Assert (a few of them)
      AssertThrows<ArgumentException>(() => service.CodecManager.GetReader(typeof(XmlDocument), MediaType.ApplicationXml));
      AssertThrows<ArgumentException>(() => service.CodecManager.GetReader(typeof(Stream), MediaType.ApplicationOctetStream));
      AssertThrows<ArgumentException>(() => service.CodecManager.GetWriter(typeof(Stream), new MediaType("unknown/other")));
    }


    [Test]
    public void CanConfigureSerializerSettings()
    {
      // Act
      RamoneConfiguration.SerializerSettings.ArrayFormat = "A";
      RamoneConfiguration.SerializerSettings.DictionaryFormat = "B";
      RamoneConfiguration.SerializerSettings.PropertyFormat = "C";
      RamoneConfiguration.SerializerSettings.DateTimeFormat = "O";
      RamoneConfiguration.SerializerSettings.Culture = CultureInfo.GetCultureInfo("da-DK");

      IRamoneService service = RamoneConfiguration.NewService(BaseUrl);

      // Assert
      Assert.AreEqual("A", service.SerializerSettings.ArrayFormat);
      Assert.AreEqual("B", service.SerializerSettings.DictionaryFormat);
      Assert.AreEqual("C", service.SerializerSettings.PropertyFormat);
      Assert.AreEqual("O", service.SerializerSettings.DateTimeFormat);
      Assert.AreEqual("da-DK", service.SerializerSettings.Culture.Name);
    }


    [Test]
    public void CanConfigureSimpleProperties()
    {
      // Act
      RamoneConfiguration.UserAgent = "Tester";
      
      IRamoneService service = RamoneConfiguration.NewService(BaseUrl);

      // Assert
      Assert.AreEqual("Tester", service.UserAgent);
    }
  }
}

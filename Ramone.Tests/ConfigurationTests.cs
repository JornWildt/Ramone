using System;
using System.Globalization;
using System.IO;
using System.ServiceModel.Syndication;
using System.Text;
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
      IService service = RamoneConfiguration.NewService(BaseUrl);

      // Assert
      Assert.That(service.UserAgent, Is.EqualTo("Ramone/1.0"));
      Assert.That(service.DefaultEncoding, Is.EqualTo(Encoding.UTF8));
      Assert.That(service.SerializerSettings.ArrayFormat, Is.EqualTo("{0}[{1}]"));
      Assert.That(service.SerializerSettings.DictionaryFormat, Is.EqualTo("{0}[{1}]"));
      Assert.That(service.SerializerSettings.PropertyFormat, Is.EqualTo("{0}.{1}"));
      Assert.That(service.SerializerSettings.DateTimeFormat, Is.EqualTo("s"));
      Assert.That(service.SerializerSettings.Culture.Name, Is.EqualTo(CultureInfo.InvariantCulture.Name));
    }


    [Test]
    public void HasStandardCodec()
    {
      // Act
      IService service = RamoneConfiguration.NewService(BaseUrl);

      // Assert (a few of them)
      Assert.That(service.CodecManager.GetReader(typeof(XmlDocument), MediaType.ApplicationXml), Is.Not.Null);
      Assert.That(service.CodecManager.GetReader(typeof(object), MediaType.ApplicationJson), Is.Not.Null);
      Assert.That(service.CodecManager.GetReader(typeof(SyndicationFeed), MediaType.ApplicationAtom), Is.Not.Null);

      // Streams can read/write any media-type
      Assert.That(service.CodecManager.GetReader(typeof(Stream), MediaType.ApplicationOctetStream), Is.Not.Null);
      Assert.That(service.CodecManager.GetReader(typeof(Stream), new MediaType("image/jpeg")), Is.Not.Null);
      Assert.That(service.CodecManager.GetReader(typeof(Stream), new MediaType("unknown/other")), Is.Not.Null);

      // Writers
      Assert.That(service.CodecManager.GetWriter(typeof(Stream), MediaType.ApplicationOctetStream), Is.Not.Null);
      Assert.That(service.CodecManager.GetWriter(typeof(Stream), new MediaType("image/jpeg")), Is.Not.Null);
      Assert.That(service.CodecManager.GetWriter(typeof(Stream), new MediaType("unknown/other")), Is.Not.Null);
    }


    [Test]
    public void CanAvoidAddingStandardCodecs()
    {
      // Act
      RamoneConfiguration.UseStandardCodecs = false;
      IService service = RamoneConfiguration.NewService(BaseUrl);

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

      IService service = RamoneConfiguration.NewService(BaseUrl);

      // Assert
      Assert.That(service.SerializerSettings.ArrayFormat, Is.EqualTo("A"));
      Assert.That(service.SerializerSettings.DictionaryFormat, Is.EqualTo("B"));
      Assert.That(service.SerializerSettings.PropertyFormat, Is.EqualTo("C"));
      Assert.That(service.SerializerSettings.DateTimeFormat, Is.EqualTo("O"));
      Assert.That(service.SerializerSettings.Culture.Name, Is.EqualTo("da-DK"));
    }


    [Test]
    public void CanConfigureSimpleProperties()
    {
      // Act
      RamoneConfiguration.UserAgent = "Tester";
      RamoneConfiguration.DefaultEncoding = Encoding.ASCII;
      
      IService service = RamoneConfiguration.NewService(BaseUrl);

      // Assert
      Assert.That(service.UserAgent, Is.EqualTo("Tester"));
      Assert.That(service.DefaultEncoding, Is.EqualTo(Encoding.ASCII));
    }
  }
}

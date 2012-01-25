using System;
using NUnit.Framework;
using Ramone.Implementation;
using Ramone.MediaTypes.FormUrlEncoded;
using Ramone.MediaTypes.MultipartFormData;
using Ramone.Tests.Codecs;
using Ramone.Tests.Common;
using Ramone.MediaTypes.Xml;
using Ramone.MediaTypes.Json;


namespace Ramone.Tests
{
  [TestFixture]
  public class CodecManagerTests : TestHelper
  {
    ICodecManager CM;


    protected override void SetUp()
    {
      base.SetUp();
      CM = new CodecManager();
      RamoneConfiguration.RegisterStandardCodecs(CM);
    }


    [Test]
    public void WhenAddingSameCodecAndMediaTypeTwiceItThrows()
    {
      // Arrange

      // Act
      CM.AddCodec<Cat>("x", new CatAsHtmlCodec());
      CM.AddCodec<Cat>("y", new CatAsHtmlCodec());

      // Assert
      AssertThrows<ArgumentException>(() => CM.AddCodec<Cat>("x", new CatAsHtmlCodec()));
    }


    [Test]
    public void CanRegisterFormUrlEncodedCodecsWithShorthand()
    {
      // Act
      CM.AddFormUrlEncoded<MyData>();
      MediaTypeWriterRegistration codecReg1 = CM.GetWriter(typeof(MyData));
      MediaTypeWriterRegistration codecReg2 = CM.GetWriter(typeof(MyData), "application/x-www-form-urlencoded");

      // Assert
      Assert.IsNotNull(codecReg1);
      Assert.IsNotNull(codecReg2);
      Assert.AreEqual("application/x-www-form-urlencoded", codecReg1.MediaType);
      Assert.AreEqual("application/x-www-form-urlencoded", codecReg2.MediaType);
      Assert.AreEqual(typeof(FormUrlEncodedSerializerCodec), codecReg1.Codec.GetType());
      Assert.AreEqual(typeof(FormUrlEncodedSerializerCodec), codecReg2.Codec.GetType());
    }


    [Test]
    public void CanRegisterFormUrlEncodedCodecWithShorthandAndMediaType()
    {
      // Act
      CM.AddFormUrlEncoded<MyData>("application/ramone");
      MediaTypeWriterRegistration codecReg = CM.GetWriter(typeof(MyData), "application/ramone");

      // Assert
      Assert.IsNotNull(codecReg);
      Assert.AreEqual("application/ramone", codecReg.MediaType);
      Assert.AreEqual(typeof(FormUrlEncodedSerializerCodec), codecReg.Codec.GetType());
    }


    [Test]
    public void CanRegisterMultipartFormDataCodecsWithShorthand()
    {
      // Act
      CM.AddMultipartFormData<MyData>();
      MediaTypeWriterRegistration codecReg1 = CM.GetWriter(typeof(MyData));
      MediaTypeWriterRegistration codecReg2 = CM.GetWriter(typeof(MyData), "multipart/form-data");

      // Assert
      Assert.IsNotNull(codecReg1);
      Assert.IsNotNull(codecReg2);
      Assert.AreEqual("multipart/form-data", codecReg1.MediaType);
      Assert.AreEqual("multipart/form-data", codecReg2.MediaType);
      Assert.AreEqual(typeof(MultipartFormDataSerializerCodec), codecReg1.Codec.GetType());
      Assert.AreEqual(typeof(MultipartFormDataSerializerCodec), codecReg2.Codec.GetType());
    }


    [Test]
    public void CanRegisterMultipartFormDataCodecWithShorthandAndMediaType()
    {
      // Act
      CM.AddMultipartFormData<MyData>("application/ramone");
      MediaTypeWriterRegistration codecReg = CM.GetWriter(typeof(MyData), "application/ramone");

      // Assert
      Assert.IsNotNull(codecReg);
      Assert.AreEqual("application/ramone", codecReg.MediaType);
      Assert.AreEqual(typeof(MultipartFormDataSerializerCodec), codecReg.Codec.GetType());
    }


    [Test]
    public void CanRegisterXmlSerializerCodecWithShorthand()
    {
      // Act
      CM.AddXml<MyData>();
      MediaTypeWriterRegistration codecReg1 = CM.GetWriter(typeof(MyData));
      MediaTypeWriterRegistration codecReg2 = CM.GetWriter(typeof(MyData), "application/xml");

      // Assert
      Assert.IsNotNull(codecReg1);
      Assert.IsNotNull(codecReg2);
      Assert.AreEqual("application/xml", codecReg1.MediaType);
      Assert.AreEqual("application/xml", codecReg2.MediaType);
      Assert.AreEqual(typeof(XmlSerializerCodec), codecReg1.Codec.GetType());
      Assert.AreEqual(typeof(XmlSerializerCodec), codecReg2.Codec.GetType());
    }


    [Test]
    public void CanRegisterXmlSerializerCodecWithShorthandAndMediaType()
    {
      // Act
      CM.AddXml<MyData>("application/ramone+xml");
      MediaTypeWriterRegistration codecReg = CM.GetWriter(typeof(MyData), "application/ramone+xml");

      // Assert
      Assert.IsNotNull(codecReg);
      Assert.AreEqual("application/ramone+xml", codecReg.MediaType);
      Assert.AreEqual(typeof(XmlSerializerCodec), codecReg.Codec.GetType());
    }


    [Test]
    public void CanRegisterJsonSerializerCodecWithShorthand()
    {
      // Act
      CM.AddJson<MyData>();
      MediaTypeWriterRegistration codecReg1 = CM.GetWriter(typeof(MyData));
      MediaTypeWriterRegistration codecReg2 = CM.GetWriter(typeof(MyData), "application/json");

      // Assert
      Assert.IsNotNull(codecReg1);
      Assert.IsNotNull(codecReg2);
      Assert.AreEqual("application/json", codecReg1.MediaType);
      Assert.AreEqual("application/json", codecReg2.MediaType);
      Assert.AreEqual(typeof(JsonSerializerCodec), codecReg1.Codec.GetType());
      Assert.AreEqual(typeof(JsonSerializerCodec), codecReg2.Codec.GetType());
    }


    [Test]
    public void CanRegisterJsonSerializerCodecWithShorthandAndMediaType()
    {
      // Act
      CM.AddJson<MyData>("application/ramone+json");
      MediaTypeWriterRegistration codecReg = CM.GetWriter(typeof(MyData), "application/ramone+json");

      // Assert
      Assert.IsNotNull(codecReg);
      Assert.AreEqual("application/ramone+json", codecReg.MediaType);
      Assert.AreEqual(typeof(JsonSerializerCodec), codecReg.Codec.GetType());
    }


    [Test]
    public void MediaTypeQueryingIsCaseInsensitive()
    {
      // Act
      CM.AddJson<MyData>("application/RAMONE+json");
      MediaTypeWriterRegistration codecReg = CM.GetWriter(typeof(MyData), "appLICAtion/ramone+json");

      // Assert
      Assert.IsNotNull(codecReg);
      Assert.AreEqual("application/ramone+json", codecReg.MediaType, "Media-type identifiers are stored in lower case.");
      Assert.AreEqual(typeof(JsonSerializerCodec), codecReg.Codec.GetType());
    }


    public class MyData
    {
    }
  }
}

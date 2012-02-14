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
      CM.AddCodec<Cat>(MediaType.ApplicationJson, new CatAsHtmlCodec());
      CM.AddCodec<Cat>(MediaType.ApplicationXml, new CatAsHtmlCodec());

      // Assert
      AssertThrows<ArgumentException>(() => CM.AddCodec<Cat>(MediaType.ApplicationJson, new CatAsHtmlCodec()));
    }


    [Test]
    public void CanRegisterFormUrlEncodedCodecsWithShorthand()
    {
      // Act
      CM.AddFormUrlEncoded<MyData>();
      MediaTypeWriterRegistration codecReg1 = CM.GetWriter(typeof(MyData), MediaType.Wildcard);
      MediaTypeWriterRegistration codecReg2 = CM.GetWriter(typeof(MyData), MediaType.ApplicationFormUrlEncoded);

      // Assert
      Assert.IsNotNull(codecReg1);
      Assert.IsNotNull(codecReg2);
      Assert.AreEqual(MediaType.ApplicationFormUrlEncoded, codecReg1.MediaType);
      Assert.AreEqual(MediaType.ApplicationFormUrlEncoded, codecReg2.MediaType);
      Assert.AreEqual(typeof(FormUrlEncodedSerializerCodec), codecReg1.Codec.GetType());
      Assert.AreEqual(typeof(FormUrlEncodedSerializerCodec), codecReg2.Codec.GetType());
    }


    [Test]
    public void CanRegisterFormUrlEncodedCodecWithShorthandAndMediaType()
    {
      // Act
      CM.AddFormUrlEncoded<MyData>(new MediaType("application/ramone"));
      MediaTypeWriterRegistration codecReg = CM.GetWriter(typeof(MyData), new MediaType("application/ramone"));

      // Assert
      Assert.IsNotNull(codecReg);
      Assert.AreEqual(new MediaType("application/ramone"), codecReg.MediaType);
      Assert.AreEqual(typeof(FormUrlEncodedSerializerCodec), codecReg.Codec.GetType());
    }


    [Test]
    public void CanRegisterMultipartFormDataCodecsWithShorthand()
    {
      // Act
      CM.AddMultipartFormData<MyData>();
      MediaTypeWriterRegistration codecReg1 = CM.GetWriter(typeof(MyData), MediaType.Wildcard);
      MediaTypeWriterRegistration codecReg2 = CM.GetWriter(typeof(MyData), MediaType.MultipartFormData);

      // Assert
      Assert.IsNotNull(codecReg1);
      Assert.IsNotNull(codecReg2);
      Assert.AreEqual(MediaType.MultipartFormData, codecReg1.MediaType);
      Assert.AreEqual(MediaType.MultipartFormData, codecReg2.MediaType);
      Assert.AreEqual(typeof(MultipartFormDataSerializerCodec), codecReg1.Codec.GetType());
      Assert.AreEqual(typeof(MultipartFormDataSerializerCodec), codecReg2.Codec.GetType());
    }


    [Test]
    public void CanRegisterMultipartFormDataCodecWithShorthandAndMediaType()
    {
      // Act
      CM.AddMultipartFormData<MyData>(new MediaType("application/ramone"));
      MediaTypeWriterRegistration codecReg = CM.GetWriter(typeof(MyData), new MediaType("application/ramone"));

      // Assert
      Assert.IsNotNull(codecReg);
      Assert.AreEqual(new MediaType("application/ramone"), codecReg.MediaType);
      Assert.AreEqual(typeof(MultipartFormDataSerializerCodec), codecReg.Codec.GetType());
    }


    [Test]
    public void CanRegisterXmlSerializerCodecWithShorthand()
    {
      // Act
      CM.AddXml<MyData>();
      MediaTypeWriterRegistration codecReg1 = CM.GetWriter(typeof(MyData), MediaType.Wildcard);
      MediaTypeWriterRegistration codecReg2 = CM.GetWriter(typeof(MyData), MediaType.ApplicationXml);

      // Assert
      Assert.IsNotNull(codecReg1);
      Assert.IsNotNull(codecReg2);
      Assert.AreEqual(MediaType.ApplicationXml, codecReg1.MediaType);
      Assert.AreEqual(MediaType.ApplicationXml, codecReg2.MediaType);
      Assert.AreEqual(typeof(XmlSerializerCodec), codecReg1.Codec.GetType());
      Assert.AreEqual(typeof(XmlSerializerCodec), codecReg2.Codec.GetType());
    }


    [Test]
    public void CanRegisterXmlSerializerCodecWithShorthandAndMediaType()
    {
      // Act
      CM.AddXml<MyData>(new MediaType("application/ramone+xml"));
      MediaTypeWriterRegistration codecReg = CM.GetWriter(typeof(MyData), new MediaType("application/ramone+xml"));

      // Assert
      Assert.IsNotNull(codecReg);
      Assert.AreEqual(new MediaType("application/ramone+xml"), codecReg.MediaType);
      Assert.AreEqual(typeof(XmlSerializerCodec), codecReg.Codec.GetType());
    }


    [Test]
    public void CanRegisterJsonSerializerCodecWithShorthand()
    {
      // Act
      CM.AddJson<MyData>();
      MediaTypeWriterRegistration codecReg1 = CM.GetWriter(typeof(MyData), MediaType.Wildcard);
      MediaTypeWriterRegistration codecReg2 = CM.GetWriter(typeof(MyData), MediaType.ApplicationJson);

      // Assert
      Assert.IsNotNull(codecReg1);
      Assert.IsNotNull(codecReg2);
      Assert.AreEqual(MediaType.ApplicationJson, codecReg1.MediaType);
      Assert.AreEqual(MediaType.ApplicationJson, codecReg2.MediaType);
      Assert.AreEqual(typeof(JsonSerializerCodec), codecReg1.Codec.GetType());
      Assert.AreEqual(typeof(JsonSerializerCodec), codecReg2.Codec.GetType());
    }


    [Test]
    public void CanRegisterJsonSerializerCodecWithShorthandAndMediaType()
    {
      // Act
      CM.AddJson<MyData>(new MediaType("application/ramone+json"));
      MediaTypeWriterRegistration codecReg = CM.GetWriter(typeof(MyData), new MediaType("application/ramone+json"));

      // Assert
      Assert.IsNotNull(codecReg);
      Assert.AreEqual(new MediaType("application/ramone+json"), codecReg.MediaType);
      Assert.AreEqual(typeof(JsonSerializerCodec), codecReg.Codec.GetType());
    }


    [Test]
    public void MediaTypeQueryingIsCaseInsensitive()
    {
      // Act
      CM.AddJson<MyData>(new MediaType("application/RAMONE+json"));
      MediaTypeWriterRegistration codecReg = CM.GetWriter(typeof(MyData), new MediaType("appLICAtion/ramone+json"));

      // Assert
      Assert.IsNotNull(codecReg);
      Assert.AreEqual(new MediaType("application/ramone+json"), codecReg.MediaType, "Media-type identifiers are stored in lower case.");
      Assert.AreEqual(typeof(JsonSerializerCodec), codecReg.Codec.GetType());
    }


    [Test]
    public void CanHandleWriterMediaTypeWildcards()
    {
      // Act
      CM.AddJson<MyData>(new MediaType("*/*"));
      MediaTypeWriterRegistration codecReg1 = CM.GetWriter(typeof(MyData), new MediaType("application/json"));
      MediaTypeWriterRegistration codecReg2 = CM.GetWriter(typeof(MyData), new MediaType("something/else"));

      // Assert
      Assert.IsNotNull(codecReg1);
      Assert.IsNotNull(codecReg2);
      Assert.AreEqual(new MediaType("*/*"), codecReg1.MediaType);
      Assert.AreEqual(new MediaType("*/*"), codecReg2.MediaType);
      Assert.AreEqual(typeof(JsonSerializerCodec), codecReg1.Codec.GetType());
      Assert.AreEqual(typeof(JsonSerializerCodec), codecReg2.Codec.GetType());
    }


    [Test]
    public void CanHandleReaderMediaTypeWildcards()
    {
      // Act
      CM.AddJson<MyData>(new MediaType("*/*"));
      MediaTypeReaderRegistration codecReg1 = CM.GetReader(typeof(MyData), new MediaType("application/json"));
      MediaTypeReaderRegistration codecReg2 = CM.GetReader(typeof(MyData), new MediaType("something/else"));

      // Assert
      Assert.IsNotNull(codecReg1);
      Assert.IsNotNull(codecReg2);
      Assert.AreEqual(new MediaType("*/*"), codecReg1.MediaType);
      Assert.AreEqual(new MediaType("*/*"), codecReg2.MediaType);
      Assert.AreEqual(typeof(JsonSerializerCodec), codecReg1.Codec.GetType());
      Assert.AreEqual(typeof(JsonSerializerCodec), codecReg2.Codec.GetType());
    }


    [Test]
    public void CanHandleWriterMediaSubTypeWildcards()
    {
      // Act
      CM.AddJson<MyData>(new MediaType("application/*"));
      MediaTypeWriterRegistration codecReg1 = CM.GetWriter(typeof(MyData), new MediaType("application/json"));
      MediaTypeWriterRegistration codecReg2 = CM.GetWriter(typeof(MyData), new MediaType("application/other"));

      // Assert
      Assert.IsNotNull(codecReg1);
      Assert.IsNotNull(codecReg2);
      Assert.AreEqual(new MediaType("application/*"), codecReg1.MediaType);
      Assert.AreEqual(new MediaType("application/*"), codecReg2.MediaType);
      Assert.AreEqual(typeof(JsonSerializerCodec), codecReg1.Codec.GetType());
      Assert.AreEqual(typeof(JsonSerializerCodec), codecReg2.Codec.GetType());
    }


    [Test]
    public void CanHandleReaderMediaSubTypeWildcards()
    {
      // Act
      CM.AddJson<MyData>(new MediaType("application/*"));
      MediaTypeReaderRegistration codecReg1 = CM.GetReader(typeof(MyData), new MediaType("application/json"));
      MediaTypeReaderRegistration codecReg2 = CM.GetReader(typeof(MyData), new MediaType("application/other"));

      // Assert
      Assert.IsNotNull(codecReg1);
      Assert.IsNotNull(codecReg2);
      Assert.AreEqual(new MediaType("application/*"), codecReg1.MediaType);
      Assert.AreEqual(new MediaType("application/*"), codecReg2.MediaType);
      Assert.AreEqual(typeof(JsonSerializerCodec), codecReg1.Codec.GetType());
      Assert.AreEqual(typeof(JsonSerializerCodec), codecReg2.Codec.GetType());
    }


    public class MyData
    {
    }
  }
}

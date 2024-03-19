using System;
using NUnit.Framework;
using Ramone.Implementation;
using Ramone.MediaTypes.FormUrlEncoded;
using Ramone.MediaTypes.Json;
using Ramone.MediaTypes.MultipartFormData;
using Ramone.MediaTypes.Xml;
using Ramone.Tests.Common;
using Ramone.Tests.Common.Codecs;

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
      CM.AddCodec<Cat, CatAsHtmlCodec>(MediaType.ApplicationJson);
      CM.AddCodec<Cat>(MediaType.ApplicationXml, typeof(CatAsHtmlCodec));

      // Assert
      AssertThrows<ArgumentException>(() => CM.AddCodec<Cat, CatAsHtmlCodec>(MediaType.ApplicationJson));
      AssertThrows<ArgumentException>(() => CM.AddCodec<Cat>(MediaType.ApplicationJson, typeof(CatAsHtmlCodec)));
    }


    [Test]
    public void CanRegisterFormUrlEncodedCodecsWithShorthand()
    {
      // Act
      CM.AddFormUrlEncoded<MyData>();
      MediaTypeWriterRegistration codecReg1 = CM.GetWriter(typeof(MyData), MediaType.Wildcard);
      MediaTypeWriterRegistration codecReg2 = CM.GetWriter(typeof(MyData), MediaType.ApplicationFormUrlEncoded);

      // Assert
      Assert.That(codecReg1, Is.Not.Null);
      Assert.That(codecReg2, Is.Not.Null);
      Assert.That(codecReg1.MediaType, Is.EqualTo(MediaType.ApplicationFormUrlEncoded));
      Assert.That(codecReg2.MediaType, Is.EqualTo(MediaType.ApplicationFormUrlEncoded));
      Assert.That(codecReg1.Codec.GetType(), Is.EqualTo(typeof(FormUrlEncodedSerializerCodec)));
      Assert.That(codecReg2.Codec.GetType(), Is.EqualTo(typeof(FormUrlEncodedSerializerCodec)));
    }


    [Test]
    public void CanRegisterFormUrlEncodedCodecWithShorthandAndMediaType()
    {
      // Act
      CM.AddFormUrlEncoded<MyData>(new MediaType("application/ramone"));
      MediaTypeWriterRegistration codecReg = CM.GetWriter(typeof(MyData), new MediaType("application/ramone"));

      // Assert
      Assert.That(codecReg, Is.Not.Null);
      Assert.That(codecReg.MediaType, Is.EqualTo(new MediaType("application/ramone")));
      Assert.That(codecReg.Codec.GetType(), Is.EqualTo(typeof(FormUrlEncodedSerializerCodec)));
    }


    [Test]
    public void CanRegisterMultipartFormDataCodecsWithShorthand()
    {
      // Act
      CM.AddMultipartFormData<MyData>();
      MediaTypeWriterRegistration codecReg1 = CM.GetWriter(typeof(MyData), MediaType.Wildcard);
      MediaTypeWriterRegistration codecReg2 = CM.GetWriter(typeof(MyData), MediaType.MultipartFormData);

      // Assert
      Assert.That(codecReg1, Is.Not.Null);
      Assert.That(codecReg2, Is.Not.Null);
      Assert.That(codecReg1.MediaType, Is.EqualTo(MediaType.MultipartFormData));
      Assert.That(codecReg2.MediaType, Is.EqualTo(MediaType.MultipartFormData));
      Assert.That(codecReg1.Codec.GetType(), Is.EqualTo(typeof(MultipartFormDataSerializerCodec)));
      Assert.That(codecReg2.Codec.GetType(), Is.EqualTo(typeof(MultipartFormDataSerializerCodec)));
    }


    [Test]
    public void CanRegisterMultipartFormDataCodecWithShorthandAndMediaType()
    {
      // Act
      CM.AddMultipartFormData<MyData>(new MediaType("application/ramone"));
      MediaTypeWriterRegistration codecReg = CM.GetWriter(typeof(MyData), new MediaType("application/ramone"));

      // Assert
      Assert.That(codecReg, Is.Not.Null);
      Assert.That(codecReg.MediaType, Is.EqualTo(new MediaType("application/ramone")));
      Assert.That(codecReg.Codec.GetType(), Is.EqualTo(typeof(MultipartFormDataSerializerCodec)));
    }


    [Test]
    public void CanRegisterXmlSerializerCodecWithShorthand()
    {
      // Act
      CM.AddXml<MyData>();
      MediaTypeWriterRegistration codecReg1 = CM.GetWriter(typeof(MyData), MediaType.Wildcard);
      MediaTypeWriterRegistration codecReg2 = CM.GetWriter(typeof(MyData), MediaType.ApplicationXml);

      // Assert
      Assert.That(codecReg1, Is.Not.Null);
      Assert.That(codecReg2, Is.Not.Null);
      Assert.That(codecReg1.MediaType, Is.EqualTo(MediaType.ApplicationXml));
      Assert.That(codecReg2.MediaType, Is.EqualTo(MediaType.ApplicationXml));
      Assert.That(codecReg1.Codec.GetType(), Is.EqualTo(typeof(XmlSerializerCodec)));
      Assert.That(codecReg2.Codec.GetType(), Is.EqualTo(typeof(XmlSerializerCodec)));
    }


    [Test]
    public void CanRegisterXmlSerializerCodecWithShorthandAndMediaType()
    {
      // Act
      CM.AddXml<MyData>(new MediaType("application/ramone+xml"));
      MediaTypeWriterRegistration codecReg = CM.GetWriter(typeof(MyData), new MediaType("application/ramone+xml"));

      // Assert
      Assert.That(codecReg, Is.Not.Null);
      Assert.That(codecReg.MediaType, Is.EqualTo(new MediaType("application/ramone+xml")));
      Assert.That(codecReg.Codec.GetType(), Is.EqualTo(typeof(XmlSerializerCodec)));
    }


    [Test]
    public void CanRegisterJsonSerializerCodecWithShorthand()
    {
      // Act
      CM.AddJson<MyData>();
      MediaTypeWriterRegistration codecReg1 = CM.GetWriter(typeof(MyData), MediaType.Wildcard);
      MediaTypeWriterRegistration codecReg2 = CM.GetWriter(typeof(MyData), MediaType.ApplicationJson);

      // Assert
      Assert.That(codecReg1, Is.Not.Null);
      Assert.That(codecReg2, Is.Not.Null);
      Assert.That(codecReg1.MediaType, Is.EqualTo(MediaType.ApplicationJson));
      Assert.That(codecReg2.MediaType, Is.EqualTo(MediaType.ApplicationJson));
      Assert.That(codecReg1.Codec.GetType(), Is.EqualTo(typeof(JsonSerializerCodec)));
      Assert.That(codecReg2.Codec.GetType(), Is.EqualTo(typeof(JsonSerializerCodec)));
    }


    [Test]
    public void CanRegisterJsonSerializerCodecWithShorthandAndMediaType()
    {
      // Act
      CM.AddJson<MyData>(new MediaType("application/ramone+json"));
      MediaTypeWriterRegistration codecReg = CM.GetWriter(typeof(MyData), new MediaType("application/ramone+json"));

      // Assert
      Assert.That(codecReg, Is.Not.Null);
      Assert.That(codecReg.MediaType, Is.EqualTo(new MediaType("application/ramone+json")));
      Assert.That(codecReg.Codec.GetType(), Is.EqualTo(typeof(JsonSerializerCodec)));
    }


    [Test]
    public void MediaTypeQueryingIsCaseInsensitive()
    {
      // Act
      CM.AddJson<MyData>(new MediaType("application/RAMONE+json"));
      MediaTypeWriterRegistration codecReg = CM.GetWriter(typeof(MyData), new MediaType("appLICAtion/ramone+json"));

      // Assert
      Assert.That(codecReg, Is.Not.Null);
      Assert.That(codecReg.MediaType, Is.EqualTo(new MediaType("application/ramone+json")));
      Assert.That(codecReg.Codec.GetType(), Is.EqualTo(typeof(JsonSerializerCodec)));
    }


    [Test]
    public void CanHandleWriterMediaTypeWildcards()
    {
      // Act
      CM.AddJson<MyData>(new MediaType("*/*"));
      MediaTypeWriterRegistration codecReg1 = CM.GetWriter(typeof(MyData), new MediaType("application/json"));
      MediaTypeWriterRegistration codecReg2 = CM.GetWriter(typeof(MyData), new MediaType("something/else"));

      // Assert
      Assert.That(codecReg1, Is.Not.Null);
      Assert.That(codecReg2, Is.Not.Null);
      Assert.That(codecReg1.MediaType, Is.EqualTo(new MediaType("*/*")));
      Assert.That(codecReg2.MediaType, Is.EqualTo(new MediaType("*/*")));
      Assert.That(codecReg1.Codec.GetType(), Is.EqualTo(typeof(JsonSerializerCodec)));
      Assert.That(codecReg2.Codec.GetType(), Is.EqualTo(typeof(JsonSerializerCodec)));
    }


    [Test]
    public void CanHandleReaderMediaTypeWildcards()
    {
      // Act
      CM.AddJson<MyData>(new MediaType("*/*"));
      MediaTypeReaderRegistration codecReg1 = CM.GetReader(typeof(MyData), new MediaType("application/json"));
      MediaTypeReaderRegistration codecReg2 = CM.GetReader(typeof(MyData), new MediaType("something/else"));

      // Assert
      Assert.That(codecReg1, Is.Not.Null);
      Assert.That(codecReg2, Is.Not.Null);
      Assert.That(codecReg1.MediaType, Is.EqualTo(new MediaType("*/*")));
      Assert.That(codecReg2.MediaType, Is.EqualTo(new MediaType("*/*")));
      Assert.That(codecReg1.Codec.GetType(), Is.EqualTo(typeof(JsonSerializerCodec)));
      Assert.That(codecReg2.Codec.GetType(), Is.EqualTo(typeof(JsonSerializerCodec)));
    }


    [Test]
    public void CanHandleWriterMediaSubTypeWildcards()
    {
      // Act
      CM.AddJson<MyData>(new MediaType("application/*"));
      MediaTypeWriterRegistration codecReg1 = CM.GetWriter(typeof(MyData), new MediaType("application/json"));
      MediaTypeWriterRegistration codecReg2 = CM.GetWriter(typeof(MyData), new MediaType("application/other"));

      // Assert
      Assert.That(codecReg1, Is.Not.Null);
      Assert.That(codecReg2, Is.Not.Null);
      Assert.That(codecReg1.MediaType, Is.EqualTo(new MediaType("application/*")));
      Assert.That(codecReg2.MediaType, Is.EqualTo(new MediaType("application/*")));
      Assert.That(codecReg1.Codec.GetType(), Is.EqualTo(typeof(JsonSerializerCodec)));
      Assert.That(codecReg2.Codec.GetType(), Is.EqualTo(typeof(JsonSerializerCodec)));
    }


    [Test]
    public void CanHandleReaderMediaSubTypeWildcards()
    {
      // Act
      CM.AddJson<MyData>(new MediaType("application/*"));
      MediaTypeReaderRegistration codecReg1 = CM.GetReader(typeof(MyData), new MediaType("application/json"));
      MediaTypeReaderRegistration codecReg2 = CM.GetReader(typeof(MyData), new MediaType("application/other"));

      // Assert
      Assert.That(codecReg1, Is.Not.Null);
      Assert.That(codecReg2, Is.Not.Null);
      Assert.That(codecReg1.MediaType, Is.EqualTo(new MediaType("application/*")));
      Assert.That(codecReg2.MediaType, Is.EqualTo(new MediaType("application/*")));
      Assert.That(codecReg1.Codec.GetType(), Is.EqualTo(typeof(JsonSerializerCodec)));
      Assert.That(codecReg2.Codec.GetType(), Is.EqualTo(typeof(JsonSerializerCodec)));
    }


    public class MyData
    {
    }
  }
}

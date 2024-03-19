using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ramone.IO;
using Ramone.Tests.Common;
using Ramone.Utility.ObjectSerialization;
using System.Collections;


namespace Ramone.Tests.MediaTypes.MultipartFormData
{
  [TestFixture]
  public class MultipartFormDataTests : TestHelper
  {
    [Test]
    public void CanPostSimpleMultipartFormData()
    {
      // Arrange
      MultipartData data = new MultipartData { Name = "Pete", Age = 10, Active = true };
      Request formdataReq = Session.Bind(MultipartFormDataTemplate);

      // Act
      using (Response<string> response = formdataReq.Accept("text/plain").ContentType("multipart/form-data").Post<string>(data))
      {
        // Assert
        Assert.That(response.Headers["x-contenttype"].StartsWith("multipart/form-data"), Is.True);
        Assert.That(response.Body, Is.EqualTo("Pete-10-True"));
      }
    }


    [Test]
    public void CanPostSimpleMultipartFormDataUsingShorthand()
    {
      // Arrange
      MultipartData data = new MultipartData { Name = "Pete", Age = 10, Active = false };
      Request formdataReq = Session.Bind(MultipartFormDataTemplate);

      // Act
      using (Response<string> response = formdataReq.Accept("text/plain").AsMultipartFormData().Post<string>(data))
      {
        // Assert
        Assert.That(response.Headers["x-contenttype"].StartsWith("multipart/form-data"), Is.True);
        Assert.That(response.Body, Is.EqualTo("Pete-10-False"));
      }
    }


    public class MultipartDataFile
    {
      public IFile DataFile { get; set; }
      public int Age { get; set; }
    }


    [Test]
    public void CanPostMultipartFormDataWithFileUsingHashtable()
    {
      // Arrange
      IFile file = new File("..\\..\\data1.txt");
      Hashtable data = new Hashtable();
      data["DataFile"] = file;
      data["Age"] = 10;
      Request formdataReq = Session.Bind(MultipartFormDataFileTemplate);

      // Act
      using (Response<string> response = formdataReq.Accept("text/plain").ContentType("multipart/form-data").Post<string>(data))
      {
        // Assert
        Assert.That(response.Headers["x-contenttype"].StartsWith("multipart/form-data"), Is.True);
        Assert.That(response.Body, Is.EqualTo("data1.txt-text/plain-w4bDvMOuwrRgJw==-10"));
      }
    }


    [Test]
    public void CanPostMultipartFormDataWithFile()
    {
      // Arrange
      IFile file = new File("..\\..\\data1.txt");
      MultipartDataFile data = new MultipartDataFile { DataFile = file, Age = 10 };
      Request formdataReq = Session.Bind(MultipartFormDataFileTemplate);

      // Act
      using (Response<string> response = formdataReq.Accept("text/plain")
                                                    .ContentType("multipart/form-data")
                                                    .Post<string>(data))
      {
        // Assert
        Assert.That(response.Headers["x-contenttype"].StartsWith("multipart/form-data"), Is.True);
        Assert.That(response.Body, Is.EqualTo("data1.txt-text/plain-w4bDvMOuwrRgJw==-10"));
      }
    }


    [Test]
    public void CanPostMultipartFormDataWithSpecialFilenameUsingHashtable_ItMustReplaceSpecialCharsWithX()
    {
      // Arrange
      IFile file = new FileWithSpecialName("..\\..\\data1.txt", "Bøllefrø.txt");
      Hashtable data = new Hashtable();
      data["DataFile"] = file;
      data["Age"] = 10;
      Request formdataReq = Session.Bind(MultipartFormDataFileTemplate);

      // Act
      using (Response<string> response = formdataReq.Accept("text/plain").ContentType("multipart/form-data").Post<string>(data))
      {
        // Assert
        Assert.That(response.Headers["x-contenttype"].StartsWith("multipart/form-data"), Is.True);
        Assert.That(response.Body, Is.EqualTo("Bxllefrx.txt-text/plain-w4bDvMOuwrRgJw==-10"));
      }
    }


    [Test]
    public void CanPostMultipartFormDataWithSpecialFilename_ItMustReplaceSpecialCharsWithX()
    {
      // Arrange
      IFile file = new FileWithSpecialName("..\\..\\data1.txt", "Bøllefrø.txt");
      MultipartDataFile data = new MultipartDataFile { DataFile = file, Age = 10 };
      Request formdataReq = Session.Bind(MultipartFormDataFileTemplate);

      // Act
      using (Response<string> response = formdataReq.Accept("text/plain").ContentType("multipart/form-data").Post<string>(data))
      {
        // Assert
        Assert.That(response.Headers["x-contenttype"].StartsWith("multipart/form-data"), Is.True);
        Assert.That(response.Body, Is.EqualTo("Bxllefrx.txt-text/plain-w4bDvMOuwrRgJw==-10"));
      }
    }


    [Test]
    [Ignore("OpenRasta does not handle quoted strings for content-disposition filename")]
    public void CanPostMultipartFormDataWithFilenameContainingQuotes()
    {
      // Arrange
      IFile file = new FileWithSpecialName("..\\..\\data1.txt", "B\"all\"e.txt");
      MultipartDataFile data = new MultipartDataFile { DataFile = file, Age = 10 };
      Request formdataReq = Session.Bind(MultipartFormDataFileTemplate);

      // Act
      using (Response<string> response = formdataReq.Accept("text/plain").ContentType("multipart/form-data").Post<string>(data))
      {
        // Assert
        Assert.That(response.Headers["x-contenttype"].StartsWith("multipart/form-data"), Is.True);

        // This is the correct result, but not what OpenRasta returns currently.
        //Assert.AreEqual("B\"all\"e.txt/plain-w4bDvMOuwrRgJw==-10", response.Body);
      }
    }


    [Test]
    public void CanPostMultipartFormDataWithBinaryFile()
    {
      // Arrange
      IFile file = new File("..\\..\\data1.gif", "image/gif");
      MultipartDataFile data = new MultipartDataFile { DataFile = file, Age = 99 };
      Request formdataReq = Session.Bind(MultipartFormDataFileTemplate);

      // Act
      using (Response<string> response = formdataReq.Accept("text/plain").ContentType("multipart/form-data").Post<string>(data))
      {
        // Assert
        Assert.That(response.Headers["x-contenttype"].StartsWith("multipart/form-data"), Is.True);
        Assert.That(response.Body, Is.EqualTo("data1.gif-image/gif-R0lGODlhAgACAA==-99"));
      }
    }


    [Test]
    public void CanPostMultipartFormDataWithAdditionalFilenameSpecified()
    {
      // Arrange
      IFile file = new File("..\\..\\data1.gif", "image/gif", "other-filename.guf");
      MultipartDataFile data = new MultipartDataFile { DataFile = file, Age = 99 };
      Request formdataReq = Session.Bind(MultipartFormDataFileTemplate);

      // Act
      using (Response<string> response = formdataReq.Accept("text/plain").ContentType("multipart/form-data").Post<string>(data))
      {
        // Assert
        Assert.That(response.Headers["x-contenttype"].StartsWith("multipart/form-data"), Is.True);
        Assert.That(response.Body, Is.EqualTo("other-filename.guf-image/gif-R0lGODlhAgACAA==-99"));
      }
    }


    [Test]
    public void CanPostSMultipartFormDataFromAnonymousTypes()
    {
      // Arrange
      var data = new { Name = "Pete", Age = 10, Active = false };
      Request formdataReq = Session.Bind(MultipartFormDataTemplate);

      // Act
      using (Response<string> response = formdataReq.Accept("text/plain").ContentType("multipart/form-data").Post<string>(data))
      {
        // Assert
        Assert.That(response.Headers["x-contenttype"].StartsWith("multipart/form-data"), Is.True);
        Assert.That(response.Body, Is.EqualTo("Pete-10-False"));
      }
    }


    [Test]
    public void CanPostSimpleMultipartFormDataWithEncoding(
      [Values("UTF-8", "iso-8859-1")] string charsetIn,
      [Values("UTF-8", "iso-8859-1")] string charsetOut)
    {
      // Arrange
      MultipartData data = new MultipartData { Name = "ÆØÅüî", Age = 10, Active = true };
      Request formdataReq = Session.Bind(MultipartFormDataTemplate);
      
      // NOTE: I haven't found a way to test what encoding it actually uses for the post data, 
      // so that must be inspected using Fiddler :-(

      // Act
      using (Response<string> response = formdataReq.Accept("text/plain")
                                                    .AcceptCharset(charsetOut)
                                                    .CodecParameter("Charset", charsetIn)
                                                    .ContentType("multipart/form-data")
                                                    .Post<string>(data))
      {
        // Assert
        Assert.That(response.Headers["x-contenttype"].StartsWith("multipart/form-data"), Is.True);
        Assert.That(response.Headers["x-accept-charset"], Is.EqualTo(charsetOut));
        Assert.That(response.Body, Is.EqualTo("ÆØÅüî-10-True"));
      }
    }


    [Test]
    public void WhenSpecifyingCharsetForCodecItUsesIt()
    {
      // Arrange
      MultipartData data = new MultipartData { Name = "ÆØÅüî", Age = 10 };
      Request formdataReq = Session.Bind(MultipartFormDataTemplate);

      // Act + Assert (throws because it tries to use non-existing charset)
      // - Not the best test ever, I know ...
      AssertThrows<ArgumentException>(() =>
        formdataReq.Accept("text/plain")
                   .CodecParameter("Charset", "NON-EXISTING")
                   .ContentType("multipart/form-data")
                   .Post<string>(data),
        ex => ex.Message.Contains("NON-EXISTING"));
    }


    [Test]
    public void CanPostComplexClass()
    {
      // Arrange
      Guid g = Guid.NewGuid();
      ComplexClassForOpenRastaSerializationTests o = new ComplexClassForOpenRastaSerializationTests
      {
        X = 15,
        Y = "Abc",
        IntArray = new List<int> { 1, 2 },
        SubC = new ComplexClassForOpenRastaSerializationTests.SubClass
        {
          SubC = new ComplexClassForOpenRastaSerializationTests.SubClass
          {
            Data = new List<string> { "Benny" }
          },
          Data = new List<string> { "Brian" }
        },
        Dict = new Dictionary<string, string>(),
        Date = new DateTime(2012, 10, 30, 12, 13, 14),
        Dou = 15.234,
        GID = g
      };
      o.Dict["abc"] = "123";
      o.Dict["qwe"] = "xyz";

      Session.SerializerSettings = new ObjectSerializerSettings
      {
        ArrayFormat = "{0}:{1}",
        DictionaryFormat = "{0}:{1}",
        PropertyFormat = "{0}.{1}"
      };

      Request request = Session.Bind(ComplexClassTemplate);

      // Act
      using (Response<string> response = request.Accept("text/plain")
                                               .AsMultipartFormData()
                                               .Post<string>(o))
      {
        // Assert
        Console.WriteLine(response.Body);
        Assert.That(response.Body, Is.EqualTo("|X=15|Y=Abc|IntArray[0]=1|IntArray[1]=2|SubC.SubC.Data[0]=Benny|SubC.Data[0]=Brian|Dict[abc]=123|Dict[qwe]=xyz|Date=2012-10-30T12:13:14|Dou=15.234|GID=" + g.ToString()));
      }
    }
  }
}

using NUnit.Framework;
using Ramone.IO;
using Ramone.Tests.Common;
using System;
using System.Collections.Generic;
using Ramone.Utility;


namespace Ramone.Tests.MediaTypes.MultipartFormData
{
  [TestFixture]
  public class MultipartFormDataTests : TestHelper
  {
    [Test]
    public void CanPostSimpleMultipartFormData()
    {
      // Arrange
      MultipartData data = new MultipartData { Name = "Pete", Age = 10 };
      RamoneRequest formdataReq = Session.Bind(MultipartFormDataTemplate);

      // Act
      RamoneResponse<string> response = formdataReq.Accept("text/plain").ContentType("multipart/form-data").Post<string>(data);

      // Assert
      Assert.IsTrue(response.Headers["x-contenttype"].StartsWith("multipart/form-data"));
      Assert.AreEqual("Pete-10", response.Body);
    }


    [Test]
    public void CanPostSimpleMultipartFormDataUsingShorthand()
    {
      // Arrange
      MultipartData data = new MultipartData { Name = "Pete", Age = 10 };
      RamoneRequest formdataReq = Session.Bind(MultipartFormDataTemplate);

      // Act
      RamoneResponse<string> response = formdataReq.Accept("text/plain").AsMultipartFormData().Post<string>(data);

      // Assert
      Assert.IsTrue(response.Headers["x-contenttype"].StartsWith("multipart/form-data"));
      Assert.AreEqual("Pete-10", response.Body);
    }


    public class MultipartDataFile
    {
      public IFile DataFile { get; set; }
      public int Age { get; set; }
    }


    [Test]
    public void CanPostMultipartFormDataWithFile()
    {
      // Arrange
      IFile file = new File("..\\..\\data1.txt");
      MultipartDataFile data = new MultipartDataFile { DataFile = file, Age = 10 };
      RamoneRequest formdataReq = Session.Bind(MultipartFormDataFileTemplate);

      // Act
      RamoneResponse<string> response = formdataReq.Accept("text/plain").ContentType("multipart/form-data").Post<string>(data);

      // Assert
      Assert.IsTrue(response.Headers["x-contenttype"].StartsWith("multipart/form-data"));
      Assert.AreEqual("data1.txt-text/plain-Æüî´`'-10", response.Body);
    }


    [Test]
    public void CanPostMultipartFormDataWithBinaryFile()
    {
      // Arrange
      IFile file = new File("..\\..\\data1.gif", "image/gif");
      MultipartDataFile data = new MultipartDataFile { DataFile = file, Age = 99 };
      RamoneRequest formdataReq = Session.Bind(MultipartFormDataFileTemplate);

      // Act
      RamoneResponse<string> response = formdataReq.Accept("text/plain").ContentType("multipart/form-data").Post<string>(data);

      // Assert
      Assert.IsTrue(response.Headers["x-contenttype"].StartsWith("multipart/form-data"));
      Assert.AreEqual("data1.gif-image/gif-GIF89a-99", response.Body);
    }


    [Test]
    public void CanPostSMultipartFormDataFromAnonymousTypes()
    {
      // Arrange
      var data = new { Name = "Pete", Age = 10 };
      RamoneRequest formdataReq = Session.Bind(MultipartFormDataTemplate);

      // Act
      RamoneResponse<string> response = formdataReq.Accept("text/plain").ContentType("multipart/form-data").Post<string>(data);

      // Assert
      Assert.IsTrue(response.Headers["x-contenttype"].StartsWith("multipart/form-data"));
      Assert.AreEqual("Pete-10", response.Body);
    }


    [Test]
    public void CanPostSimpleMultipartFormDataWithEncoding(
      [Values("UTF-8", "Windows-1252", "iso-8859-1")] string charsetIn,
      [Values("UTF-8", "Windows-1252", "iso-8859-1")] string charsetOut)
    {
      // Arrange
      MultipartData data = new MultipartData { Name = "ÆØÅüî", Age = 10 };
      RamoneRequest formdataReq = Session.Bind(MultipartFormDataTemplate);

      // Act
      RamoneResponse<string> response = formdataReq.Accept("text/plain")
                                                   .AcceptCharset(charsetOut)
                                                   .Charset(charsetIn)
                                                   .ContentType("multipart/form-data")
                                                   .Post<string>(data);

      // Assert
      Assert.IsTrue(response.Headers["x-contenttype"].StartsWith("multipart/form-data"));
      Assert.AreEqual("ÆØÅüî-10", response.Body);
    }


    [Test]
    public void CanPostComplexClass()
    {
      // Arrange
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
        Dict = new Dictionary<string, string>()
      };
      o.Dict["abc"] = "123";
      o.Dict["qwe"] = "xyz";

      Session.FormUrlEncodedSerializerSettings = new ObjectSerializerSettings
      {
        ArrayFormat = "{0}:{1}",
        DictionaryFormat = "{0}:{1}",
        PropertyFormat = "{0}.{1}"
      };

      RamoneRequest request = Session.Bind(ComplexClassTemplate);

      // Act
      RamoneResponse<string> response = request.Accept("text/plain")
                                               .AsFormUrlEncoded()
                                               .Post<string>(o);

      // Assert
      Console.WriteLine(response.Body);
      Assert.AreEqual("|X=15|Y=Abc|IntArray[0]=1|IntArray[1]=2|SubC.SubC.SubC=|SubC.SubC.Data[0]=Benny|SubC.Data[0]=Brian|Dict[abc]=123|Dict[qwe]=xyz", response.Body);
    }
  }
}

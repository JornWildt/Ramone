using NUnit.Framework;
using Ramone.IO;
using Ramone.Tests.Common;


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
      Assert.AreEqual("data1.txt-text/plain; charset=UTF-8-XxxÆØÅ-10", response.Body);
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
      Assert.AreEqual("ÆØÅüî-10", response.Body);
    }
  }
}

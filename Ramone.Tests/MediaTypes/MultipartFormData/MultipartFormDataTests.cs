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
  }
}

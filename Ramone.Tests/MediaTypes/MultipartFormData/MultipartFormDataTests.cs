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
      RamoneRequest fileReq = Session.Bind(FileTemplate);

      // Act
      RamoneResponse<string> response = fileReq.Accept("text/plain").ContentType("multipart/form-data").Post<string>(data);

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
      RamoneRequest fileReq = Session.Bind(FileTemplate);

      // Act
      RamoneResponse<string> response = fileReq.Accept("text/plain").ContentType("multipart/form-data").Post<string>(data);

      // Assert
      Assert.AreEqual("data1.txt-Ramsaladin ÆØÅ-10", response.Body);
    }
  }
}

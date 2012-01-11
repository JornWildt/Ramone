using NUnit.Framework;
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
  }
}

using System.IO;
using NUnit.Framework;
using Ramone.Utility;


namespace Ramone.Tests
{
  [TestFixture]
  public class FileDownloadTests : TestHelper
  {
    [Test]
    public void CanDownloadFile()
    {
      // Arrange
      Request request = Session.Bind(FileDownloadTemplate);

      using (TempFile file = new TempFile())
      {
        // Act
        request.Get().SaveToFile(file.Path);

        // Assert
        string s = File.ReadAllText(file.Path);
        Assert.AreEqual("1234567890", s);
      }
    }
  }
}

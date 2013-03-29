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
        using (var r = request.Get())
        {
          r.SaveToFile(file.Path);

          // Assert
          string s = File.ReadAllText(file.Path);
          Assert.AreEqual("1234567890", s);
        }
      }
    }


    [Test]
    public void CanDownloadFile_Async()
    {
      // Arrange
      Request request = Session.Bind(FileDownloadTemplate);

      using (TempFile file = new TempFile())
      {
        // Act
        TestAsync(wh =>
          {
            request.Async().Get(response =>
              {
                response.SaveToFile(file.Path);

                // Assert
                string s = File.ReadAllText(file.Path);
                Assert.AreEqual("1234567890", s);
                wh.Set();
              });
          });
      }
    }
  }
}

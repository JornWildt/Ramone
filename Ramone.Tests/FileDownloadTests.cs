﻿using System.IO;
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
          Assert.That(s, Is.EqualTo("1234567890"));
        }
      }
    }


    [Test]
    public void CanDownloadFile_AsyncEvent()
    {
      // Arrange
      Request request = Session.Bind(FileDownloadTemplate);

      using (TempFile file = new TempFile())
      {
        // Act
        TestAsyncEvent(wh =>
          {
            request.AsyncEvent().Get(response =>
              {
                response.SaveToFile(file.Path);

                // Assert
                string s = File.ReadAllText(file.Path);
                Assert.That(s, Is.EqualTo("1234567890"));
                wh.Set();
              });
          });
      }
    }
  }
}

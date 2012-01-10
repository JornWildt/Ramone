using NUnit.Framework;
using Ramone.MediaTypes.Atom;
using Ramone.Tests.Common.CMS;
using System.IO;
using System.Text;


namespace Ramone.Tests.MediaTypes
{
  [TestFixture]
  public class StreamCodecTests : TestHelper
  {
    [Test]
    public void CanGetStream()
    {
      // Arrange
      RamoneRequest fileReq = Session.Bind(FileTemplate);

      // Act
      RamoneResponse<Stream> response = fileReq.Accept("application/octet-stream").Get<Stream>();

      // Assert
      Assert.AreEqual(4, response.ContentLength);
      byte[] data = new byte[4];
      response.Body.Read(data, 0, 4);
      Assert.AreEqual(6, data[0]);
      Assert.AreEqual(7, data[1]);
      Assert.AreEqual(8, data[2]);
      Assert.AreEqual(9, data[3]);
    }

    
    [Test]
    public void CanPostStream()
    {
      // Arrange
      RamoneRequest fileReq = Session.Bind(FileTemplate);

      // Act
      using (MemoryStream s = new MemoryStream(new byte[] { 10,2,30,4 }))
      {
        RamoneResponse<Stream> response =
          fileReq.Accept("application/octet-stream").ContentType("application/octet-stream").Post<Stream>(s);

        // Assert
        Assert.AreEqual(4, response.ContentLength);

      }
    }
  }
}

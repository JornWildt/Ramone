using System.IO;
using NUnit.Framework;


namespace Ramone.Tests.MediaTypes
{
  [TestFixture]
  public class ByteArrayCodecTests : TestHelper
  {
    [Test]
    public void CanGetByteArray()
    {
      // Arrange
      Request fileReq = Session.Bind(FileTemplate);

      // Act
      using (Response<byte[]> response = fileReq.Accept("application/octet-stream").Get<byte[]>())
      {
        // Assert
        Assert.AreEqual(12, response.ContentLength);
        byte[] data = response.Body;
        Assert.AreEqual((int)'H', data[0]);
        Assert.AreEqual((int)'e', data[1]);
        Assert.AreEqual((int)'l', data[2]);
        Assert.AreEqual((int)'l', data[3]);
      }
    }


    [Test]
    public void CanPostByteArray()
    {
      // Arrange
      Request fileReq = Session.Bind(FileTemplate);

      // Act
      byte[] data = new byte[] { 10, 2, 30, 4 };

      using (Response<byte[]> response =
        fileReq.Accept("application/octet-stream").ContentType("application/octet-stream").Post<byte[]>(data))
      {
        // Assert
        Assert.AreEqual(4, response.ContentLength);
        Assert.AreEqual(10, response.Body[0]);
        Assert.AreEqual(2, response.Body[1]);
        Assert.AreEqual(30, response.Body[2]);
        Assert.AreEqual(4, response.Body[3]);
      }
    }
  }
}

using System.Linq;
using NUnit.Framework;
using Ramone.Tests.Common;

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
        Assert.That(response.ContentLength, Is.EqualTo(12));
        byte[] data = response.Body;
        Assert.That(data[0], Is.EqualTo((int)'H'));
        Assert.That(data[1], Is.EqualTo((int)'e'));
        Assert.That(data[2], Is.EqualTo((int)'l'));
        Assert.That(data[3], Is.EqualTo((int)'l'));
      }
    }


    [Test]
    public void CanPostByteArray()
    {
      // Arrange
      Request fileReq = Session.Bind(FileTemplate);
      byte[] data = new byte[] { 10, 2, 30, 4 };

      // Act
      using (Response<byte[]> response =
        fileReq.Accept("application/octet-stream").ContentType("application/octet-stream").Post<byte[]>(data))
      {
        // Assert
        Assert.That(response.ContentLength, Is.EqualTo(4));
        Assert.That(response.Body[0], Is.EqualTo(10));
        Assert.That(response.Body[1], Is.EqualTo(2));
        Assert.That(response.Body[2], Is.EqualTo(30));
        Assert.That(response.Body[3], Is.EqualTo(4));
      }
    }


    [Test]
    public void CanPostByteArrayWithDefaultMediaTypeOctetStream()
    {
      // Arrange
      Request fileReq = Session.Bind(Constants.HeaderEchoPath);
      byte[] data = new byte[] { 10, 2, 30, 4 };

      // Act
      using (Response<HeaderList> response = fileReq.Post<HeaderList>(data))
      {
        HeaderList headers = response.Body;

        // Assert
        Assert.That(headers, Is.Not.Null);
        Assert.That(headers.Any(h => h == "Content-Type: application/octet-stream"), Is.True, "Must contain content type octet stream");
      }
    }
  }
}

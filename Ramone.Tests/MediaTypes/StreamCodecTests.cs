using System.IO;
using System.Linq;
using NUnit.Framework;
using Ramone.Tests.Common;

namespace Ramone.Tests.MediaTypes
{
  [TestFixture]
  public class StreamCodecTests : TestHelper
  {
    [Test]
    public void CanGetStream()
    {
      // Arrange
      Request fileReq = Session.Bind(FileTemplate);

      // Act
      using (Response<Stream> response = fileReq.Accept("application/octet-stream").Get<Stream>())
      {
        // Assert
        Assert.That(response.ContentLength, Is.EqualTo(12));
        byte[] data = new byte[12];
        response.Body.Read(data, 0, 12);
        Assert.That(data[0], Is.EqualTo((int)'H'));
        Assert.That(data[1], Is.EqualTo((int)'e'));
        Assert.That(data[2], Is.EqualTo((int)'l'));
        Assert.That(data[3], Is.EqualTo((int)'l'));
      }
    }

    
    [Test]
    public void CanPostStream()
    {
      // Arrange
      Request fileReq = Session.Bind(FileTemplate);

      // Act
      using (MemoryStream s = new MemoryStream(new byte[] { 10,2,30,4 }))
      {
        using (Response<Stream> response =
          fileReq.Accept("application/octet-stream").ContentType("application/octet-stream").Post<Stream>(s))
        {
          // Assert
          Assert.That(response.ContentLength, Is.EqualTo(4));
          Assert.That(response.Body.ReadByte(), Is.EqualTo(10));
          Assert.That(response.Body.ReadByte(), Is.EqualTo(2));
          Assert.That(response.Body.ReadByte(), Is.EqualTo(30));
          Assert.That(response.Body.ReadByte(), Is.EqualTo(4));
        }
      }
    }


    [Test]
    public void CanPostStreamWithDefaultMediaTypeOctetStream()
    {
      // Arrange
      Request fileReq = Session.Bind(Constants.HeaderEchoPath);

      // Act
      using (MemoryStream s = new MemoryStream(new byte[] { 10, 2, 30, 4 }))
      {
        using (Response<HeaderList> response = fileReq.Post<HeaderList>(s))
        {
          HeaderList headers = response.Body;

          // Assert
          Assert.IsNotNull(headers);
          Assert.IsTrue(headers.Any(h => h == "Content-Type: application/octet-stream"), "Must contain content type octet stream");
        }
      }
    }
  }
}

using NUnit.Framework;
using Ramone.MediaTypes.JsonPatch;
using System.Text;


namespace Ramone.Tests.MediaTypes.JsonPatch
{
  [TestFixture]
  public class JsonPatchCodecTests : TestHelper
  {
    [Test]
    public void CanPatchDocument()
    {
      // Arrange
      JsonPatchDocument patch = new JsonPatchDocument();
      patch.Add("/X", 10);
      patch.Remove("/Y");
      patch.Add("/øÅ", "üÆ$€");

      Request request = Session.Bind(PatchTemplate);

      // Act
      Session.DefaultEncoding = Encoding.UTF8;
      using (var response = request.Patch<string>(patch))
      {
        // Assert - patch was sent to server and streamed back again
        Assert.IsNotNull(response.Body);
        Assert.That(response.Body, Is.EqualTo(@"[{""value"":10,""op"":""add"",""path"":""/X""},{""op"":""remove"",""path"":""/Y""},{""value"":""üÆ$€"",""op"":""add"",""path"":""/øÅ""}]"));
      }
    }
  }
}

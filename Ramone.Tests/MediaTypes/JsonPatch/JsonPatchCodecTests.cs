using NUnit.Framework;
using Ramone.MediaTypes.JsonPatch;


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
      using (var response = request.Patch<string>(patch))
      {
        // Assert - patch was sent to server and streamed back again
        Assert.IsNotNull(response.Body);
        Assert.AreEqual(@"[{""value"":10,""op"":""add"",""path"":""/X""},{""op"":""remove"",""path"":""/Y""},{""value"":""\u00FC\u00C6$\u20AC"",""op"":""add"",""path"":""/\u00F8\u00C5""}]", response.Body);
      }
    }
  }
}

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

      Request request = Session.Bind(PatchTemplate);

      // Act
      using (var response = request.Patch<string>(patch))
      {
        // Assert
        Assert.IsNotNull(response.Body);
        Assert.AreEqual(@"[{""value"":10,""op"":""add"",""path"":""/X""},{""op"":""remove"",""path"":""/Y""}]", response.Body);
      }
    }
  }
}

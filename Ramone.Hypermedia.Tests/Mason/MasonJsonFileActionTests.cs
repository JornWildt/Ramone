using NUnit.Framework;
using Ramone.IO;


namespace Ramone.Hypermedia.Tests.Mason
{
  [TestFixture]
  public class MasonJsonFileActionTests : TestHelper
  {
    [Test]
    public void CanInvokeFileAction()
    {
      // Arrange
      Resource project = GetSharedProject();

      var args = new
      {
        Title = "New issue",
        Description = "Blah ...",
        Severity = 5,
        Attachment = new { Title = "Att", Description = "Blah blah" }
      };

      var attachment = new StringFile { ContentType = "text/plain", Filename = "test.txt", Data = "1234" };
      var files = new { attachment = attachment };

      using (var resp = project.Controls[MasonTestConstants.Rels.IssueAdd].Upload<Resource>(Session, args, files))
      {
        dynamic result = resp.Created();
        Assert.AreEqual("New issue", result.Title);
        Assert.AreEqual(5, result.Severity);
        Assert.IsNotNull(result.Attachments);
        Assert.AreEqual(1, result.Attachments.Count);
      }
    }
  }
}

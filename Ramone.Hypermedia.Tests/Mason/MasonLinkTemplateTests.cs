using NUnit.Framework;
using Ramone.Hypermedia.Mason;
using System.Collections.Generic;


namespace Ramone.Hypermedia.Tests.Mason
{
  [TestFixture]
  public class MasonLinkTemplateTests : TestHelper
  {
    [Test]
    public void CanInvokeLinkTemplate()
    {
      // Arrange
      Resource common = GetCommonResource();

      var args = new { text = "blah", severity = "", pid = "" };
      using (var resp = common.Controls[MasonTestConstants.Rels.IssueQuery].Invoke<Resource>(Session, args))
      {
        dynamic result = resp.Body;
        Assert.IsNotNull(result.Issues);
        Assert.IsInstanceOf<List<MasonResource>>(result.Issues);
      }
    }
  }
}

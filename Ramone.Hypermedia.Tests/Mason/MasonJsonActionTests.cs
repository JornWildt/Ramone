using NUnit.Framework;
using System;


namespace Ramone.Hypermedia.Tests.Mason
{
  [TestFixture]
  public class MasonJsonActionTests : TestHelper
  {
    [Test]
    public void CanInvokeJsonAction()
    {
      // Arrange
      Resource common = GetCommonResource();

      string code = Guid.NewGuid().ToString();
      var newProjectArgs = new { Code = code, Title = "Human resources", Description = "Blah" };
      using (var resp = common.Controls[MasonTestConstants.Rels.ProjectCreate].Invoke<Resource>(Session, newProjectArgs))
      {
        Resource project = resp.Created();
        Assert.AreEqual(code, ((dynamic)project).Code);
        Assert.AreEqual("Human resources", ((dynamic)project).Title);
        Assert.AreEqual("Blah", ((dynamic)project).Description);
      }
    }
  }
}

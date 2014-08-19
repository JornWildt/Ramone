using NUnit.Framework;
using Ramone.Hypermedia.Mason;
using System;


namespace Ramone.Hypermedia.Tests.Mason
{
  [TestFixture]
  public class MasonActionTests : TestHelper
  {
    [Test]
    public void CanInvokeJsonAction()
    {
      // Arrange
      Resource common = GetCommonResource();

      // FIXME: can we avoid including Session here?

      string code = Guid.NewGuid().ToString();
      var newProjectArgs = new { Code = code, Title = "Human resources", Description = "Blah" };
      using (var resp = common.Controls[MasonTestConstants.ProjectCreate].Invoke<Resource>(Session, newProjectArgs))
      {
        Resource project = resp.Created();
        Assert.AreEqual(code, ((dynamic)project).Code);
        Assert.AreEqual("Human resources", ((dynamic)project).Title);
        Assert.AreEqual("Blah", ((dynamic)project).Description);
      }
    }
  }
}

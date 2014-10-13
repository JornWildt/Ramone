using NUnit.Framework;
using System;
using System.Net;


namespace Ramone.Hypermedia.Tests.Mason
{
  [TestFixture]
  public class MasonVoidActionTests : TestHelper
  {
    [Test]
    public void CanInvokeVoidAction()
    {
      // Arrange: create new project and GET it
      Guid code = Guid.NewGuid();
      Resource project = CreateProject(code.ToString(), "New project", "blah ...");

      // Act
      using (var resp = project.Controls[MasonTestConstants.Rels.ProjectDelete].Invoke(Session))
      {
        Assert.AreEqual(HttpStatusCode.NoContent, resp.StatusCode);
      }
    }
  }
}

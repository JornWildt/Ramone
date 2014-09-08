using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.Hypermedia.Mason;
using System;
using System.Net;
using Ramone.IO;
using System.Collections.Generic;


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

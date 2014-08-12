using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Ramone.Hypermedia.Mason;


// Support multiple media types - fx Sirene og HAL
// - Support inheritance from Resource or IResource in the media type codecs
// Support intellisense for member variables
// Support async
// Add "Select(name, mediatype)" to IControlCollection

// Also test for link existence
// What about session?
// - ISessionLink

// .Follow() makes sense too on a Control

namespace Ramone.Hypermedia.Tests.Mason
{
  [TestFixture]
  public class MasonReaderTests : TestHelper
  {
    const string IssueTrackerIndexUrl = "http://jorn-pc/mason-demo/resource-common";


    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      // FIXME: move this to some utility feature in Hypermedia namespace
      TestService.CodecManager.AddCodec<Resource, MasonCodec>(new MediaType("application/vnd.mason+json"));
    }


    [Test]
    public void CanGetServiceIndexAsResource()
    {
      // Act
      Resource common = GetCommonResource();

      // Assert
      Assert.IsNotNull(common);
    }


    [Test]
    public void CanAccessDataOnResource()
    {
      // Arrange
      Resource common = GetCommonResource();

      // FIXME: casting to dynamic is cumbersome

      // Assert
      Assert.AreEqual("IssueTracker Demo", ((dynamic)common).Title);
    }


    [Test]
    public void CanCheckForExistenceOfControls()
    {
      // Arrange
      Resource common = GetCommonResource();

      // Assert
      Assert.IsTrue(common.Controls.Exists(MasonTestConstants.Contact));
      Assert.IsFalse(common.Controls.Exists("XXX"));
    }


    [Test]
    public void CanFollow()
    {
      // Arrange
      Resource common = GetCommonResource();

      // FIXME: can we avoid including Session here?

      // Follow link directly (alias for "Invoke")
      using (var resp = common.Controls[MasonTestConstants.Contact].Follow<Resource>(Session))
      {
        Resource contact = resp.Body;
        Assert.AreEqual("IssueTracker Demo (by Jørn Wildt)", ((dynamic)contact).Name);
      }
    }


    [Test]
    public void CanInvokeWithoutArguments()
    {
      // Arrange
      Resource common = GetCommonResource();

      // FIXME: can we avoid including Session here?

      using (var resp = common.Controls[MasonTestConstants.Contact].Invoke<Resource>(Session))
      {
        Resource contact = resp.Body;
        Assert.AreEqual("IssueTracker Demo (by Jørn Wildt)", ((dynamic)contact).Name);
      }
    }


    [Test]
    public void CanBindAndThenInvoke()
    {
      // Arrange
      Resource common = GetCommonResource();

      // FIXME: can we avoid including Session here?

      using (var resp = common.Controls[MasonTestConstants.Contact].Bind(Session).Invoke<Resource>())
      {
        Resource contact = resp.Body;
        Assert.AreEqual("IssueTracker Demo (by Jørn Wildt)", ((dynamic)contact).Name);
      }
    }

    // FIXME: repeat most tests with and without <T> Body type

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


    private Resource _commonResource;
    private Resource GetCommonResource()
    {
      if (_commonResource == null)
      {
        Request req = Session.Request(IssueTrackerIndexUrl);
        using (var resp = req.Get<Resource>())
        {
          _commonResource = resp.Body;
        }
      }
      return _commonResource;
    }
  }
}

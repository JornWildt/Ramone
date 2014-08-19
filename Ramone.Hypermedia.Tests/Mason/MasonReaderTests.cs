using NUnit.Framework;
using Ramone.Hypermedia.Mason;
using System;


// Support multiple media types - fx Sirene og HAL
// - Support inheritance from Resource or IResource in the media type codecs

namespace Ramone.Hypermedia.Tests.Mason
{
  [TestFixture]
  public class MasonReaderTests : TestHelper
  {
    class CommonResource : MasonResource
    {
      public string Title { get; set; }
      public string Description { get; set; }
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
    public void CanAccessDataOnResourceAsDynamic()
    {
      // Arrange
      Resource common = GetCommonResource();

      // FIXME: casting to dynamic is cumbersome

      // Assert
      Assert.AreEqual("IssueTracker Demo", ((dynamic)common).Title);
    }


    [Test]
    public void CanAccessDataOnResourceAsTypedClass()
    {
      // Arrange
      Request req = Session.Request(IssueTrackerIndexUrl);

      //Session.Service.CodecManager.AddCodec<CommonResource, MasonCodec>(new MediaType("application/vnd.mason+json"));

      // Act
      using (var resp = req.Get<CommonResource>())
      {
        CommonResource common = resp.Body;

        // Assert
        Assert.AreEqual("IssueTracker Demo", common.Title);
      }
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
    public void CanInvokeLink()
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
    public void CanBindAndThenInvokeLink()
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
  }
}

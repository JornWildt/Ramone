﻿using NUnit.Framework;
using Ramone.Hypermedia.Mason;
using System;


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

      // Assert
      Assert.AreEqual("IssueTracker Demo", ((dynamic)common)["@meta"]["@title"]);
    }


    [Test]
    public void CanAccessDataOnResourceAsTypedClass()
    {
      // Arrange
      Request req = Session.Request(IssueTrackerIndexUrl);

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
      Assert.IsTrue(common.Controls.Exists(MasonTestConstants.Rels.Contact));
      Assert.IsFalse(common.Controls.Exists("XXX"));
    }


    [Test]
    public void CanFollow()
    {
      // Arrange
      Resource common = GetCommonResource();

      // Follow link directly (alias for "Invoke")
      using (var resp = common.Controls[MasonTestConstants.Rels.Contact].Follow<Resource>(Session))
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

      using (var resp = common.Controls[MasonTestConstants.Rels.Contact].Invoke<Resource>(Session))
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

      using (var resp = common.Controls[MasonTestConstants.Rels.Contact].Bind(Session).Invoke<Resource>())
      {
        Resource contact = resp.Body;
        Assert.AreEqual("IssueTracker Demo (by Jørn Wildt)", ((dynamic)contact).Name);
      }
    }


    [Test]
    public void CanBindAndThenFollowLink()
    {
      // Arrange
      Resource common = GetCommonResource();

      using (var resp = common.Controls[MasonTestConstants.Rels.Contact].Bind(Session).Follow<Resource>())
      {
        Resource contact = resp.Body;
        Assert.AreEqual("IssueTracker Demo (by Jørn Wildt)", ((dynamic)contact).Name);
      }
    }
  }
}

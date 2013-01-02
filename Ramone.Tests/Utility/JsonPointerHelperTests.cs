using System.Collections.Generic;
using NUnit.Framework;
using Ramone.Utility;
using System.Linq.Expressions;
using System;


namespace Ramone.Tests.Utility
{
  [TestFixture]
  public class JsonPointerHelperTests : TestHelper
  {
    JsonPointerHelper<BugReport> PObjHelper = new JsonPointerHelper<BugReport>();
    JsonPointerHelper<List<BugReport>> PListHelper = new JsonPointerHelper<List<BugReport>>();
    JsonPointerHelper<BugReport[]> PArrayHelper = new JsonPointerHelper<BugReport[]>();
    JsonPointerHelper<Dictionary<string, BugReport>> PDictHelper = new JsonPointerHelper<Dictionary<string, BugReport>>();


    [Test]
    public void CanGetPathFromMemberExpressions()
    {
      Assert.AreEqual("/Id", PObjHelper.GetPath(r => r.Id));
      Assert.AreEqual("/Title", PObjHelper.GetPath(r => r.Title));
      Assert.AreEqual("/Status", PObjHelper.GetPath(r => r.Status));
      Assert.AreEqual("/Responsible", PObjHelper.GetPath(r => r.Responsible));
      Assert.AreEqual("/Responsible/Name", PObjHelper.GetPath(r => r.Responsible.Name));
    }


    [Test]
    public void CanGetPathFromListIndex()
    {
      Assert.AreEqual("/0", PListHelper.GetPath(r => r[0]));
      Assert.AreEqual("/0/Title", PListHelper.GetPath(r => r[0].Title));
      Assert.AreEqual("/3/ListOfRelatedParties/2/Name", PListHelper.GetPath(r => r[3].ListOfRelatedParties[2].Name));
      Assert.AreEqual("/ListOfRelatedParties/2/Name", PObjHelper.GetPath(r => r.ListOfRelatedParties[2].Name));
    }


    [Test]
    public void CanGetPathFromArrayIndex()
    {
      Assert.AreEqual("/0", PArrayHelper.GetPath(r => r[0]));
      Assert.AreEqual("/0/Title", PArrayHelper.GetPath(r => r[0].Title));
      Assert.AreEqual("/3/ArrayOfOtherParties/2/Name", PArrayHelper.GetPath(r => r[3].ArrayOfOtherParties[2].Name));
      Assert.AreEqual("/ArrayOfOtherParties/2/Name", PObjHelper.GetPath(r => r.ArrayOfOtherParties[2].Name));
    }


    [Test]
    public void CanGetPathFromDictionaryIndex()
    {
      Assert.AreEqual("/x", PDictHelper.GetPath(r => r["x"]));
      Assert.AreEqual("/x/Title", PDictHelper.GetPath(r => r["x"].Title));
      Assert.AreEqual("/y/AdditionalProperties/x", PDictHelper.GetPath(r => r["y"].AdditionalProperties["x"]));
    }


    [Test]
    public void CanGetPathForRoot()
    {
      Assert.AreEqual("/", PArrayHelper.GetPath(r => r));
    }


    [Test]
    public void CanGetPathFromConversionExpression()
    {
      Expression<Func<BugReport, object>> expr = (r => r.Id);
      Assert.AreEqual("/Id", PObjHelper.GetPath(expr));
    }
  }


  class BugReport
  {
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public StatusType Status { get; set; }
    public Party Responsible { get; set; }
    public List<Party> ListOfRelatedParties { get; set; }
    public Party[] ArrayOfOtherParties { get; set; }
    public Dictionary<string, string> AdditionalProperties { get; set; }

    public class Party
    {
      public string Name { get; set; }
      public long Id { get; set; }
    }

    public enum StatusType { Open, Closed }
  }
}

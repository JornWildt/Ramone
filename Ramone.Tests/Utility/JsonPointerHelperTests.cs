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
      Assert.That(PObjHelper.GetPath(r => r.Id), Is.EqualTo("/Id"));
      Assert.That(PObjHelper.GetPath(r => r.Title), Is.EqualTo("/Title"));
      Assert.That(PObjHelper.GetPath(r => r.Status), Is.EqualTo("/Status"));
      Assert.That(PObjHelper.GetPath(r => r.Responsible), Is.EqualTo("/Responsible"));
      Assert.That(PObjHelper.GetPath(r => r.Responsible.Name), Is.EqualTo("/Responsible/Name"));
    }


    [Test]
    public void CanGetPathFromListIndex()
    {
      Assert.That(PListHelper.GetPath(r => r[0]), Is.EqualTo("/0"));
      Assert.That(PListHelper.GetPath(r => r[0].Title), Is.EqualTo("/0/Title"));
      Assert.That(PListHelper.GetPath(r => r[3].ListOfRelatedParties[2].Name), Is.EqualTo("/3/ListOfRelatedParties/2/Name"));
      Assert.That(PObjHelper.GetPath(r => r.ListOfRelatedParties[2].Name), Is.EqualTo("/ListOfRelatedParties/2/Name"));
    }


    [Test]
    public void CanGetPathFromArrayIndex()
    {
      Assert.That(PArrayHelper.GetPath(r => r[0]), Is.EqualTo("/0"));
      Assert.That(PArrayHelper.GetPath(r => r[0].Title), Is.EqualTo("/0/Title"));
      Assert.That(PArrayHelper.GetPath(r => r[3].ArrayOfOtherParties[2].Name), Is.EqualTo("/3/ArrayOfOtherParties/2/Name"));
      Assert.That(PObjHelper.GetPath(r => r.ArrayOfOtherParties[2].Name), Is.EqualTo("/ArrayOfOtherParties/2/Name"));
    }


    [Test]
    public void CanGetPathFromDictionaryIndex()
    {
      Assert.That(PDictHelper.GetPath(r => r["x"]), Is.EqualTo("/x"));
      Assert.That(PDictHelper.GetPath(r => r["x"].Title), Is.EqualTo("/x/Title"));
      Assert.That(PDictHelper.GetPath(r => r["y"].AdditionalProperties["x"]), Is.EqualTo("/y/AdditionalProperties/x"));
    }


    [Test]
    public void CanGetPathForRoot()
    {
      Assert.That(PArrayHelper.GetPath(r => r), Is.EqualTo("/"));
    }


    [Test]
    public void CanGetPathFromConversionExpression()
    {
      Expression<Func<BugReport, object>> expr = (r => r.Id);
      Assert.That(PObjHelper.GetPath(expr), Is.EqualTo("/Id"));
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

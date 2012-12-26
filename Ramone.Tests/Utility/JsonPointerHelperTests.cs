using System.Collections.Generic;
using NUnit.Framework;
using Ramone.Utility;


namespace Ramone.Tests.Utility
{
  [TestFixture]
  public class JsonPointerHelperTests : TestHelper
  {
    JsonPointerHelper<BugReport> PObjHelper = new JsonPointerHelper<BugReport>("/");
    JsonPointerHelper<BugReport[]> PArrayHelper = new JsonPointerHelper<BugReport[]>("/");


    [Test]
    public void CanGetPathFromMemberExpressions()
    {
      Assert.AreEqual("Id", PObjHelper.GetPath(r => r.Id));
      Assert.AreEqual("Title", PObjHelper.GetPath(r => r.Title));
      Assert.AreEqual("Status", PObjHelper.GetPath(r => r.Status));
      Assert.AreEqual("Responsible", PObjHelper.GetPath(r => r.Responsible));
      Assert.AreEqual("Responsible/Name", PObjHelper.GetPath(r => r.Responsible.Name));
    }


    [Test]
    public void CanGetPathFromArrayIndex()
    {
      Assert.AreEqual("0", PArrayHelper.GetPath(r => r[0]));
      Assert.AreEqual("0/Title", PArrayHelper.GetPath(r => r[0].Title));
      Assert.AreEqual("3/RelatedParties/2/Name", PArrayHelper.GetPath(r => r[3].RelatedParties[2].Name));
      Assert.AreEqual("RelatedParties/2/Name", PObjHelper.GetPath(r => r.RelatedParties[2].Name));
    }
  }


  class BugReport
  {
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public StatusType Status { get; set; }
    public Party Responsible { get; set; }
    public List<Party> RelatedParties { get; set; }

    public class Party
    {
      public string Name { get; set; }
      public long Id { get; set; }
    }

    public enum StatusType { Open, Closed }
  }
}

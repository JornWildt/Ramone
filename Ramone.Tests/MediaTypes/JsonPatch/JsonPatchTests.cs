using System.Collections.Generic;
using NUnit.Framework;
using Ramone.MediaTypes.JsonPatch;
using JsonFx.Json;
using System.IO;
using System;


namespace Ramone.Tests.MediaTypes.JsonPatch
{
  [TestFixture]
  public class JsonPatchTests : TestHelper
  {
    [Test]
    public void CanBuildReplace()
    {
      // Arrange
      JsonPatchDocument patch = new JsonPatchDocument();

      // Act
      patch.Replace("/Title", "Bummer");

      // Assert
      dynamic operations = patch.Operations;
      Assert.IsNotNull(operations);
      Assert.AreEqual(1, operations.Count);
      Assert.AreEqual("replace", operations[0].op);
      Assert.AreEqual("/Title", operations[0].path);
      Assert.AreEqual("Bummer", operations[0].value);

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.AreEqual(@"[{""value"":""Bummer"",""op"":""replace"",""path"":""/Title""}]", patch.ToString());
    }


    [Test]
    public void CanBuildTypedReplace()
    {
      // Arrange
      JsonPatchDocument<BugReport> patch = new JsonPatchDocument<BugReport>();

      // Act
      patch.Replace(r => r.Responsible.Name, "Bummer");

      // Assert
      dynamic operations = patch.Operations;
      Assert.IsNotNull(operations);
      Assert.AreEqual(1, operations.Count);
      Assert.AreEqual("replace", operations[0].op);
      Assert.AreEqual("/Responsible/Name", operations[0].path);
      Assert.AreEqual("Bummer", operations[0].value);

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.AreEqual(@"[{""value"":""Bummer"",""op"":""replace"",""path"":""/Responsible/Name""}]", patch.ToString());
    }


    [Test]
    public void CanBuildTypedReplaceWithObjectValue()
    {
      // Arrange
      BugReport.Party party = new BugReport.Party { Name = "John" };
      JsonPatchDocument<BugReport> patch = new JsonPatchDocument<BugReport>();

      // Act
      patch.Replace(r => r.Responsible, party);

      // Assert
      dynamic operations = patch.Operations;
      Assert.IsNotNull(operations);
      Assert.AreEqual(1, operations.Count);
      Assert.AreEqual("replace", operations[0].op);
      Assert.AreEqual("/Responsible", operations[0].path);
      Assert.AreEqual(party, operations[0].value);

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.AreEqual(@"[{""value"":{""Name"":""John"",""Id"":0},""op"":""replace"",""path"":""/Responsible""}]", patch.ToString());
    }


    [Test]
    public void CanBuildRemove()
    {
      // Arrange
      JsonPatchDocument patch = new JsonPatchDocument();

      // Act
      patch.Remove("/Title");

      // Assert
      dynamic operations = patch.Operations;
      Assert.IsNotNull(operations);
      Assert.AreEqual(1, operations.Count);
      Assert.AreEqual("remove", operations[0].op);
      Assert.AreEqual("/Title", operations[0].path);

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.AreEqual(@"[{""op"":""remove"",""path"":""/Title""}]", patch.ToString());
    }


    [Test]
    public void CanBuildTypedRemove()
    {
      // Arrange
      JsonPatchDocument<BugReport> patch = new JsonPatchDocument<BugReport>();

      // Act
      patch.Remove(r => r.Responsible.Name);

      // Assert
      dynamic operations = patch.Operations;
      Assert.IsNotNull(operations);
      Assert.AreEqual(1, operations.Count);
      Assert.AreEqual("remove", operations[0].op);
      Assert.AreEqual("/Responsible/Name", operations[0].path);

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.AreEqual(@"[{""op"":""remove"",""path"":""/Responsible/Name""}]", patch.ToString());
    }


    [Test]
    public void CanBuildAdd()
    {
      // Arrange
      JsonPatchDocument patch = new JsonPatchDocument();

      // Act
      patch.Add("/RelatedParties", new BugReport.Party { Name = "Liza" });

      // Assert
      dynamic operations = patch.Operations;
      Assert.IsNotNull(operations);
      Assert.AreEqual(1, operations.Count);
      Assert.AreEqual("add", operations[0].op);
      Assert.AreEqual("/RelatedParties", operations[0].path);

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.AreEqual(@"[{""value"":{""Name"":""Liza"",""Id"":0},""op"":""add"",""path"":""/RelatedParties""}]", patch.ToString());
    }


    [Test]
    public void CanBuildTypedAdd()
    {
      // Arrange
      JsonPatchDocument<BugReport> patch = new JsonPatchDocument<BugReport>();

      // Act
      patch.Add(r => r.RelatedParties, new BugReport.Party { Name = "Liza" });

      // Assert
      dynamic operations = patch.Operations;
      Assert.IsNotNull(operations);
      Assert.AreEqual(1, operations.Count);
      Assert.AreEqual("add", operations[0].op);
      Assert.AreEqual("/RelatedParties", operations[0].path);

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.AreEqual(@"[{""value"":{""Name"":""Liza"",""Id"":0},""op"":""add"",""path"":""/RelatedParties""}]", patch.ToString());
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

using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Ramone.MediaTypes.JsonPatch;


namespace Ramone.Tests.MediaTypes.JsonPatch
{
  // Note: No need to test all sorts of typed JSON paths - these are tested extensively in JsonPointerHelperTests.


  [TestFixture]
  public class JsonPatchTests : TestHelper
  {
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
    public void CanBuildMove()
    {
      // Arrange
      JsonPatchDocument patch = new JsonPatchDocument();

      // Act
      patch.Move("/Title", "/AnotherTitle");

      // Assert
      dynamic operations = patch.Operations;
      Assert.IsNotNull(operations);
      Assert.AreEqual(1, operations.Count);
      Assert.AreEqual("move", operations[0].op);
      Assert.AreEqual("/Title", operations[0].from);
      Assert.AreEqual("/AnotherTitle", operations[0].path);

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.AreEqual(@"[{""from"":""/Title"",""op"":""move"",""path"":""/AnotherTitle""}]", patch.ToString());
    }


    [Test]
    public void CanBuildTypedMove()
    {
      // Arrange
      JsonPatchDocument<BugReport> patch = new JsonPatchDocument<BugReport>();

      // Act
      patch.Move(r => r.Title, r => r.AnotherTitle);

      // Assert
      dynamic operations = patch.Operations;
      Assert.IsNotNull(operations);
      Assert.AreEqual(1, operations.Count);
      Assert.AreEqual("move", operations[0].op);
      Assert.AreEqual("/Title", operations[0].from);
      Assert.AreEqual("/AnotherTitle", operations[0].path);

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.AreEqual(@"[{""from"":""/Title"",""op"":""move"",""path"":""/AnotherTitle""}]", patch.ToString());
    }


    [Test]
    public void CanBuildCopy()
    {
      // Arrange
      JsonPatchDocument patch = new JsonPatchDocument();

      // Act
      patch.Copy("/Title", "/AnotherTitle");

      // Assert
      dynamic operations = patch.Operations;
      Assert.IsNotNull(operations);
      Assert.AreEqual(1, operations.Count);
      Assert.AreEqual("copy", operations[0].op);
      Assert.AreEqual("/Title", operations[0].from);
      Assert.AreEqual("/AnotherTitle", operations[0].path);

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.AreEqual(@"[{""from"":""/Title"",""op"":""copy"",""path"":""/AnotherTitle""}]", patch.ToString());
    }


    [Test]
    public void CanBuildTypedCopy()
    {
      // Arrange
      JsonPatchDocument<BugReport> patch = new JsonPatchDocument<BugReport>();

      // Act
      patch.Copy(r => r.Title, r => r.AnotherTitle);

      // Assert
      dynamic operations = patch.Operations;
      Assert.IsNotNull(operations);
      Assert.AreEqual(1, operations.Count);
      Assert.AreEqual("copy", operations[0].op);
      Assert.AreEqual("/Title", operations[0].from);
      Assert.AreEqual("/AnotherTitle", operations[0].path);

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.AreEqual(@"[{""from"":""/Title"",""op"":""copy"",""path"":""/AnotherTitle""}]", patch.ToString());
    }


    [Test]
    public void CanBuildTest()
    {
      // Arrange
      JsonPatchDocument patch = new JsonPatchDocument();

      // Act
      patch.Test("/Title", "TestValue");

      // Assert
      dynamic operations = patch.Operations;
      Assert.IsNotNull(operations);
      Assert.AreEqual(1, operations.Count);
      Assert.AreEqual("test", operations[0].op);
      Assert.AreEqual("/Title", operations[0].path);
      Assert.AreEqual("TestValue", operations[0].value);

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.AreEqual(@"[{""value"":""TestValue"",""op"":""test"",""path"":""/Title""}]", patch.ToString());
    }


    [Test]
    public void CanBuildTypedTest()
    {
      // Arrange
      JsonPatchDocument<BugReport> patch = new JsonPatchDocument<BugReport>();

      // Act
      patch.Test(r => r.Title, "TestValue");

      // Assert
      dynamic operations = patch.Operations;
      Assert.IsNotNull(operations);
      Assert.AreEqual(1, operations.Count);
      Assert.AreEqual("test", operations[0].op);
      Assert.AreEqual("/Title", operations[0].path);
      Assert.AreEqual("TestValue", operations[0].value);

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.AreEqual(@"[{""value"":""TestValue"",""op"":""test"",""path"":""/Title""}]", patch.ToString());
    }


    [Test]
    public void CanReadFromReader()
    {
      // Arrange
      using (TextReader r = new StringReader(@"
[
  {""value"":{""Name"":""Liza"",""Id"":0},""op"":""add"",""path"":""/RelatedParties""},
  {""op"":""remove"",""path"":""/Title""},
  {""value"":{""Name"":""Liza"",""Id"":0},""op"":""replace"",""path"":""/Responsible""},
  {""from"":""/AnotherTitle"",""op"":""move"",""path"":""/Title""},
  {""from"":""/Title"",""op"":""copy"",""path"":""/AnotherTitle""},
  {""value"":""Liza"",""op"":""test"",""path"":""/RelatedParties""}
]"))
      {
        // Act
        JsonPatchDocument patch = JsonPatchDocument.Read(r);

        // Assert
        dynamic operations = patch.Operations;
        Assert.IsNotNull(operations);
        Assert.AreEqual(6, operations.Count);
        Assert.AreEqual("add", operations[0].op);
        Assert.AreEqual("/RelatedParties", operations[0].path);
        Assert.AreEqual("remove", operations[1].op);
        Assert.AreEqual("/Title", operations[1].path);
        Assert.AreEqual("replace", operations[2].op);
        Assert.AreEqual("/Responsible", operations[2].path);
        Assert.AreEqual("Liza", operations[2].value.Name);
        Assert.AreEqual("move", operations[3].op);
        Assert.AreEqual("/AnotherTitle", operations[3].from);
        Assert.AreEqual("/Title", operations[3].path);
        Assert.AreEqual("copy", operations[4].op);
        Assert.AreEqual("/Title", operations[4].from);
        Assert.AreEqual("/AnotherTitle", operations[4].path);
        Assert.AreEqual("test", operations[5].op);
        Assert.AreEqual("/RelatedParties", operations[5].path);
        Assert.AreEqual("Liza", operations[5].value);
      }
    }


    [Test]
    public void WhenReadingItIgnoresUnknownOperationsAndProperties()
    {
      // Arrange
      using (TextReader r = new StringReader(@"
[
  {""value"":10, ""op"":""future"", ""path"":""/RelatedParties"", xxx: 10},
  {""op"":""remove"",""path"":""/Title""}
]"))
      {
        // Act
        JsonPatchDocument patch = JsonPatchDocument.Read(r);

        // Assert
        dynamic operations = patch.Operations;
        Assert.IsNotNull(operations);
        Assert.AreEqual(1, operations.Count);
        Assert.AreEqual("remove", operations[0].op);
        Assert.AreEqual("/Title", operations[0].path);
      }
    }


    [Test]
    public void WhenReadingInvalidJsonItThrowsJsonException()
    {
      // Arrange
      using (TextReader r = new StringReader(@"
[
  {
  {""op"":""remove"",""path"":""/Title""}
]"))
      {
        // Act + Assert
        AssertThrows<JsonPatchParserException>(() => JsonPatchDocument.Read(r));
      }
    }


    [Test]
    public void WhenReadingInvalidStructureItThrowsJsonException()
    {
      // Arrange (missing array)
      using (TextReader r = new StringReader(@"{""op"":""remove"",""path"":""/Title""}"))
      {
        // Act + Assert
        AssertThrows<JsonPatchParserException>(() => JsonPatchDocument.Read(r));
      }
    }


    [Test]
    public void WhenReadingMissingValuesItThrows()
    {
      // Arrange
      using (TextReader r = new StringReader(@"
[
  {""op"":""remove""}
]"))
      {
        // Act + Assert
        AssertThrows<JsonPatchParserException>(() => JsonPatchDocument.Read(r));
      }
    }
  }


  class BugReport
  {
    public long Id { get; set; }
    public string Title { get; set; }
    public string AnotherTitle { get; set; }
    public string Description { get; set; }
    public StatusType Status { get; set; }
    public Party Responsible { get; set; }
    public List<Party> RelatedParties { get; set; }
    public DateTime Created { get; set; }

    public class Party
    {
      public string Name { get; set; }
      public long Id { get; set; }
    }

    public enum StatusType { Open, Closed }
  }
}

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
      Assert.That(operations, Is.Not.Null);
      Assert.That(operations.Count, Is.EqualTo(1));
      Assert.That(operations[0].op, Is.EqualTo("add"));
      Assert.That(operations[0].path, Is.EqualTo("/RelatedParties"));

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.That(patch.ToString(), Is.EqualTo(@"[{""value"":{""Name"":""Liza"",""Id"":0},""op"":""add"",""path"":""/RelatedParties""}]"));
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
      Assert.That(operations, Is.Not.Null);
      Assert.That(operations.Count, Is.EqualTo(1));
      Assert.That(operations[0].op, Is.EqualTo("add"));
      Assert.That(operations[0].path, Is.EqualTo("/RelatedParties"));

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.That(patch.ToString(), Is.EqualTo(@"[{""value"":{""Name"":""Liza"",""Id"":0},""op"":""add"",""path"":""/RelatedParties""}]"));
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
      Assert.That(operations, Is.Not.Null);
      Assert.That(operations.Count, Is.EqualTo(1));
      Assert.That(operations[0].op, Is.EqualTo("remove"));
      Assert.That(operations[0].path, Is.EqualTo("/Title"));

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.That(patch.ToString(), Is.EqualTo(@"[{""op"":""remove"",""path"":""/Title""}]"));
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
      Assert.That(operations, Is.Not.Null);
      Assert.That(operations.Count, Is.EqualTo(1));
      Assert.That(operations[0].op, Is.EqualTo("remove"));
      Assert.That(operations[0].path, Is.EqualTo("/Responsible/Name"));

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.That(patch.ToString(), Is.EqualTo(@"[{""op"":""remove"",""path"":""/Responsible/Name""}]"));
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
      Assert.That(operations, Is.Not.Null);
      Assert.That(operations.Count, Is.EqualTo(1));
      Assert.That(operations[0].op, Is.EqualTo("replace"));
      Assert.That(operations[0].path, Is.EqualTo("/Title"));
      Assert.That(operations[0].value, Is.EqualTo("Bummer"));

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.That(patch.ToString(), Is.EqualTo(@"[{""value"":""Bummer"",""op"":""replace"",""path"":""/Title""}]"));
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
      Assert.That(operations, Is.Not.Null);
      Assert.That(operations.Count, Is.EqualTo(1));
      Assert.That(operations[0].op, Is.EqualTo("replace"));
      Assert.That(operations[0].path, Is.EqualTo("/Responsible/Name"));
      Assert.That(operations[0].value, Is.EqualTo("Bummer"));

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.That(patch.ToString(), Is.EqualTo(@"[{""value"":""Bummer"",""op"":""replace"",""path"":""/Responsible/Name""}]"));
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
      Assert.That(operations, Is.Not.Null);
      Assert.That(operations.Count, Is.EqualTo(1));
      Assert.That(operations[0].op, Is.EqualTo("replace"));
      Assert.That(operations[0].path, Is.EqualTo("/Responsible"));
      Assert.That(operations[0].value, Is.EqualTo(party));

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.That(patch.ToString(), Is.EqualTo(@"[{""value"":{""Name"":""John"",""Id"":0},""op"":""replace"",""path"":""/Responsible""}]"));
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
      Assert.That(operations, Is.Not.Null);
      Assert.That(operations.Count, Is.EqualTo(1));
      Assert.That(operations[0].op, Is.EqualTo("move"));
      Assert.That(operations[0].from, Is.EqualTo("/Title"));
      Assert.That(operations[0].path, Is.EqualTo("/AnotherTitle"));

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.That(patch.ToString(), Is.EqualTo(@"[{""from"":""/Title"",""op"":""move"",""path"":""/AnotherTitle""}]"));
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
      Assert.That(operations, Is.Not.Null);
      Assert.That(operations.Count, Is.EqualTo(1));
      Assert.That(operations[0].op, Is.EqualTo("move"));
      Assert.That(operations[0].from, Is.EqualTo("/Title"));
      Assert.That(operations[0].path, Is.EqualTo("/AnotherTitle"));

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.That(patch.ToString(), Is.EqualTo(@"[{""from"":""/Title"",""op"":""move"",""path"":""/AnotherTitle""}]"));
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
      Assert.That(operations, Is.Not.Null);
      Assert.That(operations.Count, Is.EqualTo(1));
      Assert.That(operations[0].op, Is.EqualTo("copy"));
      Assert.That(operations[0].from, Is.EqualTo("/Title"));
      Assert.That(operations[0].path, Is.EqualTo("/AnotherTitle"));

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.That(patch.ToString(), Is.EqualTo(@"[{""from"":""/Title"",""op"":""copy"",""path"":""/AnotherTitle""}]"));
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
      Assert.That(operations, Is.Not.Null);
      Assert.That(operations.Count, Is.EqualTo(1));
      Assert.That(operations[0].op, Is.EqualTo("copy"));
      Assert.That(operations[0].from, Is.EqualTo("/Title"));
      Assert.That(operations[0].path, Is.EqualTo("/AnotherTitle"));

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.That(patch.ToString(), Is.EqualTo(@"[{""from"":""/Title"",""op"":""copy"",""path"":""/AnotherTitle""}]"));
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
      Assert.That(operations, Is.Not.Null);
      Assert.That(operations.Count, Is.EqualTo(1));
      Assert.That(operations[0].op, Is.EqualTo("test"));
      Assert.That(operations[0].path, Is.EqualTo("/Title"));
      Assert.That(operations[0].value, Is.EqualTo("TestValue"));

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.That(patch.ToString(), Is.EqualTo(@"[{""value"":""TestValue"",""op"":""test"",""path"":""/Title""}]"));
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
      Assert.That(operations, Is.Not.Null);
      Assert.That(operations.Count, Is.EqualTo(1));
      Assert.That(operations[0].op, Is.EqualTo("test"));
      Assert.That(operations[0].path, Is.EqualTo("/Title"));
      Assert.That(operations[0].value, Is.EqualTo("TestValue"));

      // Not the best way to test, but some how we need to know that it Builds good JSON
      Assert.That(patch.ToString(), Is.EqualTo(@"[{""value"":""TestValue"",""op"":""test"",""path"":""/Title""}]"));
    }


    [Test]
    public void CanChainWithFluentInterface()
    {
      // Arrange
      JsonPatchDocument patch = new JsonPatchDocument();

      // Act
      patch.Test("Title", "TestValue")
           .Replace("Description", "...")
           .Add("RelatedParties", "")
           .Remove("Responsible")
           .Copy("Title", "AnotherTitle")
           .Move("Title", "AnotherTitle");

      Assert.That(patch.Operations.Count, Is.EqualTo(6));
      Assert.That(patch.Operations[0].op, Is.EqualTo("test"));
      Assert.That(patch.Operations[5].op, Is.EqualTo("move"));
    }


    [Test]
    public void CanChainWithFluentInterface_typed()
    {
      // Arrange
      JsonPatchDocument<BugReport> patch = new JsonPatchDocument<BugReport>();

      // Act
      patch.Test(r => r.Title, "TestValue")
           .Replace(r => r.Description, "...")
           .Add(r => r.RelatedParties, "")
           .Remove(r => r.Responsible)
           .Copy(r => r.Title, r => r.AnotherTitle)
           .Move(r => r.Title, r => r.AnotherTitle);

      Assert.That(patch.Operations.Count, Is.EqualTo(6));
      Assert.That(patch.Operations[0].op, Is.EqualTo("test"));
      Assert.That(patch.Operations[5].op, Is.EqualTo("move"));
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
        Assert.That(operations, Is.Not.Null);
        Assert.That(operations.Count, Is.EqualTo(6));
        Assert.That(operations[0].op, Is.EqualTo("add"));
        Assert.That(operations[0].path, Is.EqualTo("/RelatedParties"));
        Assert.That(operations[1].op, Is.EqualTo("remove"));
        Assert.That(operations[1].path, Is.EqualTo("/Title"));
        Assert.That(operations[2].op, Is.EqualTo("replace"));
        Assert.That(operations[2].path, Is.EqualTo("/Responsible"));
        Assert.That(operations[2].value.Name, Is.EqualTo("Liza"));
        Assert.That(operations[3].op, Is.EqualTo("move"));
        Assert.That(operations[3].from, Is.EqualTo("/AnotherTitle"));
        Assert.That(operations[3].path, Is.EqualTo("/Title"));
        Assert.That(operations[4].op, Is.EqualTo("copy"));
        Assert.That(operations[4].from, Is.EqualTo("/Title"));
        Assert.That(operations[4].path, Is.EqualTo("/AnotherTitle"));
        Assert.That(operations[5].op, Is.EqualTo("test"));
        Assert.That(operations[5].path, Is.EqualTo("/RelatedParties"));
        Assert.That(operations[5].value, Is.EqualTo("Liza"));
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
        Assert.That(operations, Is.Not.Null);
        Assert.That(operations.Count, Is.EqualTo(1));
        Assert.That(operations[0].op, Is.EqualTo("remove"));
        Assert.That(operations[0].path, Is.EqualTo("/Title"));
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
    public DateTime? LastModified { get; set; }

    public class Party
    {
      public string Name { get; set; }
      public long Id { get; set; }
    }

    public enum StatusType { Open, Closed }
  }
}

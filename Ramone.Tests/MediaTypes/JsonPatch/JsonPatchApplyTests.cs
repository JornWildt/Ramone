using System;
using NUnit.Framework;
using Ramone.MediaTypes.JsonPatch;


namespace Ramone.Tests.MediaTypes.JsonPatch
{
  [TestFixture]
  public class JsonPatchApplyTests : TestHelper
  {
    [Test]
    public void CanApplyPatchDocument()
    {
      // Arrange
      JsonPatchDocument doc = new JsonPatchDocument();
      doc.Add("/A", 1);
      doc.Remove("/A");
      doc.Replace("/A", 2);
      doc.Move("/A", "/B");
      doc.Copy("/A", "/B");
      doc.Test("/A", 3);

      PatchVisitor callback = new PatchVisitor();

      // Act
      doc.Apply(callback);

      // Assert
      Assert.AreEqual(doc.ToString(), callback.newDoc.ToString());
      Assert.IsTrue(callback.IsComplete, "Apply() method must call Complete.");
    }


    [Test]
    public void CanUseIfMatchInVisitor()
    {
      // Arrange
      JsonPatchDocument doc = new JsonPatchDocument();
      doc.Add("/Id", 10);

      BugReportPatchVisitor callback = new BugReportPatchVisitor();

      // Act
      doc.Apply(callback);

      // Assert
      Assert.AreEqual("/Id => 10|", callback.Result);
    }


    [Test]
    public void DefaultPatchVisitorFailsOnUnknownPaths()
    {
      // Arrange
      JsonPatchDocument doc = new JsonPatchDocument();
      doc.Add("/XxxUnknown", 10);

      EmptyPatchVisitor callback = new EmptyPatchVisitor();

      // Act + Assert
      AssertThrows<JsonPatchParserException>(() => doc.Apply(callback));
    }


    [Test]
    public void IfMatchHandlesNullValuesForNonNullable()
    {
      // Arrange
      JsonPatchDocument doc = new JsonPatchDocument();
      doc.Add("/Created", null);

      BugReportPatchVisitor callback = new BugReportPatchVisitor();

      // Act + Assert
      AssertThrows<JsonPatchParserException>(() => doc.Apply(callback));
    }


    [Test]
    public void IfMatchHandlesNullValuesForNullable()
    {
      // Arrange
      JsonPatchDocument doc = new JsonPatchDocument();
      doc.Add("/Responsible", null);
      doc.Add("/Title", null);
      doc.Add("/LastModified", null);

      BugReportPatchVisitor callback = new BugReportPatchVisitor();

      // Act
      doc.Apply(callback);

      // Assert
      Assert.AreEqual("/Responsible => |/Title => |/LastModified => |", callback.Result);
    }
  }


  internal class EmptyPatchVisitor : JsonPatchDocumentVisitor
  {
  }


  internal class PatchVisitor : JsonPatchDocumentVisitor
  {
    public JsonPatchDocument newDoc = new JsonPatchDocument();
    public bool IsComplete = false;

    public override bool Add(string path, object value)
    {
      newDoc.Add(path, value);
      return true;
    }

    public override bool Remove(string path)
    {
      newDoc.Remove(path);
      return true;
    }

    public override bool Replace(string path, object value)
    {
      newDoc.Replace(path, value);
      return true;
    }

    public override bool Copy(string from, string path)
    {
      newDoc.Copy(from, path);
      return true;
    }

    public override bool Move(string from, string path)
    {
      newDoc.Move(from, path);
      return true;
    }

    public override bool Test(string path, object value)
    {
      newDoc.Test(path, value);
      return true;
    }

    public override void Complete()
    {
      IsComplete = true;
    }
  }



  internal class BugReportPatchVisitor : JsonPatchDocumentVisitor<BugReport>
  {
    public string Result = "";

    public override bool Add(string path, object value)
    {
      return 
        IfMatch<int>(r => r.Id, path, value,
          v => Result += string.Format("{0} => {1}|", path, v))
      ||
        IfMatch<DateTime>(r => r.Created, path, value,
          v => Result += string.Format("{0} => {1}|", path, v))
      ||
        IfMatch<DateTime?>(r => r.LastModified, path, value,
          v => Result += string.Format("{0} => {1}|", path, v))
      ||
        IfMatch<dynamic>(r => r.Responsible, path, value,
          v => Result += string.Format("{0} => {1}|", path, v))
      ||
        IfMatch<string>(r => r.Title, path, value,
          v => Result += string.Format("{0} => {1}|", path, v));
    }
  }
}

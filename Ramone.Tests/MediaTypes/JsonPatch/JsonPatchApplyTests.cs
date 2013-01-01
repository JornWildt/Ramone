using System.Collections.Generic;
using System.IO;
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
  }


  internal class PatchVisitor : JsonPatchDocumentVisitor
  {
    public JsonPatchDocument newDoc = new JsonPatchDocument();
    public bool IsComplete = false;

    public override void Add(string path, object value)
    {
      newDoc.Add(path, value);
    }

    public override void Remove(string path)
    {
      newDoc.Remove(path);
    }

    public override void Replace(string path, object value)
    {
      newDoc.Replace(path, value);
    }

    public override void Copy(string from, string path)
    {
      newDoc.Copy(from, path);
    }

    public override void Move(string from, string path)
    {
      newDoc.Move(from, path);
    }

    public override void Test(string path, object value)
    {
      newDoc.Test(path, value);
    }

    public override void Complete()
    {
      IsComplete = true;
    }
  }
}

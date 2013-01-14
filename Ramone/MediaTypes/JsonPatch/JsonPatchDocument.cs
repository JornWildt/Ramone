using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using JsonFx.Json;
using Ramone.Utility;
using System.Collections;
using CuttingEdge.Conditions;
using JsonFx.Serialization;


namespace Ramone.MediaTypes.JsonPatch
{
  /// <summary>
  /// Represents a JSON patch document.
  /// </summary>
  /// <remarks>Create an instance and add operations using the Add/Remove/etc. methods. Then write the JSON document
  /// to any stream using Write() or print it using ToString(). Or use Read() to read a complete JSON document from
  /// a TextReader.
  /// See http://tools.ietf.org/html/draft-ietf-appsawg-json-patch-08 for information about JSON patch.
  /// </remarks>
  public class JsonPatchDocument
  {
    protected List<Operation> OperationList { get; set; }


    public JsonPatchDocument()
    {
      OperationList = new List<Operation>();
    }


    public JsonPatchDocument Add(string path, object value)
    {
      Condition.Requires(path, "path").IsNotNull();
      OperationList.Add(new ValueOperation { op = "add", path = path, value = value });
      return this;
    }


    public JsonPatchDocument Remove(string path)
    {
      Condition.Requires(path, "path").IsNotNull();
      OperationList.Add(new Operation { op = "remove", path = path });
      return this;
    }


    public JsonPatchDocument Replace(string path, object value)
    {
      Condition.Requires(path, "path").IsNotNull();
      OperationList.Add(new ValueOperation { op = "replace", path = path, value = value });
      return this;
    }


    public JsonPatchDocument Move(string from, string path)
    {
      Condition.Requires(from, "from").IsNotNull();
      Condition.Requires(path, "path").IsNotNull();
      OperationList.Add(new FromOperation { op = "move", from = from, path = path });
      return this;
    }


    public JsonPatchDocument Copy(string from, string path)
    {
      Condition.Requires(from, "from").IsNotNull();
      Condition.Requires(path, "path").IsNotNull();
      OperationList.Add(new FromOperation { op = "copy", from = from, path = path });
      return this;
    }


    public JsonPatchDocument Test(string path, object value)
    {
      Condition.Requires(path, "path").IsNotNull();
      OperationList.Add(new ValueOperation { op = "test", path = path, value = value });
      return this;
    }


    public List<Operation> Operations
    {
      get
      {
        return OperationList;
      }
    }


    public void Write(TextWriter w)
    {
      JsonWriter jsw = new JsonWriter();
      jsw.Write(Operations, w);
    }


    public static JsonPatchDocument Read(TextReader r)
    {
      JsonPatchDocument patch = new JsonPatchDocument();

      JsonReader jsr = new JsonReader();
      CompleteOperation[] operations = null;
      try
      {
        operations = jsr.Read<CompleteOperation[]>(r);
      }
      catch (DeserializationException ex)
      {
        throw new JsonPatchParserException(ex.Message, ex);
      }

      foreach (CompleteOperation operation in operations)
      {
        try
        {
          switch (operation.op)
          {
            case "add":
              patch.Add(operation.path, operation.value);
              break;
            case "remove":
              patch.Remove(operation.path);
              break;
            case "replace":
              patch.Replace(operation.path, operation.value);
              break;
            case "move":
              patch.Move(operation.from, operation.path);
              break;
            case "copy":
              patch.Copy(operation.from, operation.path);
              break;
            case "test":
              patch.Test(operation.path, operation.value);
              break;
            case null:
              throw new JsonPatchParserException("No 'op' property found.");
          }
        }
        catch (ArgumentNullException ex)
        {
          throw new JsonPatchParserException(string.Format("Missing parameter '{0}' for op:'{1}'.", ex.ParamName, operation.op), ex);
        }
      }

      return patch;
    }


    public void Apply(IJsonPatchDocumentVisitor visitor)
    {
      foreach (Operation op in Operations)
      {
        op.Apply(visitor);
      }
      visitor.Complete();
    }


    public override string ToString()
    {
      using (TextWriter w = new StringWriter())
      {
        Write(w);
        return w.ToString();
      }
    }


    /// <summary>
    /// For writing patch documents.
    /// </summary>
    public class Operation
    {
      public string op { get; set; }

      public string path { get; set; }

      public virtual void Apply(IJsonPatchDocumentVisitor visitor)
      {
        bool ok = false;
        if (op == "remove")
          ok = visitor.Remove(path);
        if (!ok)
          throw new JsonPatchParserException(string.Format("Unsupported patch operation '{0}' on path '{1}'.", op, path));
      }
    }


    public class ValueOperation : Operation
    {
      public object value { get; set; }

      public override void Apply(IJsonPatchDocumentVisitor visitor)
      {
        bool ok = false;
        if (op == "add")
          ok = visitor.Add(path, value);
        else if (op == "replace")
          ok = visitor.Replace(path, value);
        else if (op == "test")
          ok = visitor.Test(path, value);
        if (!ok)
          throw new JsonPatchParserException(string.Format("Unsupport patch operation '{0}' on path '{1}'.", op, path));
      }
    }


    public class FromOperation : Operation
    {
      public string from { get; set; }

      public override void Apply(IJsonPatchDocumentVisitor visitor)
      {
        bool ok = false;
        if (op == "copy")
          ok = visitor.Copy(from, path);
        else if (op == "move")
          ok = visitor.Move(from, path);
        if (!ok)
          throw new JsonPatchParserException(string.Format("Unsupport patch operation '{0}' on path '{1}' from '{2}'.", op, path, from));
      }
    }


    /// <summary>
    /// For reading patch documents (and turning them into "Operation" classes).
    /// </summary>
    protected class CompleteOperation
    {
      public string op { get; set; }
      public string path { get; set; }
      public object value { get; set; }
      public string from { get; set; }
    }
  }


  public class JsonPatchDocument<TDocument> : JsonPatchDocument
  {
    private JsonPointerHelper<TDocument> PathHelper = new JsonPointerHelper<TDocument>("/");


    public JsonPatchDocument<TDocument> Add<TProperty>(Expression<Func<TDocument, TProperty>> path, object value)
    {
      string spath = PathHelper.GetPath(path);
      return (JsonPatchDocument<TDocument>)Add(spath, value);
    }


    public JsonPatchDocument<TDocument> Replace<TProperty>(Expression<Func<TDocument, TProperty>> path, object value)
    {
      string spath = PathHelper.GetPath(path);
      return (JsonPatchDocument<TDocument>)Replace(spath, value);
    }


    public JsonPatchDocument<TDocument> Remove<TProperty>(Expression<Func<TDocument, TProperty>> path)
    {
      string spath = PathHelper.GetPath(path);
      return (JsonPatchDocument<TDocument>)Remove(spath);
    }


    public JsonPatchDocument<TDocument> Move<TProperty>(Expression<Func<TDocument, TProperty>> from, Expression<Func<TDocument, TProperty>> path)
    {
      string sfrom = PathHelper.GetPath(from);
      string spath = PathHelper.GetPath(path);
      return (JsonPatchDocument<TDocument>)Move(sfrom, spath);
    }


    public JsonPatchDocument<TDocument> Copy<TProperty>(Expression<Func<TDocument, TProperty>> from, Expression<Func<TDocument, TProperty>> path)
    {
      string sfrom = PathHelper.GetPath(from);
      string spath = PathHelper.GetPath(path);
      return (JsonPatchDocument<TDocument>)Copy(sfrom, spath);
    }


    public JsonPatchDocument<TDocument> Test<TProperty>(Expression<Func<TDocument, TProperty>> path, object value)
    {
      string spath = PathHelper.GetPath(path);
      return (JsonPatchDocument<TDocument>)Test(spath, value);
    }
  }
}

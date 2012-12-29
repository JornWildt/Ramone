using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using JsonFx.Json;
using Ramone.Utility;
using System.Collections;


namespace Ramone.MediaTypes.JsonPatch
{
  /// <summary>
  /// Represents a JSON patch document.
  /// </summary>
  /// <remarks>Create an instance and add operations using the Add/Remove/etc. methods. Then write the JSON document
  /// to any stream using Write() or print it using ToString().
  /// See http://tools.ietf.org/html/draft-ietf-appsawg-json-patch-08 for information about JSON patch.
  /// </remarks>
  public class JsonPatchDocument
  {
    protected List<Operation> OperationList { get; set; }


    public JsonPatchDocument()
    {
      OperationList = new List<Operation>();
    }


    public void Add(string path, object value)
    {
      OperationList.Add(new ValueOperation { op = "add", path = path, value = value });
    }


    public void Remove(string path)
    {
      OperationList.Add(new Operation { op = "remove", path = path });
    }


    public void Replace(string path, object value)
    {
      OperationList.Add(new ValueOperation { op = "replace", path = path, value = value });
    }


    public void Move(string from, string path)
    {
      OperationList.Add(new FromOperation { op = "move", from = from, path = path });
    }


    public void Copy(string from, string path)
    {
      OperationList.Add(new FromOperation { op = "copy", from = from, path = path });
    }


    public void Test(string path, object value)
    {
      OperationList.Add(new ValueOperation { op = "test", path = path, value = value });
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
      CompleteOperation[] operations = jsr.Read<CompleteOperation[]>(r);
      foreach (CompleteOperation operation in operations)
      {
        switch (operation.op)
        {
          case "add":
            patch.Add(operation.path, operation.value);
            break;
          case "remove":
            patch.Remove(operation.path);
            break;
        }
      }

      return patch;
    }


    public override string ToString()
    {
      using (TextWriter w = new StringWriter())
      {
        Write(w);
        return w.ToString();
      }
    }


    public class Operation
    {
      public string op { get; set; }

      public string path { get; set; }
    }


    public class ValueOperation : Operation
    {
      public object value { get; set; }
    }


    public class FromOperation : Operation
    {
      public string from { get; set; }
    }


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


    public void Add<TProperty>(Expression<Func<TDocument, TProperty>> path, object value)
    {
      string spath = "/" + PathHelper.GetPath(path);
      Add(spath, value);
    }


    public void Replace<TProperty>(Expression<Func<TDocument, TProperty>> path, object value)
    {
      string spath = "/" + PathHelper.GetPath(path);
      Replace(spath, value);
    }


    public void Remove<TProperty>(Expression<Func<TDocument, TProperty>> path)
    {
      string spath = "/" + PathHelper.GetPath(path);
      Remove(spath);
    }


    public void Move<TProperty>(Expression<Func<TDocument, TProperty>> from, Expression<Func<TDocument, TProperty>> path)
    {
      string sfrom = "/" + PathHelper.GetPath(from);
      string spath = "/" + PathHelper.GetPath(path);
      Move(sfrom, spath);
    }


    public void Copy<TProperty>(Expression<Func<TDocument, TProperty>> from, Expression<Func<TDocument, TProperty>> path)
    {
      string sfrom = "/" + PathHelper.GetPath(from);
      string spath = "/" + PathHelper.GetPath(path);
      Copy(sfrom, spath);
    }


    public void Test<TProperty>(Expression<Func<TDocument, TProperty>> path, object value)
    {
      string spath = "/" + PathHelper.GetPath(path);
      Test(spath, value);
    }
  }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using JsonFx.Json;
using System.Linq.Expressions;
using Ramone.Utility;

namespace Ramone.MediaTypes.JsonPatch
{
  public class JsonPatchDocument
  {
    protected List<PatchOperation> OperationList { get; set; }


    public JsonPatchDocument()
    {
      OperationList = new List<PatchOperation>();
    }


    public void Add(string path, object value)
    {
      OperationList.Add(new PatchValueOperation { op = "add", path = path, value = value });
    }


    public void Remove(string path)
    {
      OperationList.Add(new PatchOperation { op = "remove", path = path });
    }


    public void Replace(string path, object value)
    {
      OperationList.Add(new PatchValueOperation { op = "replace", path = path, value = value });
    }


    public void Move(string from, string path)
    {
      OperationList.Add(new PatchFromOperation { op = "move", from = from, path = path });
    }


    public void Copy(string from, string path)
    {
      OperationList.Add(new PatchFromOperation { op = "copy", from = from, path = path });
    }


    public void Test(string path, object value)
    {
      OperationList.Add(new PatchValueOperation { op = "test", path = path, value = value });
    }


    public List<PatchOperation> Operations
    {
      get
      {
        return OperationList;
      }
    }


    public void WriteDocument(TextWriter w)
    {
      JsonWriter jsw = new JsonWriter();
      jsw.Write(Operations, w);
    }


    public override string ToString()
    {
      using (TextWriter w = new StringWriter())
      {
        WriteDocument(w);
        return w.ToString();
      }
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


  public class PatchOperation
  {
    public string op { get; set; }
    
    public string path { get; set; }
  }


  public class PatchValueOperation : PatchOperation
  {
    public object value { get; set; }
  }


  public class PatchFromOperation : PatchOperation
  {
    public string from { get; set; }
  }
}

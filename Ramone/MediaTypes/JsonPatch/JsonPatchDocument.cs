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


    public void Add<P>(string path, P value)
    {
      OperationList.Add(new PatchValueOperation { op = "add", path = path, value = value });
    }


    public void Replace<P>(string path, P value)
    {
      OperationList.Add(new PatchValueOperation { op = "replace", path = path, value = value });
    }


    public void Remove(string path)
    {
      OperationList.Add(new PatchOperation { op = "remove", path = path });
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
      OperationList.Add(new PatchValueOperation { op = "add", path = spath, value = value });
    }


    public void Replace<TProperty>(Expression<Func<TDocument, TProperty>> path, object value)
    {
      string spath = "/" + PathHelper.GetPath(path);
      OperationList.Add(new PatchValueOperation { op = "replace", path = spath, value = value });
    }


    public void Remove<TProperty>(Expression<Func<TDocument, TProperty>> path)
    {
      string spath = "/" + PathHelper.GetPath(path);
      OperationList.Add(new PatchOperation { op = "remove", path = spath });
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
}

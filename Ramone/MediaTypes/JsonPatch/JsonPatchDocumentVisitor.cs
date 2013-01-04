using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Ramone.Utility;

namespace Ramone.MediaTypes.JsonPatch
{
  public abstract class JsonPatchDocumentVisitor : IJsonPatchDocumentVisitor
  {
    #region IJsonPatchDocumentVisitor Members

    public virtual void Add(string path, object value)
    {
    }

    public virtual void Remove(string path)
    {
    }

    public virtual void Replace(string path, object value)
    {
    }

    public virtual void Move(string from, string path)
    {
    }

    public virtual void Copy(string from, string path)
    {
    }

    public virtual void Test(string path, object value)
    {
    }

    public virtual void Complete()
    {
    }

    #endregion
  }


  public class JsonPatchDocumentVisitor<TDocument> : JsonPatchDocumentVisitor
  {
    protected JsonPointerHelper<TDocument> JsonPointer = new JsonPointerHelper<TDocument>();

    /// <summary>
    /// Try to match paths
    /// </summary>
    /// <remarks>Test path againt JSON pointer build from lambda expression - if they match 
    /// then convert the value to TValue and pass that to the supplied action.</remarks>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="expr"></param>
    /// <param name="path"></param>
    /// <param name="value"></param>
    /// <param name="a"></param>
    public void IfMatch<TValue>(Expression<Func<TDocument, object>> expr, string path, object value, Action<TValue> a)
    {
      if (JsonPointer.GetPath(expr) == path)
      {
        if (value is TValue)
          a((TValue)value);
        else
          throw new JsonPatchParserException(string.Format("Unable to convert '{0}' to {1} (got {2}).", path, typeof(TValue), value.GetType()));
      }
    }
  }
}

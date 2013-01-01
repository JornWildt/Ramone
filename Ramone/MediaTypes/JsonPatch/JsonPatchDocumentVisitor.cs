using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

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
}

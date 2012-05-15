using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ramone.HyperMedia
{
  public class SelectFailed : InvalidOperationException
  {
    public SelectFailed(string msg)
      : base(msg)
    {
    }


    public SelectFailed(string msg, Exception innerException)
      : base(msg, innerException)
    {
    }
  }
}

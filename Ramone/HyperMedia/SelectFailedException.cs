using System;


namespace Ramone.HyperMedia
{
  public class SelectFailedException : InvalidOperationException
  {
    public SelectFailedException(string msg)
      : base(msg)
    {
    }


    public SelectFailedException(string msg, Exception innerException)
      : base(msg, innerException)
    {
    }
  }
}

using System;
using System.Net;


namespace Ramone
{
  public class RamoneException : Exception
  {
    public HttpWebResponse Response { get; private set; }

    
    public RamoneException(HttpWebResponse response, WebException ex)
      : base(ex.Message, ex)
    {
      Response = response;
    }


    public RamoneException(string message, Exception ex)
      : base(message, ex)
    {
    }
  }


  public class NotAuthorizedException : RamoneException
  {
    public NotAuthorizedException(HttpWebResponse response, WebException ex)
      : base(response, ex)
    {
    }
  }


  public class NotCreatedException : RamoneException
  {
    public NotCreatedException(HttpWebResponse response, WebException ex)
      : base(response, ex)
    {
    }
  }
}

using System;


namespace Ramone
{
  public class AsyncEventError
  {
    public Exception Exception { get; private set; }
    public Response Response { get; private set; }

    public AsyncEventError(Exception exception, Response response)
    {
      Exception = exception;
      Response = response;
    }
  }
}

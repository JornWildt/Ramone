using System;


namespace Ramone
{
  public class AsyncError
  {
    public Exception Exception { get; private set; }
    public Response Response { get; private set; }

    public AsyncError(Exception exception, Response response)
    {
      Exception = exception;
      Response = response;
    }
  }
}

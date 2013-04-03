using System.Net;


namespace Ramone
{
  public class AsyncError
  {
    public WebException Exception { get; private set; }
    public Response Response { get; private set; }

    public AsyncError(WebException exception, Response response)
    {
      Exception = exception;
      Response = response;
    }
  }
}

using System.Net;


namespace Ramone
{
  public interface IRequestInterceptor
  {
    void HeadersReady(RequestContext context);
    void DataSent(RequestContext context);
  }
}

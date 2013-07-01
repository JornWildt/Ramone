using System.Net;


namespace Ramone
{
  public interface IRequestInterceptor
  {
    void HeadersReady(RequestContext context);
    void DataSent(RequestContext context);
  }

  public interface IRequestInterceptor2 : IRequestInterceptor
  {
    void MethodSet(RequestContext context);
  }
}

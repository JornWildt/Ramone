namespace Ramone
{
  public interface IResponseInterceptor
  {
    void ResponseReady(ResponseContext response);
  }
}

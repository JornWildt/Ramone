using System.Net;


namespace Ramone
{
  public interface IRequestInterceptor
  {
    void Intercept(HttpWebRequest request);
  }
}

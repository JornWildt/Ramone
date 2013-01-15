using System.Net;


namespace Ramone.OAuth2
{
  public class BearerTokenRequestInterceptor : IRequestInterceptor
  {
    string Token { get; set; }


    public BearerTokenRequestInterceptor(string token)
    {
      Token = token;
    }


    #region IRequestInterceptor Members

    public void HeadersReady(RequestContext context)
    {
      HttpWebRequest request = context.Request;
      request.Headers["Authorization"] = "BEARER " + Token;
    }


    public void DataSent(RequestContext context)
    {
    }

    #endregion
  }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ramone
{
  public class AsyncRequest : BaseRequest
  {
    public AsyncRequest(Request request)
      : base(request)
    {
    }


    #region GET

    public async Task<Response> Get()
    {
      return await DoRequestAsync("GET");
    }


    public async Task<Response<TResponse>> Get<TResponse>() where TResponse : class
    {
      return await DoRequestAsync<TResponse>("GET");
    }

    #endregion GET



    #region Generic request

    protected async Task<Response> DoRequestAsync(string method, int retryLevel = 0)
    {
      return await DoRequestAsync(Url, method, true, req => req.Accept = GetAcceptHeader(null), retryLevel);
    }


    protected async Task<Response<TResponse>> DoRequestAsync<TResponse>(string method, int retryLevel = 0) where TResponse : class
    {
      Response r = await DoRequestAsync(Url, method, true, req => req.Accept = GetAcceptHeader(null), retryLevel);
      return new Response<TResponse>(r, r.RedirectCount);
    }


    protected async Task<Response> DoRequestAsync(Uri url, string method, bool includeBody, Action<HttpWebRequest> requestModifier, int retryLevel = 0)
    {
      WebRequest request = SetupRequest(url, method, includeBody, requestModifier);

      HttpWebResponse response = ((HttpWebResponse)await request.GetResponseAsync());
      Response r = HandleResponse(response, method, includeBody, requestModifier, retryLevel);
      return r;
    }

    #endregion
  }
}

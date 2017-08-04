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

    #endregion


    #region POST

    public async Task<Response> Post()
    {
      return await DoRequestAsync("POST");
    }


    public async Task<Response<TResponse>> Post<TResponse>() where TResponse : class
    {
      return await DoRequestAsync<TResponse>("POST");
    }

    public async Task<Response> Post(object body)
    {
      SetBody(body);
      return await DoRequestAsync("POST");
    }


    public async Task<Response<TResponse>> Post<TResponse>(object body) where TResponse : class
    {
      SetBody(body);
      return await DoRequestAsync<TResponse>("POST");
    }

    #endregion


    #region PUT

    public async Task<Response> Put()
    {
      return await DoRequestAsync("PUT");
    }


    public async Task<Response<TResponse>> Put<TResponse>() where TResponse : class
    {
      return await DoRequestAsync<TResponse>("PUT");
    }

    public async Task<Response> Put(object body)
    {
      SetBody(body);
      return await DoRequestAsync("PUT");
    }


    public async Task<Response<TResponse>> Put<TResponse>(object body) where TResponse : class
    {
      SetBody(body);
      return await DoRequestAsync<TResponse>("PUT");
    }

    #endregion


    #region DELETE

    public async Task<Response> Delete()
    {
      return await DoRequestAsync("DELETE");
    }


    public async Task<Response<TResponse>> Delete<TResponse>() where TResponse : class
    {
      return await DoRequestAsync<TResponse>("DELETE");
    }

    #endregion


    #region EXECUTE

    public async Task<Response> Execute(string method)
    {
      return await DoRequestAsync(method);
    }


    public async Task<Response<TResponse>> Execute<TResponse>(string method) where TResponse : class
    {
      return await DoRequestAsync<TResponse>(method);
    }

    public async Task<Response> Execute(string method, object body)
    {
      SetBody(body);
      return await DoRequestAsync(method);
    }


    public async Task<Response<TResponse>> Execute<TResponse>(string method, object body) where TResponse : class
    {
      SetBody(body);
      return await DoRequestAsync<TResponse>(method);
    }

    #endregion



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
      HttpWebRequest request = SetupRequest(url, method, includeBody, requestModifier);
      ApplyHeadersReadyInterceptors(request);

      if (includeBody && BodyData != null)
      {
        using (Stream o = await request.GetRequestStreamAsync())
        {
          ApplyRequestStreamWrappers(o, request);

          if (BodyCodec is IMediaTypeWriterAsync)
            await ((IMediaTypeWriterAsync)BodyCodec).WriteToAsync(new WriterContext(o, BodyData, request, Session, CodecParameters));
          else
            BodyCodec.WriteTo(new WriterContext(o, BodyData, request, Session, CodecParameters));
        }
      }

      HttpWebResponse response = ((HttpWebResponse)await request.GetResponseAsync());
      Response r = HandleResponse(response, method, includeBody, requestModifier, retryLevel);
      return r;
    }

    #endregion
  }


  public class RamoneAsyncRequest<TResponse> : AsyncRequest
    where TResponse : class
  {
    public RamoneAsyncRequest(Request request)
      : base(request)
    {
    }


    #region Standard methods

    public new async Task<Response<TResponse>> Get()
    {
      return await Get<TResponse>();
    }


    public new async Task<Response<TResponse>> Post(object body)
    {
      return await Post<TResponse>(body);
    }


    public new async Task<Response<TResponse>> Post()
    {
      return await Post<TResponse>();
    }


    public new async Task<Response<TResponse>> Put(object body)
    {
      return await Put<TResponse>(body);
    }


    public new async Task<Response<TResponse>> Put()
    {
      return await Put<TResponse>();
    }

    public new async Task<Response<TResponse>> Delete()
    {
      return await Delete<TResponse>();
    }

    #endregion


    #region Generic methods

    public new async Task<Response<TResponse>> Execute(string method)
    {
      return await Execute<TResponse>(method);
    }


    public new async Task<Response<TResponse>> Execute(string method, object body)
    {
      return await Execute<TResponse>(method, body);
    }

    #endregion
  }
}

using System;
using System.Net;
using System.IO;


namespace Ramone
{
  public class AsyncRequest : Request
  {
    private Action<Response> ResponseCallback { get; set; }

    private Action CompleteAction { get; set; }

    private Action<AsyncError> ErrorAction { get; set; }

    public AsyncRequest(Request r)
      : base(r)
    {
    }


    #region Standard methods

    public void Get<TResponse>(Action<Response<TResponse>> callback) where TResponse : class
    {
      ResponseCallback = (r => callback(new Response<TResponse>(r, 0)));
      DoRequest("GET");
    }


    public void Get(Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest("GET");
    }


    public void Post<TResponse>(Action<Response<TResponse>> callback) where TResponse : class
    {
      ResponseCallback = (r => callback(new Response<TResponse>(r, 0)));
      DoRequest("POST");
    }


    public void Post(Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest("POST");
    }


    public void Post<TResponse>(object body, Action<Response<TResponse>> callback) where TResponse : class
    {
      Body(body);
      ResponseCallback = (r => callback(new Response<TResponse>(r, 0)));
      DoRequest("POST");
    }


    public void Post(object body, Action<Response> callback)
    {
      Body(body);
      ResponseCallback = callback;
      DoRequest("POST");
    }


    public void Put<TResponse>(Action<Response<TResponse>> callback) where TResponse : class
    {
      ResponseCallback = (r => callback(new Response<TResponse>(r, 0)));
      DoRequest("PUT");
    }


    public void Put(Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest("PUT");
    }


    public void Put<TResponse>(object body, Action<Response<TResponse>> callback) where TResponse : class
    {
      Body(body);
      ResponseCallback = (r => callback(new Response<TResponse>(r, 0)));
      DoRequest("PUT");
    }


    public void Put(object body, Action<Response> callback)
    {
      Body(body);
      ResponseCallback = callback;
      DoRequest("PUT");
    }


    public void Delete<TResponse>(Action<Response<TResponse>> callback) where TResponse : class
    {
      ResponseCallback = (r => callback(new Response<TResponse>(r, 0)));
      DoRequest("DELETE");
    }


    public void Delete(Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest("DELETE");
    }


    public void Patch<TResponse>(object body, Action<Response<TResponse>> callback) where TResponse : class
    {
      Body(body);
      ResponseCallback = (r => callback(new Response<TResponse>(r, 0)));
      DoRequest("PATCH");
    }


    public void Patch(object body, Action<Response> callback)
    {
      Body(body);
      ResponseCallback = callback;
      DoRequest("Patch");
    }


    public void Head(Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest("HEAD");
    }


    public void Options<TResponse>(Action<Response<TResponse>> callback) where TResponse : class
    {
      ResponseCallback = (r => callback(new Response<TResponse>(r, 0)));
      DoRequest("OPTIONS");
    }


    public void Options(Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest("OPTIONS");
    }


    #endregion Standard methods


    #region Generic methods

    public void Execute<TResponse>(string method, Action<Response<TResponse>> callback) where TResponse : class
    {
      ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest(method);
    }


    public void Execute(string method, Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest(method);
    }


    public void Execute<TResponse>(string method, object body, Action<Response<TResponse>> callback) where TResponse : class
    {
      Body(body);
      ResponseCallback = (r => callback(new Response<TResponse>(r, 0)));
      DoRequest(method);
    }


    public void Execute(string method, object body, Action<Response> callback)
    {
      Body(body);
      ResponseCallback = callback;
      DoRequest(method);
    }


    #endregion


    #region Disable standard methods from base class

    public override Response Get()
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Get<TResponse>()
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response Post(object body)
    {
      throw new InvalidOperationException("Synchronous POST operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Post<TResponse>(object body)
    {
      throw new InvalidOperationException("Synchronous POST operation is not supported on Asynchronous requests.");
    }

    public override Response Post()
    {
      throw new InvalidOperationException("Synchronous POST operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Post<TResponse>()
    {
      throw new InvalidOperationException("Synchronous POST operation is not supported on Asynchronous requests.");
    }

    public override Response Put(object body)
    {
      throw new InvalidOperationException("Synchronous PUT operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Put<TResponse>(object body)
    {
      throw new InvalidOperationException("Synchronous PUT operation is not supported on Asynchronous requests.");
    }

    public override Response Put()
    {
      throw new InvalidOperationException("Synchronous PUT operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Put<TResponse>()
    {
      throw new InvalidOperationException("Synchronous PUT operation is not supported on Asynchronous requests.");
    }

    public override Response Delete()
    {
      throw new InvalidOperationException("Synchronous DELETE operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Delete<TResponse>()
    {
      throw new InvalidOperationException("Synchronous DELETE operation is not supported on Asynchronous requests.");
    }

    public override Response Patch(object body)
    {
      throw new InvalidOperationException("Synchronous PATCH operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Patch<TResponse>(object body)
    {
      throw new InvalidOperationException("Synchronous PATCH operation is not supported on Asynchronous requests.");
    }

    public override Response Patch()
    {
      throw new InvalidOperationException("Synchronous PATCH operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Patch<TResponse>()
    {
      throw new InvalidOperationException("Synchronous PATCH operation is not supported on Asynchronous requests.");
    }

    public override Response Options()
    {
      throw new InvalidOperationException("Synchronous OPTIONS operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Options<TResponse>()
    {
      throw new InvalidOperationException("Synchronous OPTIONS operation is not supported on Asynchronous requests.");
    }

    public override Response Head()
    {
      throw new InvalidOperationException("Synchronous HEAD operation is not supported on Asynchronous requests.");
    }

    public override Response Execute(string method)
    {
      throw new InvalidOperationException("Synchronous operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Execute<TResponse>(string method)
    {
      throw new InvalidOperationException("Synchronous operation is not supported on Asynchronous requests.");
    }

    public override Response Execute(string method, object body)
    {
      throw new InvalidOperationException("Synchronous operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Execute<TResponse>(string method, object body)
    {
      throw new InvalidOperationException("Synchronous operation is not supported on Asynchronous requests.");
    }

    #endregion


    public virtual AsyncRequest OnComplete(Action completeAction)
    {
      CompleteAction = completeAction;
      return this;
    }


    public virtual AsyncRequest OnError(Action<AsyncError> errorAction)
    {
      ErrorAction = errorAction;
      return this;
    }


    /// <summary>
    /// Do actual async request.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="method"></param>
    /// <param name="includeBody">Indicates a redirect where the available POST data should be ignored (if false).</param>
    /// <param name="requestModifier"></param>
    /// <param name="retryLevel"></param>
    /// <returns>Always null.</returns>
    protected override Response DoRequest(Uri url, string method, bool includeBody, Action<HttpWebRequest> requestModifier, int retryLevel = 0)
    {
      HttpWebRequest request = SetupRequest(url, method, includeBody, requestModifier);

      AsyncState state = new AsyncState
      {
        IncludeBody = includeBody,
        Method = method,
        RequestModifier = requestModifier,
        RetryLevel = retryLevel,
        Url = url,
        Request = request
      };

      if (includeBody && BodyData != null)
      {
        request.BeginGetRequestStream(HandleGetRequestStream, state);
      }
      else
      {
        ApplyHeadersReadyInterceptors(request);
        request.BeginGetResponse(HandleResponse, state);
      }

      return null;
    }


    private void HandleGetRequestStream(IAsyncResult result)
    {
      AsyncState state = (AsyncState)result.AsyncState;

      using (Stream requestStream = state.Request.EndGetRequestStream(result))
      {
        WriteBody(requestStream, state.Request, state.IncludeBody);
      }

      state.Request.BeginGetResponse(HandleResponse, state);
    }


    private void HandleResponse(IAsyncResult result)
    {
      AsyncState state = (AsyncState)result.AsyncState;

      try
      {
        HttpWebResponse response = state.Request.EndGetResponse(result) as HttpWebResponse;
        using (Response r = HandleResponse(response, state.Method, state.IncludeBody, state.RequestModifier, state.RetryLevel))
        {
          if (r != null)
          {
            ResponseCallback(r);
          }
        }

        if (CompleteAction != null)
          CompleteAction();
      }
      catch (WebException ex)
      {
        HandleWebExceptionResult exResult = HandleWebException(ex, state.Url, state.Method, state.IncludeBody, state.RequestModifier, state.RetryLevel);
        if (!exResult.Retried)
        {
          if (ErrorAction != null)
            ErrorAction(new AsyncError(ex, new Response((HttpWebResponse)ex.Response, Session, state.RetryLevel)));
          if (CompleteAction != null)
            CompleteAction();
        }
      }
    }


    private class AsyncState
    {
      public Uri Url { get; set; }
      public string Method { get; set; }
      public bool IncludeBody { get; set; }
      public Action<HttpWebRequest> RequestModifier { get; set; }
      public int RetryLevel { get; set; }
      public HttpWebRequest Request { get; set; }
    }
  }
}

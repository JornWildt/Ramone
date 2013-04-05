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

    #region GET

    public override Response Get()
    {
      Get(null);
      return null;
    }


    public override Response<TResponse> Get<TResponse>()
    {
      Get<TResponse>(null);
      return null;
    }


    public virtual void Get(Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest("GET");
    }


    public virtual void Get<TResponse>(Action<Response<TResponse>> callback) where TResponse : class
    {
      if (callback != null)
        ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest("GET");
    }

    #endregion GET


    #region POST

    public override Response Post(object body)
    {
      Post(body, null);
      return null;
    }

    
    public override Response Post()
    {
      Post(null, null);
      return null;
    }

    
    public override Response<TResponse> Post<TResponse>(object body)
    {
      Post<TResponse>(body, null);
      return null;
    }

    
    public override Response<TResponse> Post<TResponse>()
    {
      Post<TResponse>(null, null);
      return null;
    }


    public virtual void Post(object body, Action<Response> callback)
    {
      Body(body);
      ResponseCallback = callback;
      DoRequest("POST");
    }


    public virtual void Post(Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest("POST");
    }


    public virtual void Post<TResponse>(Action<Response<TResponse>> callback) where TResponse : class
    {
      if (callback != null)
        ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest("POST");
    }


    public virtual void Post<TResponse>(object body, Action<Response<TResponse>> callback) where TResponse : class
    {
      Body(body);
      if (callback != null)
        ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest("POST");
    }

    #endregion POST


    #region PUT

    public override Response Put(object body)
    {
      Put(body, null);
      return null;
    }

    
    public override Response Put()
    {
      Put(null, null);
      return null;
    }

    
    public override Response<TResponse> Put<TResponse>(object body)
    {
      Put<TResponse>(body, null);
      return null;
    }

    
    public override Response<TResponse> Put<TResponse>()
    {
      Put<TResponse>(null, null);
      return null;
    }


    public virtual void Put(object body, Action<Response> callback)
    {
      Body(body);
      ResponseCallback = callback;
      DoRequest("PUT");
    }


    public virtual void Put(Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest("PUT");
    }


    public virtual void Put<TResponse>(Action<Response<TResponse>> callback) where TResponse : class
    {
      if (callback != null)
        ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest("PUT");
    }


    public virtual void Put<TResponse>(object body, Action<Response<TResponse>> callback) where TResponse : class
    {
      Body(body);
      if (callback != null)
        ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest("PUT");
    }

    #endregion PUT


    #region DELETE

    public override Response Delete()
    {
      Delete(null);
      return null;
    }


    public override Response<TResponse> Delete<TResponse>()
    {
      Delete<TResponse>(null);
      return null;
    }


    public virtual void Delete(Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest("GET");
    }


    public virtual void Delete<TResponse>(Action<Response<TResponse>> callback) where TResponse : class
    {
      if (callback != null)
        ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest("DELETE");
    }

    #endregion DELETE


    public virtual void Patch<TResponse>(object body, Action<Response<TResponse>> callback) where TResponse : class
    {
      Body(body);
      ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest("PATCH");
    }


    public virtual void Patch(object body, Action<Response> callback)
    {
      Body(body);
      ResponseCallback = callback;
      DoRequest("Patch");
    }


    public virtual void Head(Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest("HEAD");
    }


    public virtual void Options<TResponse>(Action<Response<TResponse>> callback) where TResponse : class
    {
      ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest("OPTIONS");
    }


    public virtual void Options(Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest("OPTIONS");
    }


    #endregion Standard methods


    #region Generic methods

    public virtual void Execute<TResponse>(string method, Action<Response<TResponse>> callback) where TResponse : class
    {
      ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest(method);
    }


    public virtual void Execute(string method, Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest(method);
    }


    public virtual void Execute<TResponse>(string method, object body, Action<Response<TResponse>> callback) where TResponse : class
    {
      Body(body);
      ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest(method);
    }


    public virtual void Execute(string method, object body, Action<Response> callback)
    {
      Body(body);
      ResponseCallback = callback;
      DoRequest(method);
    }


    #endregion


    #region Disable standard methods from base class

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


    /// <summary>
    /// Register callback for when all asynchronous operations and optional error handling has completed (including redirects and response callback).
    /// </summary>
    /// <param name="completeAction"></param>
    /// <returns></returns>
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
            if (ResponseCallback != null)
              ResponseCallback(r);
            if (CompleteAction != null)
              CompleteAction();
          }
        }
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

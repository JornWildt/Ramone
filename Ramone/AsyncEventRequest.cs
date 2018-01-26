using System;
using System.Net;
using System.IO;


namespace Ramone
{
  public class AsyncEventRequest : BaseRequest
  {
    private Action<Response> ResponseCallback { get; set; }

    private Action CompleteAction { get; set; }

    private Action<AsyncEventError> ErrorAction { get; set; }

    public AsyncEventRequest(Request r)
      : base(r)
    {
    }


    #region GET

    public void Get()
    {
      Get(null);
    }


    public void Get<TResponse>() where TResponse : class
    {
      Get<TResponse>(null);
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

    public void Post(object body)
    {
      Post(body, null);
    }

    
    public void Post()
    {
      Post(null, null);
    }


    public void Post<TResponse>(object body) where TResponse : class
    {
      Post<TResponse>(body, null);
    }


    public void Post<TResponse>() where TResponse : class
    {
      Post<TResponse>(null, null);
    }


    public virtual void Post(object body, Action<Response> callback)
    {
      SetBody(body);
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
      SetBody(body);
      if (callback != null)
        ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest("POST");
    }

    #endregion POST


    #region PUT

    public void Put(object body)
    {
      Put(body, null);
    }

    
    public void Put()
    {
      Put(null, null);
    }


    public void Put<TResponse>(object body) where TResponse : class
    {
      Put<TResponse>(body, null);
    }


    public void Put<TResponse>() where TResponse : class
    {
      Put<TResponse>(null, null);
    }


    public virtual void Put(object body, Action<Response> callback)
    {
      SetBody(body);
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
      SetBody(body);
      if (callback != null)
        ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest("PUT");
    }

    #endregion PUT


    #region DELETE

    public void Delete()
    {
      Delete(null);
    }


    public void Delete<TResponse>() where TResponse : class
    {
      Delete<TResponse>(null);
    }


    public virtual void Delete(Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest("DELETE");
    }


    public virtual void Delete<TResponse>(Action<Response<TResponse>> callback) where TResponse : class
    {
      if (callback != null)
        ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest("DELETE");
    }

    #endregion DELETE


    #region PATCH

    public void Patch(object body)
    {
      Patch(body, null);
    }


    public void Patch()
    {
      Patch(null, null);
    }


    public void Patch<TResponse>(object body) where TResponse : class
    {
      Patch<TResponse>(body, null);
    }


    public void Patch<TResponse>() where TResponse : class
    {
      Patch<TResponse>(null, null);
    }


    public virtual void Patch(object body, Action<Response> callback)
    {
      SetBody(body);
      ResponseCallback = callback;
      DoRequest("PATCH");
    }


    public virtual void Patch(Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest("PATCH");
    }


    public virtual void Patch<TResponse>(Action<Response<TResponse>> callback) where TResponse : class
    {
      if (callback != null)
        ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest("PATCH");
    }


    public virtual void Patch<TResponse>(object body, Action<Response<TResponse>> callback) where TResponse : class
    {
      SetBody(body);
      if (callback != null)
        ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest("PATCH");
    }

    #endregion PATCH

    
    #region HEAD

    public void Head()
    {
      Head(null);
    }


    public virtual void Head(Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest("HEAD");
    }

    #endregion


    #region OPTIONS

    public void Options()
    {
      Options(null);
    }


    public void Options<TResponse>() where TResponse : class
    {
      Options<TResponse>(null);
    }


    public virtual void Options(Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest("OPTIONS");
    }


    public virtual void Options<TResponse>(Action<Response<TResponse>> callback) where TResponse : class
    {
      if (callback != null)
        ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest("OPTIONS");
    }

    #endregion OPTIONS


    #region Generic methods

    public void Execute(string method, object body)
    {
      Execute(method, body, null);
    }


    public void Execute(string method)
    {
      Execute(method, null, null);
    }


    public void Execute<TResponse>(string method, object body) where TResponse : class
    {
      Execute<TResponse>(method, body, null);
    }


    public void Execute<TResponse>(string method) where TResponse : class
    {
      Execute<TResponse>(method, null, null);
    }


    public virtual void Execute(string method, object body, Action<Response> callback)
    {
      SetBody(body);
      ResponseCallback = callback;
      DoRequest(method);
    }


    public virtual void Execute(string method, Action<Response> callback)
    {
      ResponseCallback = callback;
      DoRequest(method);
    }


    public virtual void Execute<TResponse>(string method, Action<Response<TResponse>> callback) where TResponse : class
    {
      if (callback != null)
        ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest(method);
    }


    public virtual void Execute<TResponse>(string method, object body, Action<Response<TResponse>> callback) where TResponse : class
    {
      SetBody(body);
      if (callback != null)
        ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest(method);
    }


    /// <summary>
    /// Submit request using previously registered method and payload.
    /// </summary>
    public virtual void Submit()
    {
      Submit(null);
    }


    /// <summary>
    /// Submit request using previously registered method and payload.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public virtual void Submit<TResponse>() where TResponse : class
    {
      Submit<TResponse>(null);
    }


    /// <summary>
    /// Submit request using previously registered method payload.
    /// </summary>
    public virtual void Submit(Action<Response> callback)
    {
      if (string.IsNullOrEmpty(SubmitMethod))
        throw new InvalidOperationException("Missing method for Submit(). Call Method() first.");
      ResponseCallback = callback;
      DoRequest(SubmitMethod);
    }


    /// <summary>
    /// Submit request using previously registered method payload.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public virtual void Submit<TResponse>(Action<Response<TResponse>> callback) where TResponse : class
    {
      if (string.IsNullOrEmpty(SubmitMethod))
        throw new InvalidOperationException("Missing method for Submit(). Call Method() first.");
      if (callback != null)
        ResponseCallback = (r => callback(new Response<TResponse>(r, r.RedirectCount)));
      DoRequest<TResponse>(SubmitMethod);
    }
    
    #endregion


    /// <summary>
    /// Register callback for when all asynchronous operations and optional error handling has completed (including redirects and response callback).
    /// </summary>
    /// <param name="completeAction"></param>
    /// <returns></returns>
    public virtual AsyncEventRequest OnComplete(Action completeAction)
    {
      CompleteAction = completeAction;
      return this;
    }


    public virtual AsyncEventRequest OnError(Action<AsyncEventError> errorAction)
    {
      ErrorAction = errorAction;
      return this;
    }


    protected override Response DoRequest(string method, int retryLevel = 0)
    {
      DoRequest(Url, method, true, req => req.Accept = GetAcceptHeader(null), retryLevel);
      return null;
    }


    protected override Response<TResponse> DoRequest<TResponse>(string method, int retryLevel = 0)
    {
      DoRequest(Url, method, true, req => req.Accept = GetAcceptHeader(null), retryLevel);
      return null;
    }


    internal HttpWebRequest CurrentAsyncRequest { get; set; }

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
      CurrentAsyncRequest = SetupRequest(url, method, includeBody, requestModifier);

      AsyncState state = new AsyncState
      {
        IncludeBody = includeBody,
        Method = method,
        RequestModifier = requestModifier,
        RetryLevel = retryLevel,
        Url = url,
        Request = CurrentAsyncRequest
      };

      if (includeBody && BodyData != null)
      {
        CurrentAsyncRequest.BeginGetRequestStream(HandleGetRequestStream, state);
      }
      else
      {
        ApplyHeadersReadyInterceptors(CurrentAsyncRequest);
        CurrentAsyncRequest.BeginGetResponse(HandleResponse, state);
      }

      return null;
    }


    public void CancelAsync()
    {
      if (CurrentAsyncRequest != null)
      {
        HttpWebRequest r = CurrentAsyncRequest;
        CurrentAsyncRequest = null;
        r.Abort();
      }
    }


    private void HandleGetRequestStream(IAsyncResult result)
    {
      AsyncState state = (AsyncState)result.AsyncState;

      try
      {
        using (Stream requestStream = state.Request.EndGetRequestStream(result))
        {
          WriteBody(requestStream, state.Request, state.IncludeBody);
        }

        state.Request.BeginGetResponse(HandleResponse, state);
      }
      catch (WebException ex)
      {
        Response response = new Response((HttpWebResponse)ex.Response, Session, state.RetryLevel);
        HandleWebExceptionResult exResult = HandleWebException(ex, state.Url, state.Method, state.IncludeBody, state.RequestModifier, state.RetryLevel);
        if (!exResult.Retried)
        {
          if (ex.Status != WebExceptionStatus.RequestCanceled)
          {
            TryCallErrorCallback(ex, response);
          }
          TryCallCompleteCallback(response);
        }
      }
      finally
      {
        CurrentAsyncRequest = null;
      }
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
            TryCallResponseCallback(r);
            TryCallCompleteCallback(r);
          }
        }
      }
      catch (WebException ex)
      {
        Response response = new Response((HttpWebResponse)ex.Response, Session, state.RetryLevel);
        HandleWebExceptionResult exResult = HandleWebException(ex, state.Url, state.Method, state.IncludeBody, state.RequestModifier, state.RetryLevel);
        if (!exResult.Retried)
        {
          if (ex.Status != WebExceptionStatus.RequestCanceled)
          {
            TryCallErrorCallback(ex, response);
          }
          TryCallCompleteCallback(response);
        }
      }
      finally
      {
        CurrentAsyncRequest = null;
      }
    }


    protected void TryCallResponseCallback(Response r)
    {
      if (ResponseCallback != null)
      {
        try
        {
          ResponseCallback(r);
        }
        catch (Exception ex)
        {
          TryCallErrorCallback(ex, r);
        }
      }
    }


    protected void TryCallCompleteCallback(Response r)
    {
      if (CompleteAction != null)
      {
        try
        {
          CompleteAction();
        }
        catch (Exception ex)
        {
          TryCallErrorCallback(ex, r);
        }
      }
    }


    protected void TryCallErrorCallback(Exception ex, Response r)
    {
      if (ErrorAction != null)
        ErrorAction(new AsyncEventError(ex, r));
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

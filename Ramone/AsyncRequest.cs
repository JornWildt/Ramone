using System;
using System.Net;


namespace Ramone
{
  public class AsyncRequest : Request
  {
    private Action<Response> ResponseCallback { get; set; }

    private Action CompleteAction { get; set; }

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
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Post<TResponse>(object body)
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response Post()
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Post<TResponse>()
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response Put(object body)
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Put<TResponse>(object body)
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response Put()
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Put<TResponse>()
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response Delete()
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Delete<TResponse>()
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response Patch(object body)
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Patch<TResponse>(object body)
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response Patch()
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Patch<TResponse>()
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response Options()
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Options<TResponse>()
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response Head()
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response Execute(string method)
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Execute<TResponse>(string method)
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response Execute(string method, object body)
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    public override Response<TResponse> Execute<TResponse>(string method, object body)
    {
      throw new InvalidOperationException("Synchronous GET operation is not supported on Asynchronous requests.");
    }

    #endregion


    public AsyncRequest OnComplete(Action completeAction)
    {
      CompleteAction = completeAction;
      return this;
    }


    protected override Response DoRequest(Uri url, string method, bool includeBody, Action<HttpWebRequest> requestModifier, int retryLevel = 0)
    {
      HttpWebRequest request = SetupRequest(url, method, includeBody, requestModifier);
      AsynState state = new AsynState
      {
        IncludeBody = includeBody,
        Method = method,
        RequestModifier = requestModifier,
        RetryLevel = retryLevel,
        Url = url,
        Request = request
      };
      request.BeginGetResponse(HandleResponse, state);
      return null;
    }


    private void HandleResponse(IAsyncResult result)
    {
      AsynState state = (AsynState)result.AsyncState;

      try
      {
        HttpWebResponse response = state.Request.EndGetResponse(result) as HttpWebResponse;
        using (Response r = HandleResponse(response, state.Method, state.IncludeBody, state.RequestModifier, state.RetryLevel))
        {
          ResponseCallback(r);
        }

        if (CompleteAction != null)
          CompleteAction();
      }
      catch (WebException ex)
      {
        Response response = HandleWebException(ex, state.Url, state.Method, state.IncludeBody, state.RequestModifier, state.RetryLevel);
        if (response == null)
          throw;
        ResponseCallback(response);
      }
    }


    private class AsynState
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

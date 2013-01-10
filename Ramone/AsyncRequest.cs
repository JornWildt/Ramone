using System;
using System.Net;


namespace Ramone
{
  public class AsyncRequest : Request
  {
    private Action<Response> ResponseCallback { get; set; }

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

    protected override Response DoRequest(Uri url, string method, bool includeBody, Action<HttpWebRequest> requestModifier, int retryLevel = 0)
    {
      HttpWebRequest request = SetupRequest(url, method, includeBody, requestModifier);
      request.BeginGetResponse(HandleResponse, request);
      return null;
    }


    private void HandleResponse(IAsyncResult result)
    {
      HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
      ResponseCallback(new Response(response, Session, 0));
    }
  }
}

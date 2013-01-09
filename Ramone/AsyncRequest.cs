using System;
using System.Net;


namespace Ramone
{
  public class AsyncRequest : Request
  {
    private Action<Response> ResponseCallback { get; set; }

    public AsyncRequest(Request r, Action<Response> callback)
      : base(r)
    {
      ResponseCallback = callback;
    }


    protected override Response DoRequest(Uri url, string method, bool includeBody, Action<HttpWebRequest> requestModifier, int retryLevel = 0)
    {
      HttpWebRequest request = SetupRequest(url, method, includeBody, requestModifier);
      request.BeginGetResponse(HandleResponse, request);
      return null;
    }


    private void HandleResponse(IAsyncResult result)
    {
      HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
      ResponseCallback(new Response(response, null, 0));
    }
  }
}

using System;
using System.Collections.Generic;
using System.Net;
using Ramone.Utility;


namespace Ramone
{
  public class Response : IDisposable
  {
    public HttpWebResponse WebResponse { get; protected set; }

    public MediaType ContentType { get; protected set; }

    public long ContentLength { get { return WebResponse.ContentLength; } }

    public HttpStatusCode StatusCode { get { return WebResponse.StatusCode; } }

    public Uri Location { get { return WebResponse.LocationAsUri(); } }

    public Uri ResponseUri { get { return WebResponse.ResponseUri; } }

    public Uri BaseUri { get { return new Uri(WebResponse.ResponseUri.GetLeftPart(UriPartial.Path)); } }

    public int RedirectCount { get; protected set; }

    public ISession Session { get; protected set; }

    public Guid? ConnectionId { get; protected set; }


    public Response(HttpWebResponse response, ISession session, int retryCount, Guid? connectionId = null)
    {
      WebResponse = response;
      ReadContentType();
      Session = session;
      RedirectCount = retryCount;
      ConnectionId = connectionId;
    }


    private void ReadContentType()
    {
      if (WebResponse != null && WebResponse.ContentType != null)
      {
        string error;
        MediaType contentType = null;
        MediaType.TryParse(WebResponse.ContentType, out contentType, out error);
        ContentType = contentType;
      }
    }


    public WebHeaderCollection Headers
    {
      get { return WebResponse.Headers; }
    }


    public T Decode<T>() where T : class
    {
      if (WebResponse.ContentLength == 0 || ContentType == null || WebResponse.StatusCode == HttpStatusCode.NoContent)
        return null;

      MediaTypeReaderRegistration reader = Session.Service.CodecManager.GetReader(typeof(T), ContentType);
      ReaderContext context = new ReaderContext(WebResponse.GetResponseStream(), typeof(T), WebResponse, Session);
      T result = (T)reader.Codec.ReadFrom(context);
      ContextRegistrator.RegisterContext(Session, BaseUri, result);
      return result;
    }


    private object _body;
    public object Body
    {
      get
      {
        if (_body == null)
          _body = Decode<object>();
        return _body;
      }
    }


    public Uri CreatedLocation
    {
      get
      {
        if (WebResponse.StatusCode != HttpStatusCode.Created)
          return null;

        if (WebResponse.Headers[HttpResponseHeader.Location] == null)
          return null;

        return new Uri(WebResponse.Headers[HttpResponseHeader.Location]);
      }
    }


    public object Created()
    {
      return Created<object>();
    }


    public T Created<T>() where T : class
    {
      if (WebResponse.StatusCode != HttpStatusCode.Created)
        return null;

      T body = Decode<T>();
      if (body == null)
      {
        if (WebResponse.Headers[HttpResponseHeader.Location] == null)
          return null;

        Request request = Session.Request(WebResponse.Headers[HttpResponseHeader.Location]);
        using (var response = request.Get<T>())
          body = response.Body;
      }

      return body;
    }


    protected void ApplyResponseReadyInterceptors(HttpWebResponse response)
    {
      foreach (KeyValuePair<string, IResponseInterceptor> interceptor in Session.ResponseInterceptors)
      {
        var context = new ResponseContext(response, Session);
        interceptor.Value.ResponseReady(context);
      }
    }


    public void Dispose()
    {
      if (WebResponse != null)
        WebResponse.Close();
      if (ConnectionId != null)
        ConnectionStatistics.DiscardConnection(ConnectionId.Value);
    }
  }


  public class Response<TBody> : Response
    where TBody : class
  {
    public Response(HttpWebResponse response, ISession session, int retryCount, Guid? id = null)
      : base(response, session, retryCount, id)
    {
    }


    public Response(Response src, int retryCount)
      : base(src.WebResponse, src.Session, retryCount, src.ConnectionId)
    {
    }


    private TBody _body;
    public new TBody Body
    {
      get 
      {
        if (_body == null)
          _body = Decode<TBody>();
        return _body; 
      }
    }


    public new TBody Created()
    {
      return base.Created<TBody>();
    }
  }
}

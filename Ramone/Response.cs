using System;
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

    public Uri BaseUri { get { return new Uri(WebResponse.ResponseUri.GetLeftPart(UriPartial.Path)); } }

    public int RedirectCount { get; protected set; }

    public ISession Session { get; protected set; }

    public Guid? ConnectionId { get; protected set; }


    public Response(HttpWebResponse response, ISession session, int retryCount, Guid? connectionId = null)
    {
      WebResponse = response;
      try
      {
        // FIXME: TryParse
        ContentType = new MediaType(WebResponse.ContentType);
      }
      catch (Exception)
      {
        ContentType = null;
      }
      Session = session;
      RedirectCount = retryCount;
      ConnectionId = connectionId;
    }


    public WebHeaderCollection Headers
    {
      get { return WebResponse.Headers; }
    }


    public T Decode<T>() where T : class
    {
      if (WebResponse.ContentLength == 0 || ContentType == null || WebResponse.StatusCode == HttpStatusCode.NoContent)
        return null;

      IMediaTypeReader reader = Session.Service.CodecManager.GetReader(typeof(T), ContentType).Codec;
      ReaderContext context = new ReaderContext(WebResponse.GetResponseStream(), typeof(T), WebResponse, Session);
      T result = (T)reader.ReadFrom(context);
      ContextRegistrator.RegisterContext(Session, BaseUri, result);
      return result;
    }


    public object Body
    {
      get
      {
        return Decode<object>();
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


    public TBody Created()
    {
      return base.Created<TBody>();
    }
  }
}

using System;
using System.Net;
using Ramone.HyperMedia;


namespace Ramone
{
  public class Response
  {
    public HttpWebResponse WebResponse { get; protected set; }

    public MediaType ContentType { get; protected set; }

    public long ContentLength { get { return WebResponse.ContentLength; } }

    public HttpStatusCode StatusCode { get { return WebResponse.StatusCode; } }

    public Uri BaseUri { get { return new Uri(WebResponse.ResponseUri.GetLeftPart(UriPartial.Path)); } }

    public IRamoneSession Session { get; protected set; }


    public Response(HttpWebResponse response, IRamoneSession session)
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
      return result;
    }


    public object Body
    {
      get
      {
        return Decode<object>();
      }
    }


    public Uri CreatedLocation()
    {
      if (WebResponse.StatusCode != HttpStatusCode.Created)
        return null;

      if (WebResponse.Headers[HttpResponseHeader.Location] == null)
        return null;

      return new Uri(WebResponse.Headers[HttpResponseHeader.Location]);
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
        body = request.Get<T>().Body;
      }

      return body;
    }
  }


  public class Response<TBody> : Response
    where TBody : class
  {
    public Response(HttpWebResponse response, IRamoneSession session)
      : base(response, session)
    {
    }


    public Response(Response src)
      : base(src.WebResponse, src.Session)
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

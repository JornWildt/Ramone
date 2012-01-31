using System;
using System.Net;


namespace Ramone
{
  public class RamoneResponse
  {
    public HttpWebResponse Response { get; protected set; }

    public string ContentType { get; protected set; }

    public long ContentLength { get { return Response.ContentLength; } }

    public HttpStatusCode StatusCode { get { return Response.StatusCode; } }

    public Uri BaseUri { get { return new Uri(Response.ResponseUri.GetLeftPart(UriPartial.Path)); } }

    public IRamoneSession Session { get; protected set; }


    public RamoneResponse(HttpWebResponse response, IRamoneSession session)
    {
      Response = response;
      ContentType = string.IsNullOrEmpty(Response.ContentType) ? "" : Response.ContentType.Split(';')[0];
      ContentType = ContentType.Trim();
      Session = session;
    }


    public WebHeaderCollection Headers
    {
      get { return Response.Headers; }
    }


    public T Decode<T>() where T : class
    {
      if (Response.ContentLength == 0 || string.IsNullOrEmpty(ContentType) || Response.StatusCode == HttpStatusCode.NoContent)
        return null;

      IMediaTypeReader reader = Session.Service.CodecManager.GetReader(typeof(T), ContentType).Codec;
      ReaderContext context = new ReaderContext(Response.GetResponseStream(), typeof(T), Response, Session);
      T result = reader.ReadFrom(context) as T;
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
      if (Response.StatusCode != HttpStatusCode.Created)
        return null;

      if (Response.Headers[HttpResponseHeader.Location] == null)
        return null;

      return new Uri(Response.Headers[HttpResponseHeader.Location]);
    }


    public T Created<T>() where T : class
    {
      if (Response.StatusCode != HttpStatusCode.Created)
        return null;

      T body = Decode<T>();
      if (body == null)
      {
        if (Response.Headers[HttpResponseHeader.Location] == null)
          return null;

        RamoneRequest request = Session.Request(Response.Headers[HttpResponseHeader.Location]);
        body = request.Get<T>().Body;
      }

      return body;
    }
  }


  public class RamoneResponse<T> : RamoneResponse
    where T : class
  {
    public RamoneResponse(HttpWebResponse response, IRamoneSession session)
      : base(response, session)
    {
    }


    public RamoneResponse(RamoneResponse src)
      : base(src.Response, src.Session)
    {
    }


    private T _body;
    public new T Body
    {
      get 
      {
        if (_body == null)
          _body = Decode<T>();
        return _body; 
      }
    }


    public T Created()
    {
      return base.Created<T>();
    }
  }
}

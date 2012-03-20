using System;
using System.Net;
using Ramone.HyperMedia;


namespace Ramone
{
  public class Resource
  {
    public HttpWebResponse Response { get; protected set; }

    public MediaType ContentType { get; protected set; }

    public long ContentLength { get { return Response.ContentLength; } }

    public HttpStatusCode StatusCode { get { return Response.StatusCode; } }

    public Uri BaseUri { get { return new Uri(Response.ResponseUri.GetLeftPart(UriPartial.Path)); } }

    public IRamoneSession Session { get; protected set; }


    public Resource(HttpWebResponse response, IRamoneSession session)
    {
      Response = response;
      try
      {
        // FIXME: TryParse
        ContentType = new MediaType(Response.ContentType);
      }
      catch (Exception)
      {
        ContentType = null;
      }
      Session = session;
    }


    public WebHeaderCollection Headers
    {
      get { return Response.Headers; }
    }


    public T Decode<T>() where T : class
    {
      if (Response.ContentLength == 0 || ContentType == null || Response.StatusCode == HttpStatusCode.NoContent)
        return null;

      IMediaTypeReader reader = Session.Service.CodecManager.GetReader(typeof(T), ContentType).Codec;
      ReaderContext context = new ReaderContext(Response.GetResponseStream(), typeof(T), Response, Session);
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


  public class Resource<TBody> : Resource
    where TBody : class
  {
    public Resource(HttpWebResponse response, IRamoneSession session)
      : base(response, session)
    {
    }


    public Resource(Resource src)
      : base(src.Response, src.Session)
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

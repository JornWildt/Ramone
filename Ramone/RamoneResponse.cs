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


    public T Decode<T>() where T : class
    {
      if (Response.ContentLength == 0 || string.IsNullOrEmpty(ContentType) || Response.StatusCode == HttpStatusCode.NoContent)
        return null;

      IMediaTypeReader reader = Session.Service.Settings.CodecManager.GetReader(typeof(T), ContentType).Codec;
      T result = reader.ReadFrom(Response.GetResponseStream(), typeof(T)) as T;
      return result;
    }


    public Uri Created()
    {
      if (Response.StatusCode != HttpStatusCode.Created)
        return null;

      if (Response.Headers[HttpResponseHeader.Location] == null)
        return null;

      return new Uri(Response.Headers[HttpResponseHeader.Location]);
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
    public T Body
    {
      get 
      {
        if (_body == null)
          _body = Decode<T>();
        return _body; 
      }
    }
  }
}

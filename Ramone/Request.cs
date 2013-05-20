using System;
using System.Globalization;
using CuttingEdge.Conditions;


namespace Ramone
{
  public class Request : BaseRequest
  {
    #region Constructors

    public Request(ISession session, Uri url)
      : base(session, url)
    {
    }


    public Request(ISession session, string url)
      : base(session, new Uri(url))
    {
    }


    public Request(Request src)
      : base(src)
    {
    }

    #endregion


    #region Setting up

    public Request ContentType(string contentType)
    {
      return ContentType(new MediaType(contentType));
    }

    
    public Request ContentType(MediaType contentType)
    {
      BodyContentType = contentType;
      return this;
    }


    public Request Accept(MediaType accept)
    {
      AcceptHeader = (string)accept;
      return this;
    }


    public RamoneRequest<TAccept> Accept<TAccept>(MediaType accept = null)
      where TAccept : class
    {
      Accept(accept);
      return new RamoneRequest<TAccept>(this);
    }


    public Request Accept(string accept)
    {
      return Accept(new MediaType(accept));
    }


    public RamoneRequest<TAccept> Accept<TAccept>(string accept)
      where TAccept : class
    {
      return Accept<TAccept>(new MediaType(accept));
    }


    public Request AcceptCharset(string charset)
    {
      Header("Accept-Charset", charset);
      return this;
    }


    public Request Charset(string charset)
    {
      BodyCharacterSet = charset;
      return this;
    }


    public Request IfModifiedSince(DateTime t)
    {
      IfModifiedSinceValue = t;
      return this;
    }


    public Request IfUnmodifiedSince(DateTime t)
    {
      Header(HeaderConstants.IfUnmodifiedSince, t.ToUniversalTime().ToString("r"));
      return this;
    }


    public Request IfMatch(string tag)
    {
      Header(HeaderConstants.IfMatch, tag);
      return this;
    }


    public Request IfNoneMatch(string tag)
    {
      Header(HeaderConstants.IfNoneMatch, tag);
      return this;
    }


    public Request Header(string name, string value)
    {
      if (name == HeaderConstants.IfModifiedSince)
        IfModifiedSinceValue = DateTime.ParseExact(value, "r", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
      else
        AdditionalHeaders[name] = value;
      return this;
    }


    public Request Method(string method)
    {
      Condition.Requires(method, "method").IsNotNullOrEmpty();
      SubmitMethod = method;
      return this;
    }


    public Request Body(object body)
    {
      SetBody(body);
      return this;
    }


    public Request CodecParameter(string key, string value)
    {
      Condition.Requires(key, "key").IsNotNull();

      CodecParameters[key] = value;

      return this;
    }


    public string CodecParameter(string key)
    {
      Condition.Requires(key, "key").IsNotNull();

      return CodecParameters[key];
    }

    #endregion


    #region Standard methods

    public virtual Response<TResponse> Get<TResponse>() where TResponse : class
    {
      return DoRequest<TResponse>("GET");
    }


    public virtual Response Get()
    {
      return DoRequest("GET");
    }


    public virtual Response<TResponse> Post<TResponse>() where TResponse : class
    {
      return Post<TResponse>(null);
    }


    public virtual Response<TResponse> Post<TResponse>(object body) where TResponse : class
    {
      Body(body);
      return DoRequest<TResponse>("POST");
    }


    public virtual Response Post(object body)
    {
      Body(body);
      return DoRequest("POST");
    }


    public virtual Response Post()
    {
      return Post(null);
    }


    public virtual Response<TResponse> Put<TResponse>() where TResponse : class
    {
      return Put<TResponse>(null);
    }


    public virtual Response<TResponse> Put<TResponse>(object body) where TResponse : class
    {
      Body(body);
      return DoRequest<TResponse>("PUT");
    }


    public virtual Response Put(object body)
    {
      Body(body);
      return DoRequest("PUT");
    }


    public virtual Response Put()
    {
      return Put(null);
    }


    public virtual Response<TResponse> Delete<TResponse>() where TResponse : class
    {
      return DoRequest<TResponse>("DELETE");
    }


    public virtual Response Delete()
    {
      return DoRequest("DELETE");
    }


    public virtual Response Head()
    {
      return DoRequest("HEAD");
    }


    public virtual Response<TResponse> Options<TResponse>() where TResponse : class
    {
      return DoRequest<TResponse>("OPTIONS");
    }


    public virtual Response Options()
    {
      return DoRequest("OPTIONS");
    }


    public virtual Response<TResponse> Patch<TResponse>() where TResponse : class
    {
      return Patch<TResponse>(null);
    }


    public virtual Response<TResponse> Patch<TResponse>(object body) where TResponse : class
    {
      Body(body);
      return DoRequest<TResponse>("PATCH");
    }


    public virtual Response Patch(object body)
    {
      Body(body);
      return DoRequest("PATCH");
    }


    public virtual Response Patch()
    {
      return Patch(null);
    }

    #endregion


    #region Generic methods

    public virtual Response<TResponse> Execute<TResponse>(string method) where TResponse : class
    {
      return DoRequest<TResponse>(method);
    }


    public virtual Response Execute(string method)
    {
      return DoRequest(method);
    }


    public virtual Response<TResponse> Execute<TResponse>(string method, object body) where TResponse : class
    {
      Body(body);
      return DoRequest<TResponse>(method);
    }


    public virtual Response Execute(string method, object body)
    {
      Body(body);
      return DoRequest(method);
    }


    /// <summary>
    /// Submit request using previously registered method and payload.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public virtual Response<TResponse> Submit<TResponse>() where TResponse : class
    {
      if (SubmitMethod == null)
        throw new InvalidOperationException("Missing method for Submit(). Call Method() first.");
      return DoRequest<TResponse>(SubmitMethod);
    }


    /// <summary>
    /// Submit request using previously registered method.
    /// </summary>
    /// <returns></returns>
    public virtual Response Submit()
    {
      if (SubmitMethod == null)
        throw new InvalidOperationException("Missing method for Submit(). Call Method() first.");
      return DoRequest(SubmitMethod);
    }

    #endregion


    public AsyncRequest Async()
    {
      return new AsyncRequest(this);
    }
  }


  public class RamoneRequest<TResponse> : Request
    where TResponse : class
  {
    public RamoneRequest(Request src)
      : base(src)
    {
    }


    #region Standard methods

    public new Response<TResponse> Get()
    {
      return Get<TResponse>();
    }


    public new Response<TResponse> Post(object body)
    {
      return Post<TResponse>(body);
    }


    public new Response<TResponse> Post()
    {
      return Post<TResponse>();
    }


    public new Response<TResponse> Put(object body)
    {
      return Put<TResponse>(body);
    }


    public new Response<TResponse> Put()
    {
      return Put<TResponse>();
    }


    public new Response<TResponse> Delete()
    {
      return Delete<TResponse>();
    }


    public new Response<TResponse> Options()
    {
      return Options<TResponse>();
    }


    public new Response<TResponse> Patch(object body)
    {
      return Patch<TResponse>(body);
    }


    public new Response<TResponse> Patch()
    {
      return Patch<TResponse>();
    }

    #endregion


    #region Generic methods

    public new Response<TResponse> Execute(string method)
    {
      return Execute<TResponse>(method);
    }


    public new Response<TResponse> Execute(string method, object body)
    {
      return Execute<TResponse>(method, body);
    }

    #endregion
  }
}

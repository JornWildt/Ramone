using System;
using System.Globalization;
using CuttingEdge.Conditions;
using System.Net;
using System.Net.Cache;


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
      Condition.Requires(src, "src").IsNotNull();
      RelatedAsyncRequest = src.RelatedAsyncRequest;
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


    /// <summary>
    /// Set Accept header.
    /// </summary>
    /// <remarks>Can be called multiple times to add more than one media type to the accept header.</remarks>
    /// <param name="accept">Media type to accept.</param>
    /// <param name="q">Quality value.</param>
    /// <returns></returns>
    public Request Accept(MediaType accept, double? q = null)
    {
      if (!string.IsNullOrEmpty(AcceptHeader))
        AcceptHeader += ", ";
      AcceptHeader += (string)accept;
      if (q != null)
        AcceptHeader += string.Format(CultureInfo.InvariantCulture, "; q={0:F2}", q);
      return this;
    }


    /// <summary>
    /// Set Accept header and expected payload type.
    /// </summary>
    /// <remarks>Can be called multiple times to add more than one media type to the accept header.</remarks>
    /// <param name="accept">Media type to accept.</param>
    /// <param name="q">Quality value.</param>
    /// <returns></returns>
    public RamoneRequest<TAccept> Accept<TAccept>(MediaType accept = null, double? q = null)
      where TAccept : class
    {
      Accept(accept, q);
      return new RamoneRequest<TAccept>(this);
    }


    /// <summary>
    /// Set Accept header.
    /// </summary>
    /// <remarks>Can be called multiple times to add more than one media type to the accept header.</remarks>
    /// <param name="accept">Media type (identifier) to accept.</param>
    /// <param name="q">Quality value.</param>
    /// <returns></returns>
    public Request Accept(string accept, double? q = null)
    {
      return Accept(new MediaType(accept), q);
    }


    /// <summary>
    /// Set Accept header and expected payload type.
    /// </summary>
    /// <remarks>Can be called multiple times to add more than one media type to the accept header.</remarks>
    /// <param name="accept">Media type (identifier) to accept.</param>
    /// <param name="q">Quality value.</param>
    /// <returns></returns>
    public RamoneRequest<TAccept> Accept<TAccept>(string accept, double? q = null)
      where TAccept : class
    {
      return Accept<TAccept>(new MediaType(accept), q);
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


    public Request OnHeadersReady(Action<HttpWebRequest> handler)
    {
      Condition.Requires(handler, "handler").IsNotNull();

      OnHeadersReadyHandler = handler;

      return this;
    }


    /// <summary>
    /// Add query parameters to request URL while keeping exiting parameters already specified in the URL.
    /// </summary>
    /// <remarks>This method respects repeated keys, such that adding "x=3&amp;x=4" to "x=1&amp;x=2" yields "x=1&amp;x=2&amp;x=3&amp;x=4".</remarks>
    /// <param name="url"></param>
    /// <param name="parameters">Either IDictionary&lt;string,string&gt;, NameValueCollection or any other
    /// class where the public properties are added as query parameters.</param>
    /// <returns>Same request with Url having parameters added.</returns>
    public Request AddQueryParameters(object parameters)
    {
      if (parameters != null)
      {
        Url = Url.AddQueryParameters(parameters);
      }
      return this;
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


    #region Async handling

    protected AsyncRequest RelatedAsyncRequest { get; set; }


    public AsyncRequest Async()
    {
      return RelatedAsyncRequest = new AsyncRequest(this);
    }


    public void CancelAsync()
    {
      if (RelatedAsyncRequest != null)
        RelatedAsyncRequest.CancelAsync();
    }

    #endregion
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

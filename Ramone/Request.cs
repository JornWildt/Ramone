using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using CuttingEdge.Conditions;


namespace Ramone
{
  public class Request
  {
    public Uri Url { get; protected set; }


    #region Constructors

    public Request(ISession session, Uri url)
    {
      Condition.Requires(session, "session").IsNotNull();
      Condition.Requires(url, "url").IsNotNull();

      Session = session;
      Url = url;
      AdditionalHeaders = new NameValueCollection();
      CodecParameters = new NameValueCollection();
    }


    public Request(ISession session, string url)
      : this(session, new Uri(url))
    {
    }


    public Request(Request src)
    {
      Condition.Requires(src, "src").IsNotNull();

      Session = src.Session;
      Url = src.Url;
      BodyData = src.BodyData;
      BodyCodec = src.BodyCodec;
      BodyContentType = src.BodyContentType;
      AcceptHeader = src.AcceptHeader;
      SubmitMethod = src.SubmitMethod;
      AdditionalHeaders = new NameValueCollection(src.AdditionalHeaders);
      CodecParameters = new NameValueCollection();
    }

    #endregion



    #region Properties

    protected ISession Session { get; set; }

    protected string SubmitMethod { get; set; }

    protected object BodyData { get; set; }

    protected IMediaTypeWriter BodyCodec { get; set; }

    protected MediaType BodyContentType { get; set; }

    protected string BodyCharacterSet { get; set; }

    protected string BodyBoundary { get; set; }

    protected string AcceptHeader { get; set; }

    protected NameValueCollection AdditionalHeaders { get; set; }

    protected NameValueCollection CodecParameters { get; set; }

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


    public Request Header(string name, string value)
    {
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
      ICodecManager codecManager = Session.Service.CodecManager;
      
      if (BodyContentType == null)
        BodyContentType = Session.DefaultRequestMediaType;

      if (body != null)
      {
        MediaTypeWriterRegistration writer = codecManager.GetWriter(body.GetType(), BodyContentType ?? MediaType.Wildcard);
        if (BodyContentType == null)
          BodyContentType = writer.MediaType;
        BodyCodec = writer.Codec;
      }
      else if (BodyContentType == null)
      {
        BodyContentType = MediaType.ApplicationFormUrlEncoded;
      }

      if (BodyContentType.Matches("multipart/form-data"))
      {
        BodyBoundary = Guid.NewGuid().ToString();
      }

      BodyData = body;

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
    /// Submit request using previously registered method.
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


    protected virtual string GetAcceptHeader(Type t)
    {
      if (!string.IsNullOrEmpty(AcceptHeader))
        return AcceptHeader;
      if (Session.DefaultResponseMediaType != null)
        return (string)Session.DefaultResponseMediaType;

      if (t == null)
        return null;

      string accept = "";

      if (t != null)
      {
        IEnumerable<MediaTypeReaderRegistration> readers = Session.Service.CodecManager.GetReaders(t);
        foreach (MediaTypeReaderRegistration r in readers)
        {
          if (accept.Length > 0)
            accept += ", ";
          accept += r.MediaType;
        }
      }

      if (string.IsNullOrEmpty(accept))
        throw new InvalidOperationException(string.Format("Could not find a reader codec for {0}. Try specifying Accept header.", t));

      return accept;
    }


    public AsyncRequest Async()
    {
      return new AsyncRequest(this);
    }


    protected Response<TResponse> DoRequest<TResponse>(string method, int retryLevel = 0) where TResponse : class
    {
      Response r = DoRequest(Url, method, true, req => req.Accept = GetAcceptHeader(typeof(TResponse)), retryLevel);
      return new Response<TResponse>(r, r.RedirectCount);
    }


    protected Response DoRequest(string method, int retryLevel = 0)
    {
      return DoRequest(Url, method, true, req => req.Accept = GetAcceptHeader(null), retryLevel);
    }


    protected virtual Response DoRequest(Uri url, string method, bool includeBody, Action<HttpWebRequest> requestModifier, int retryLevel = 0)
    {
      try
      {
        HttpWebRequest request = SetupRequest(url, method, includeBody, requestModifier);
        WriteBody(null, request, includeBody);
        ApplyDataSentInterceptors(request);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        return HandleResponse(response, method, includeBody, requestModifier, retryLevel);
      }
      catch (WebException ex)
      {
        Response r = HandleWebException(ex, url, method, includeBody, requestModifier, retryLevel);
        if (r == null)
          throw;
        return r;
      }
    }


    protected HttpWebRequest SetupRequest(Uri url, string method, bool includeBody, Action<HttpWebRequest> requestModifier)
    {
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

      // Set headers and similar before writing to stream
      request.Method = method;
      request.CookieContainer = Session.Cookies;
      request.UserAgent = Session.UserAgent;
      request.AllowAutoRedirect = false;

      request.Headers.Add(AdditionalHeaders);

      if (requestModifier != null)
        requestModifier(request);

      if (includeBody && BodyCharacterSet != null && BodyData == null)
        throw new InvalidOperationException("Request character set is not allowed when no body is supplied.");

      if (includeBody && BodyData != null)
      {
        string charset = "";
        if (BodyCharacterSet != null)
          charset = "; charset=" + BodyCharacterSet;

        string boundary = "";
        if (BodyBoundary != null && BodyCodec != null)
        {
          boundary = "; boundary=" + BodyBoundary;
          BodyCodec.CodecArgument = BodyBoundary;
        }

        request.ContentType = BodyContentType + charset + boundary;
      }
      else
      {
        request.ContentLength = 0;
      }

      return request;
    }


    protected virtual void WriteBody(Stream requestStream, HttpWebRequest request, bool includeBody)
    {
      if (BodyData != null && includeBody)
      {
        // Do not call GetRequestStream unless there is any request data
        if (requestStream == null)
          requestStream = request.GetRequestStream();

        ApplyHeadersReadyInterceptors(request);

        foreach (KeyValuePair<string, IRequestInterceptor> interceptor in Session.RequestInterceptors)
          if (interceptor.Value is IRequestStreamWrapper)
            requestStream = ((IRequestStreamWrapper)interceptor.Value).Wrap(new RequestStreamWrapperContext(requestStream, request, Session));

        BodyCodec.WriteTo(new WriterContext(requestStream, BodyData, request, Session, CodecParameters));
        request.GetRequestStream().Close();
      }
      else
      {
        ApplyHeadersReadyInterceptors(request);
      }
    }


    protected void ApplyHeadersReadyInterceptors(HttpWebRequest request)
    {
      foreach (KeyValuePair<string, IRequestInterceptor> interceptor in Session.RequestInterceptors)
      {
        interceptor.Value.HeadersReady(new RequestContext(request, Session));
      }
    }


    protected virtual void ApplyDataSentInterceptors(HttpWebRequest request)
    {
      foreach (KeyValuePair<string, IRequestInterceptor> interceptor in Session.RequestInterceptors)
      {
        interceptor.Value.DataSent(new RequestContext(request, Session));
      }
    }


    protected Response HandleResponse(HttpWebResponse response, string method, bool includeBody, Action<HttpWebRequest> requestModifier, int retryLevel = 0)
    {
      Guid? connectionId = null;
      try
      {
        connectionId = ConnectionStatistics.RegisterConnection(response);

        // Handle redirects
        if (300 <= (int)response.StatusCode && (int)response.StatusCode <= 399)
        {
          int allowedRedirectCount = Session.GetAllowedRedirects((int)response.StatusCode);
          if (retryLevel < allowedRedirectCount)
          {
            if (response.StatusCode == HttpStatusCode.SeeOther)
            {
              method = "GET";
              includeBody = false;
            }
            Uri location = response.LocationAsUri();
            if (location == null)
              throw new InvalidOperationException(string.Format("No redirect location supplied in {0} response from {1}.", (int)response.StatusCode, response.ResponseUri));

            response.Close();
            if (connectionId != null)
              ConnectionStatistics.DiscardConnection(connectionId.Value);

            return DoRequest(location, method, includeBody, requestModifier, retryLevel + 1);
          }
        }

        return new Response(response, Session, retryLevel, connectionId);
      }
      catch (Exception)
      {
        response.Close();
        if (connectionId != null)
          ConnectionStatistics.DiscardConnection(connectionId.Value);
        throw;
      }
    }


    protected Response HandleWebException(WebException ex, Uri url, string method, bool includeBody, Action<HttpWebRequest> requestModifier, int retryLevel)
    {
      HttpWebResponse response = ex.Response as HttpWebResponse;
      if (response != null)
      {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
          // This either throws or returns with success
          HandleUnauthorized(response, ex);

          if (retryLevel == 0)
          {
            // Resend request one time if no exceptions are thrown
            return DoRequest(url, method, includeBody, requestModifier, retryLevel + 1);
          }
          else
            throw new NotAuthorizedException(response, ex);
        }
      }

      return null;
    }


    private void HandleUnauthorized(HttpWebResponse response, WebException ex)
    {
      string authenticationHeader = response.Headers["WWW-Authenticate"];
      if (!string.IsNullOrEmpty(authenticationHeader))
      {
        int pos = authenticationHeader.IndexOf(' ');
        string scheme = authenticationHeader.Substring(0, pos);
        string parameters = authenticationHeader.Substring(pos+1);
        IAuthorizationHandler handler = Session.AuthorizationDispatcher.Get(scheme);
        if (handler != null && handler.HandleAuthorizationRequest(new AuthorizationContext(Session, response, scheme, parameters)))
          return;
      }

      throw new NotAuthorizedException(response, ex);
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

using System;
using System.Collections.Generic;
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
      AdditionalHeaders = new Dictionary<string, string>();
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
      AdditionalHeaders = new Dictionary<string, string>(src.AdditionalHeaders);
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

    protected Dictionary<string, string> AdditionalHeaders { get; set; }

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
      AcceptHeader = (accept != null ? accept.FullType : null);
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

      if (BodyContentType.Matches("multipart/form-data"))
      {
        BodyBoundary = Guid.NewGuid().ToString();
      }

      BodyData = body;

      return this;
    }

    #endregion


    #region Standard methods

    public Response<TResponse> Get<TResponse>(string accept = null) where TResponse : class
    {
      if (accept != null)
        Accept(accept);
      return DoRequest<TResponse>("GET");
    }


    public Response Get(string accept = null)
    {
      if (accept != null)
        Accept(accept);
      return DoRequest("GET");
    }


    public Response<TResponse> Post<TResponse>() where TResponse : class
    {
      return Post<TResponse>(null);
    }


    public Response<TResponse> Post<TResponse>(object body) where TResponse : class
    {
      Body(body);
      return DoRequest<TResponse>("POST");
    }


    public Response Post(object body)
    {
      Body(body);
      return DoRequest("POST");
    }


    public Response Post()
    {
      return Post(null);
    }


    public Response<TResponse> Put<TResponse>() where TResponse : class
    {
      return Put<TResponse>(null);
    }


    public Response<TResponse> Put<TResponse>(object body) where TResponse : class
    {
      Body(body);
      return DoRequest<TResponse>("PUT");
    }


    public Response Put(object body)
    {
      Body(body);
      return DoRequest("PUT");
    }


    public Response Put()
    {
      return Put(null);
    }


    public Response<TResponse> Delete<TResponse>(string accept = null) where TResponse : class
    {
      if (accept != null)
        Accept(accept);
      return DoRequest<TResponse>("DELETE");
    }


    public Response Delete(string accept = null)
    {
      if (accept != null)
        Accept(accept);
      return DoRequest("DELETE");
    }


    public Response Head()
    {
      return DoRequest("HEAD");
    }


    public Response<TResponse> Options<TResponse>(string accept = null) where TResponse : class
    {
      if (accept != null)
        Accept(accept);
      return DoRequest<TResponse>("OPTIONS");
    }


    public Response Options(string accept = null)
    {
      if (accept != null)
        Accept(accept);
      return DoRequest("OPTIONS");
    }

    #endregion


    #region Generic methods

    public Response<TResponse> Execute<TResponse>(string method, string accept = null) where TResponse : class
    {
      if (accept != null)
        Accept(accept);
      return DoRequest<TResponse>(method);
    }


    public Response Execute(string method, string accept = null)
    {
      if (accept != null)
        Accept(accept);
      return DoRequest(method);
    }


    public Response<TResponse> Execute<TResponse>(string method, object body) where TResponse : class
    {
      Body(body);
      return DoRequest<TResponse>(method);
    }


    public Response Execute(string method, object body)
    {
      Body(body);
      return DoRequest(method);
    }


    public Response<TResponse> Submit<TResponse>() where TResponse : class
    {
      if (SubmitMethod == null)
        throw new InvalidOperationException("Missing method for Submit(). Call Method() first.");
      return DoRequest<TResponse>(SubmitMethod);
    }


    public Response Submit()
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
        return Session.DefaultResponseMediaType.FullType;

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


    protected Response<TResponse> DoRequest<TResponse>(string method, int retryLevel = 0) where TResponse : class
    {
      Response r = DoRequest(method, req => req.Accept = GetAcceptHeader(typeof(TResponse)), retryLevel);
      return new Response<TResponse>(r);
    }


    protected Response DoRequest(string method, int retryLevel = 0)
    {
      return DoRequest(method, req => req.Accept = GetAcceptHeader(null), retryLevel);
    }


    protected Response DoRequest(string method, Action<HttpWebRequest> requestModifier, int retryLevel = 0)
    {
      if (retryLevel > 2)
        return null;

      try
      {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

        // Set headers and similar before writing to stream
        request.Method = method;
        request.CookieContainer = Session.Cookies;
        request.UserAgent = Session.UserAgent;

        foreach (KeyValuePair<string, string> h in AdditionalHeaders)
          request.Headers[h.Key] = h.Value;

        if (requestModifier != null)
          requestModifier(request);

        foreach (KeyValuePair<string,IRequestInterceptor> interceptor in Session.RequestInterceptors)
        {
          interceptor.Value.Intercept(new RequestContext(request, Session));
        }

        if (BodyCharacterSet != null && BodyData == null)
          throw new InvalidOperationException("Request character set is not allowed when no body is supplied.");

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

        if (BodyData != null)
        {
          Stream requestStream = request.GetRequestStream();
          foreach (KeyValuePair<string, IRequestInterceptor> interceptor in Session.RequestInterceptors)
            if (interceptor.Value is IRequestStreamWrapper)
              requestStream = ((IRequestStreamWrapper)interceptor.Value).Wrap(new RequestStreamWrapperContext(requestStream, request, Session));

          BodyCodec.WriteTo(new WriterContext(requestStream, BodyData, request, Session));
          request.GetRequestStream().Close();
        }
        else
        {
          request.ContentLength = 0;
        }

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        return new Response(response, Session);
      }
      catch (WebException ex)
      {
        HttpWebResponse response = ex.Response as HttpWebResponse;
        if (response != null)
        {
          if (response.StatusCode == HttpStatusCode.Unauthorized)
          {
            HandleUnauthorized(response, ex);
            if (retryLevel == 0)
            {
              // Resend request one time if no exceptions are thrown
              return DoRequest(method, retryLevel+1);
            }
            else
              throw new NotAuthorizedException(response, ex);
          }
          else
            throw;
        }
        else
        {
          throw;
        }
      }
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

    public new Response<TResponse> Get(string accept = null)
    {
      return Get<TResponse>(accept);
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


    public Response<TResponse> Delete()
    {
      return Delete<TResponse>();
    }


    public new Response<TResponse> Options(string accept = null)
    {
      return Options<TResponse>(accept);
    }

    #endregion


    #region Generic methods

    public new Response<TResponse> Execute(string method, string accept = null)
    {
      return Execute<TResponse>(method, accept);
    }


    public new Response<TResponse> Execute(string method, object body)
    {
      return Execute<TResponse>(method, body);
    }

    #endregion
  }
}

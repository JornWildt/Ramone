using System;
using System.Collections.Generic;
using System.Net;
using CuttingEdge.Conditions;
using System.IO;


namespace Ramone
{
  public class RamoneRequest
  {
    public Uri Url { get; protected set; }


    #region Constructors

    public RamoneRequest(IRamoneSession session, Uri url)
    {
      Condition.Requires(session, "session").IsNotNull();
      Condition.Requires(url, "url").IsNotNull();

      Session = session;
      Url = url;
      AdditionalHeaders = new Dictionary<string, string>();
    }


    public RamoneRequest(IRamoneSession session, string url)
      : this(session, new Uri(url))
    {
    }


    public RamoneRequest(RamoneRequest src)
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

    protected IRamoneSession Session { get; set; }

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

    public RamoneRequest ContentType(string contentType)
    {
      return ContentType(new MediaType(contentType));
    }

    
    public RamoneRequest ContentType(MediaType contentType)
    {
      BodyContentType = contentType;
      return this;
    }


    public RamoneRequest Accept(MediaType accept)
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


    public RamoneRequest Accept(string accept)
    {
      return Accept(new MediaType(accept));
    }


    public RamoneRequest<TAccept> Accept<TAccept>(string accept)
      where TAccept : class
    {
      return Accept<TAccept>(new MediaType(accept));
    }


    public RamoneRequest AcceptCharset(string charset)
    {
      Header("Accept-Charset", charset);
      return this;
    }


    public RamoneRequest Charset(string charset)
    {
      BodyCharacterSet = charset;
      return this;
    }


    public RamoneRequest Header(string name, string value)
    {
      AdditionalHeaders[name] = value;
      return this;
    }


    public RamoneRequest Method(string method)
    {
      Condition.Requires(method, "method").IsNotNullOrEmpty();
      SubmitMethod = method;
      return this;
    }


    public RamoneRequest Body(object body)
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

    public Resource<TResponse> Get<TResponse>(string accept = null) where TResponse : class
    {
      if (accept != null)
        Accept(accept);
      return Request<TResponse>("GET");
    }


    public Resource Get(string accept = null)
    {
      if (accept != null)
        Accept(accept);
      return Request("GET");
    }


    public Resource<TResponse> Post<TResponse>() where TResponse : class
    {
      return Post<TResponse>(null);
    }


    public Resource<TResponse> Post<TResponse>(object body) where TResponse : class
    {
      Body(body);
      return Request<TResponse>("POST");
    }


    public Resource Post(object body)
    {
      Body(body);
      return Request("POST");
    }


    public Resource Post()
    {
      return Post(null);
    }


    public Resource<TResponse> Put<TResponse>() where TResponse : class
    {
      return Put<TResponse>(null);
    }


    public Resource<TResponse> Put<TResponse>(object body) where TResponse : class
    {
      Body(body);
      return Request<TResponse>("PUT");
    }


    public Resource Put(object body)
    {
      Body(body);
      return Request("PUT");
    }


    public Resource Put()
    {
      return Put(null);
    }


    public Resource<TResponse> Delete<TResponse>(string accept = null) where TResponse : class
    {
      if (accept != null)
        Accept(accept);
      return Request<TResponse>("DELETE");
    }


    public Resource Delete(string accept = null)
    {
      if (accept != null)
        Accept(accept);
      return Request("DELETE");
    }


    public Resource Head()
    {
      return Request("HEAD");
    }


    public Resource<TResponse> Options<TResponse>(string accept = null) where TResponse : class
    {
      if (accept != null)
        Accept(accept);
      return Request<TResponse>("OPTIONS");
    }


    public Resource Options(string accept = null)
    {
      if (accept != null)
        Accept(accept);
      return Request("OPTIONS");
    }

    #endregion


    #region Generic methods

    public Resource<TResponse> Execute<TResponse>(string method, string accept = null) where TResponse : class
    {
      if (accept != null)
        Accept(accept);
      return Request<TResponse>(method);
    }


    public Resource Execute(string method, string accept = null)
    {
      if (accept != null)
        Accept(accept);
      return Request(method);
    }


    public Resource<TResponse> Execute<TResponse>(string method, object body) where TResponse : class
    {
      Body(body);
      return Request<TResponse>(method);
    }


    public Resource Execute(string method, object body)
    {
      Body(body);
      return Request(method);
    }


    public Resource<TResponse> Submit<TResponse>() where TResponse : class
    {
      if (SubmitMethod == null)
        throw new InvalidOperationException("Missing method for Submit(). Call Method() first.");
      return Request<TResponse>(SubmitMethod);
    }


    public Resource Submit()
    {
      if (SubmitMethod == null)
        throw new InvalidOperationException("Missing method for Submit(). Call Method() first.");
      return Request(SubmitMethod);
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


    protected Resource<TResponse> Request<TResponse>(string method, int retryLevel = 0) where TResponse : class
    {
      Resource r = Request(method, req => req.Accept = GetAcceptHeader(typeof(TResponse)), retryLevel);
      return new Resource<TResponse>(r);
    }


    protected Resource Request(string method, int retryLevel = 0)
    {
      return Request(method, req => req.Accept = GetAcceptHeader(null), retryLevel);
    }


    protected Resource Request(string method, Action<HttpWebRequest> requestModifier, int retryLevel = 0)
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
        return new Resource(response, Session);
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
              return Request(method, retryLevel+1);
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


  public class RamoneRequest<TResponse> : RamoneRequest
    where TResponse : class
  {
    public RamoneRequest(RamoneRequest src)
      : base(src)
    {
    }


    #region Standard methods

    public new Resource<TResponse> Get(string accept = null)
    {
      return Get<TResponse>(accept);
    }


    public new Resource<TResponse> Post(object body)
    {
      return Post<TResponse>(body);
    }


    public new Resource<TResponse> Post()
    {
      return Post<TResponse>();
    }


    public new Resource<TResponse> Put(object body)
    {
      return Put<TResponse>(body);
    }


    public new Resource<TResponse> Put()
    {
      return Put<TResponse>();
    }


    public Resource<TResponse> Delete()
    {
      return Delete<TResponse>();
    }


    public new Resource<TResponse> Options(string accept = null)
    {
      return Options<TResponse>(accept);
    }

    #endregion


    #region Generic methods

    public new Resource<TResponse> Execute(string method, string accept = null)
    {
      return Execute<TResponse>(method, accept);
    }


    public new Resource<TResponse> Execute(string method, object body)
    {
      return Execute<TResponse>(method, body);
    }

    #endregion
  }
}

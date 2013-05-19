using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using CuttingEdge.Conditions;


namespace Ramone
{
  public class BaseRequest
  {
    public Uri Url { get; protected set; }


    #region Constructors

    protected BaseRequest(ISession session, Uri url)
    {
      Condition.Requires(session, "session").IsNotNull();
      Condition.Requires(url, "url").IsNotNull();

      Session = session;
      Url = url;
      AdditionalHeaders = new NameValueCollection();
      CodecParameters = new NameValueCollection();
    }


    protected BaseRequest(ISession session, string url)
      : this(session, new Uri(url))
    {
    }


    protected BaseRequest(BaseRequest src)
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

    protected DateTime? IfModifiedSinceValue { get; set; }

    protected NameValueCollection CodecParameters { get; set; }

    #endregion


    protected void SetBody(object body)
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
    }


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


    protected virtual Response<TResponse> DoRequest<TResponse>(string method, int retryLevel = 0) where TResponse : class
    {
      Response r = DoRequest(Url, method, true, req => req.Accept = GetAcceptHeader(typeof(TResponse)), retryLevel);
      return new Response<TResponse>(r, r.RedirectCount);
    }


    protected virtual Response DoRequest(string method, int retryLevel = 0)
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
        HandleWebExceptionResult result = HandleWebException(ex, url, method, includeBody, requestModifier, retryLevel);
        if (!result.Retried)
          throw;
        return result.Response;
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
      if (IfModifiedSinceValue != null)
        request.IfModifiedSince = IfModifiedSinceValue.Value;

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
        ApplyRequestStreamWrappers(requestStream, request);

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


    protected Stream ApplyRequestStreamWrappers(Stream requestStream, HttpWebRequest request)
    {
      foreach (KeyValuePair<string, IRequestInterceptor> interceptor in Session.RequestInterceptors)
        if (interceptor.Value is IRequestStreamWrapper)
          requestStream = ((IRequestStreamWrapper)interceptor.Value).Wrap(new RequestStreamWrapperContext(requestStream, request, Session));

      return requestStream;
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
            bool allowAutomaticRedirect = false;
            if (response.StatusCode == HttpStatusCode.SeeOther)
            {
              method = "GET";
              includeBody = false;
              allowAutomaticRedirect = true;
            }
            else if (method.Equals("GET", StringComparison.InvariantCultureIgnoreCase)
                     || method.Equals("HEAD", StringComparison.InvariantCultureIgnoreCase))
            {
              allowAutomaticRedirect = true;
            }

            if (allowAutomaticRedirect)
            {
              Uri location = response.LocationAsUri();
              if (location == null)
                throw new InvalidOperationException(string.Format("No redirect location supplied in {0} response from {1}.", (int)response.StatusCode, response.ResponseUri));

              response.Close();
              if (connectionId != null)
                ConnectionStatistics.DiscardConnection(connectionId.Value);

              return DoRequest(location, method, includeBody, requestModifier, retryLevel + 1);
            }
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


    protected class HandleWebExceptionResult
    {
      public bool Retried { get; set; }
      public Response Response { get; set; }
    }


    protected HandleWebExceptionResult HandleWebException(WebException ex, Uri url, string method, bool includeBody, Action<HttpWebRequest> requestModifier, int retryLevel)
    {
      HttpWebResponse response = ex.Response as HttpWebResponse;
      if (response != null)
      {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
          if (!HandleUnauthorized(response, ex))
            return new HandleWebExceptionResult();

          if (retryLevel == 0)
          {
            // Resend request one time if no exceptions are thrown
            return new HandleWebExceptionResult
            {
              Response = DoRequest(url, method, includeBody, requestModifier, retryLevel + 1),
              Retried = true
            };
          }
          else
            return new HandleWebExceptionResult();
        }
      }

      return new HandleWebExceptionResult();
    }


    private bool HandleUnauthorized(HttpWebResponse response, WebException ex)
    {
      string authenticationHeader = response.Headers["WWW-Authenticate"];
      if (!string.IsNullOrEmpty(authenticationHeader))
      {
        int pos = authenticationHeader.IndexOf(' ');
        string scheme = authenticationHeader.Substring(0, pos);
        string parameters = authenticationHeader.Substring(pos + 1);
        IAuthorizationHandler handler = Session.AuthorizationDispatcher.Get(scheme);
        if (handler != null && handler.HandleAuthorizationRequest(new AuthorizationContext(Session, response, scheme, parameters)))
          return true;
      }

      return false;
    }
  }
}

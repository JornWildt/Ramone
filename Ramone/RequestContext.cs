using System.Net;


namespace Ramone
{
  public class RequestContext
  {
    public HttpWebRequest Request { get; private set; }

    public IRamoneSession Session { get; private set; }


    public RequestContext(HttpWebRequest request, IRamoneSession session)
    {
      Request = request;
      Session = session;
    }
  }
}

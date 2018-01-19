using System.Net;

namespace Ramone
{
  public class RequestContext
  {
    public HttpWebRequest Request { get; private set; }

    public ISession Session { get; private set; }


    public RequestContext(HttpWebRequest request, ISession session)
    {
      Request = request;
      Session = session;
    }
  }
}

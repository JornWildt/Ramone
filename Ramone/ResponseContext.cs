using System.Net;

namespace Ramone
{
  public class ResponseContext
  {
    public HttpWebResponse Response { get; private set; }

    public ISession Session { get; private set; }


    public ResponseContext(HttpWebResponse response, ISession session)
    {
      Response = response;
      Session = session;
    }
  }
}

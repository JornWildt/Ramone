using System.IO;
using System.Net;


namespace Ramone
{
  public class RequestStreamWrapperContext
  {
    public Stream RequestStream { get; private set; }

    public HttpWebRequest Request { get; private set; }
    
    public ISession Session { get; private set; }
    
    
    public RequestStreamWrapperContext(Stream requestStream, HttpWebRequest request, ISession session)
    {
      RequestStream = requestStream;
      Request = request;
      Session = session;
    }
  }
}

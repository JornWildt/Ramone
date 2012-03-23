using System.IO;
using System.Net;


namespace Ramone
{
  public class WriterContext : MediaTypeContext
  {
    public object Data { get; set; }

    public HttpWebRequest Request { get; protected set; }

    public WriterContext(Stream s, object data, HttpWebRequest request, ISession session)
    {
      HttpStream = s;
      Data = data;
      Request = request;
      Session = session;
    }
  }


  public interface IMediaTypeWriter : IMediaTypeCodec
  {
    void WriteTo(WriterContext context);
  }
}

using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Ramone
{
  public class WriterContext : MediaTypeContext
  {
    public object Data { get; protected set; }

    public HttpWebRequest Request { get; protected set; }

    public NameValueCollection CodecParameters { get; protected set; }

    public WriterContext(Stream s, object data, HttpWebRequest request, ISession session, NameValueCollection codecParameters)
    {
      HttpStream = s;
      Data = data;
      Request = request;
      Session = session;
      CodecParameters = codecParameters;
    }
  }


  public interface IMediaTypeWriter : IMediaTypeCodec
  {
    void WriteTo(WriterContext context);
  }


  public interface IMediaTypeWriterAsync : IMediaTypeCodec
  {
    Task WriteToAsync(WriterContext context);
  }
}

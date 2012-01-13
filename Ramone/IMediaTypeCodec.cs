using System.IO;
using System.Net;


namespace Ramone
{
  public class MediaTypeContext
  {
    public Stream HttpStream { get; protected set; }
  }


  public interface IMediaTypeCodec
  {
    object CodecArgument { get; set; }
  }
}

using System.IO;


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

using OpenRasta.Codecs;
using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Codecs
{
  [MediaType("application/xml")]
  public class HeaderEchoCodec : XmlSerializerCodec<HeaderList>
  {
  }
}
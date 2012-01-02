using Ramone.Tests.Common;
using OpenRasta.Codecs;


namespace Ramone.Tests.Server.Codecs
{
  [MediaType("application/xml")]
  public class CatAsXmlCodec : XmlSerializerCodec<Cat>
  {
  }
}
using Ramone.Tests.Common;
using OpenRasta.Codecs;


namespace Ramone.Tests.Server.Codecs
{
  [MediaType("application/vnd.dog+xml")]
  public class Dog2AsXmlCodec : XmlSerializerCodec<Dog2>
  {
  }
}
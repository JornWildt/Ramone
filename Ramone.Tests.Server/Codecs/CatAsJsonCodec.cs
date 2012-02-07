using OpenRasta.Codecs;
using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Codecs
{
  [MediaType("application/json")]
  public class CatAsJsonCodec : JsonSerializerCodec<Cat>
  {
  }
}
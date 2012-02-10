using Ramone.Tests.Common;
using OpenRasta.Web;
using System.IO;


namespace Ramone.Tests.Server.Handlers
{
  public class AnyEchoHandler
  {
    public object Post()
    {
      return null;
    }


    public object Post(Stream s)
    {
      return new Ramone.Tests.Server.Configuration.AnyEcho { S = s };
    }
  }
}
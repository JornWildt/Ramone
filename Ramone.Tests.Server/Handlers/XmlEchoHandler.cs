using Ramone.Tests.Common;
using OpenRasta.Web;
using System.IO;
using System.Xml;


namespace Ramone.Tests.Server.Handlers
{
  public class XmlEchoHandler
  {
    public object Post(Stream s)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(s);
      return new Ramone.Tests.Server.Configuration.XmlEcho { Doc = doc };
    }
  }
}
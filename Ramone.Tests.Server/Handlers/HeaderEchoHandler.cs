using Ramone.Tests.Common;
using System.Web;
using System.Collections.Specialized;


namespace Ramone.Tests.Server.Handlers
{
  public class HeaderEchoHandler
  {
    public object Get()
    {
      HeaderList list = new HeaderList();
      NameValueCollection headers = HttpContext.Current.Request.Headers;

      foreach (string header in headers.AllKeys)
      {
        list.Add(string.Format("{0}: {1}", header, headers[header]));
      }

      return list;
    }
  }
}
using System.Collections.Specialized;
using System.Web;
using OpenRasta.Web;
using Ramone.MediaTypes.JsonPatch;
using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Handlers
{
  public class HeaderEchoHandler
  {
    public ICommunicationContext CommunicationContext { get; set; }


    public object Get()
    {
      HeaderList list = new HeaderList();
      NameValueCollection headers = HttpContext.Current.Request.Headers;

      foreach (string header in headers.AllKeys)
      {
        list.Add(string.Format("{0}: {1}", header, headers[header]));
      }

      list.Add("Method: " + CommunicationContext.Request.HttpMethod);
      return list;
    }


    public object Post()
    {
      return Get();
    }


    public object Patch()
    {
      return Get();
    }


    public object Patch(JsonPatchDocument patch)
    {
      return Get();
    }
  }
}
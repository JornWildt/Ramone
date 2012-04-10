using System;
using OpenRasta.Web;
using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Handlers
{
  public class RedirectResult : OperationResult
  {
  }


  public class RedirectHandler
  {
    public object Get(int code, int count)
    {
      return Process(code, count, "GET");
    }


    public object Post(int code, int count)
    {
      return Process(code, count, "POST");
    }


    private object Process(int code, int count, string method)
    {
      if (count == 5)
      {
        return new RedirectArgs { Count = count, Method = method };
      }

      Uri redirectUrl = typeof(RedirectArgs).CreateUri(new { code = code, count = count + 1 });

      return new RedirectResult
      {
        ResponseResource = new RedirectArgs { Count = count },
        StatusCode = code,
        RedirectLocation = redirectUrl
      };
    }
  }
}
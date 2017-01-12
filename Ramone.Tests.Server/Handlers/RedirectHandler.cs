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
    public object Get(int code, int count, int v)
    {
      return Process(code, count, "GET", v);
    }


    public object Post(int code, int count, int? v)
    {
      return Process(code, count, "POST", (int)v);
    }


    public object Put(int code, int count, int v)
    {
      return Process(code, count, "PUT", v);
    }


    public object Head(int code, int count, int v)
    {
      return Process(code, count, "HEAD", v);
    }


    private object Process(int code, int count, string method, int voidResponse)
    {
      if (count == -1)
      {
        return null;
      }

      if (count == 5)
      {
        return new RedirectArgs { Count = count, Method = method };
      }

      Uri redirectUrl = typeof(RedirectArgs).CreateUri(new { code = code, count = count + 1, v = voidResponse });

      return new RedirectResult
      {
        ResponseResource = (voidResponse!=0 ? null : new RedirectArgs { Count = count }),
        StatusCode = code,
        RedirectLocation = redirectUrl
      };
    }
  }
}
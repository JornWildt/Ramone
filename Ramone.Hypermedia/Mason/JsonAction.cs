using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ramone;


namespace Ramone.Hypermedia.Mason
{
  public class JsonAction : ActionBase
  {
    public JsonAction(string name, string href, string method)
      : base(name, href, method)
    {
    }


    public override Request Bind(ISession session)
    {
      return session.Bind(HRef).Method(Method).AsJson();
    }


    public override Request Bind(ISession session, object args)
    {
      return session.Bind(HRef).Method(Method).AsJson().Body(args);
    }


    public override Response Invoke(ISession session)
    {
      throw new InvalidOperationException("A json action must have arguments applied");
    }


    public override Response Invoke(ISession session, object args)
    {
      return session.Bind(HRef).Method(Method).AsJson().Body(args).Submit();
    }

    
    public override Response<T> Invoke<T>(ISession session)
    {
      throw new InvalidOperationException("A json action must have arguments applied");
    }


    public override Response<T> Invoke<T>(ISession session, object args)
    {
      return session.Bind(HRef).Method(Method).AsJson().Body(args).Submit<T>();
    }
  }
}

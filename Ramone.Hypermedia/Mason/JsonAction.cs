using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ramone;


namespace Ramone.Hypermedia.Mason
{
  public class JsonAction : ControlBase
  {
    public string HRef { get; set; }

    public string Method { get; set; }


    public JsonAction(string name, string href, string method)
      : base(name)
    {
      HRef = href;
      Method = method;
    }


    public override Request Bind(ISession session)
    {
      return session.Bind(HRef).Method(Method);
    }


    public override Request Bind(ISession session, object args)
    {
      return session.Bind(HRef).Method(Method).Body(args);
    }


    public override Response Invoke(ISession session)
    {
      return session.Bind(HRef).Method(Method).Submit();
    }


    public override Response Invoke(ISession session, object args)
    {
      return session.Bind(HRef).Method(Method).Body(args).Submit();
    }

    
    public override Response<T> Invoke<T>(ISession session)
    {
      return session.Bind(HRef).Method(Method).Submit<T>();
    }


    public override Response<T> Invoke<T>(ISession session, object args)
    {
      return session.Bind(HRef).Method(Method).Body(args).Submit<T>();
    }
  }
}

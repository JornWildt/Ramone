using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Ramone.Hypermedia.Mason
{
  public class VoidAction : ActionBase
  {
    public VoidAction(string name, string href, string method)
      : base(name, href, method)
    {
    }

    
    public override Request Bind(ISession session)
    {
      return session.Bind(HRef).Method(Method);
    }

    
    public override Request Bind(ISession session, object args)
    {
      throw new InvalidOperationException("A void action does not accept any arguments");
    }

    
    public override Response Invoke(ISession session)
    {
      return session.Bind(HRef).Method(Method).Submit();
    }

    
    public override Response Invoke(ISession session, object args)
    {
      throw new InvalidOperationException("A link does not accept any arguments");
    }

    
    public override Response<T> Invoke<T>(ISession session)
    {
      return session.Bind(HRef).Method(Method).Submit<T>();
    }

    
    public override Response<T> Invoke<T>(ISession session, object args)
    {
      throw new InvalidOperationException("A link does not accept any arguments");
    }
  }
}

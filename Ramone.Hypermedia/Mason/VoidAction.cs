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
      throw new NotImplementedException();
    }

    
    public override Request Bind(ISession session, object args)
    {
      throw new NotImplementedException();
    }

    
    public override Response Invoke(ISession session)
    {
      throw new NotImplementedException();
    }

    
    public override Response Invoke(ISession session, object args)
    {
      throw new NotImplementedException();
    }

    
    public override Response<T> Invoke<T>(ISession session)
    {
      throw new NotImplementedException();
    }

    
    public override Response<T> Invoke<T>(ISession session, object args)
    {
      throw new NotImplementedException();
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ramone;


namespace Ramone.Hypermedia.Mason
{
  public class Link : ControlBase
  {
    public string HRef { get; set; }


    public Link(string name, string href)
      : base(name)
    {
      HRef = href;
    }


    public override Request Bind(ISession session)
    {
      return session.Bind(HRef).Method("GET");
    }


    public override Request Bind(ISession session, object args)
    {
      throw new InvalidOperationException("A link does not accept any arguments");
    }


    public override Response Invoke(ISession session)
    {
      return session.Request(HRef).Get();
    }


    public override Response Invoke(ISession session, object args)
    {
      throw new InvalidOperationException("A link does not accept any arguments");
    }

    
    public override Response<T> Invoke<T>(ISession session)
    {
      return session.Request(HRef).Get<T>();
    }

    
    public override Response<T> Invoke<T>(ISession session, object args)
    {
      throw new InvalidOperationException("A link does not accept any arguments");
    }
  }
}

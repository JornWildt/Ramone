using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ramone.Hypermedia.Mason
{
  public class LinkTemplate : ControlBase
  {
    public string Template { get; set; }


    public LinkTemplate(string name, string template)
      : base(name)
    {
      Template = template;
    }


    public override Request Bind(ISession session)
    {
      return session.Bind(Template).Method("GET");
    }

    
    public override Request Bind(ISession session, object args)
    {
      return session.Bind(Template, args).Method("GET");
    }

    
    public override Response Invoke(ISession session)
    {
      return session.Bind(Template).Get();
    }

    
    public override Response Invoke(ISession session, object args)
    {
      return session.Bind(Template, args).Get();
    }

    
    public override Response<T> Invoke<T>(ISession session)
    {
      return session.Bind(Template).Get<T>();
    }

    
    public override Response<T> Invoke<T>(ISession session, object args)
    {
      return session.Bind(Template, args).Get<T>();
    }
  }
}

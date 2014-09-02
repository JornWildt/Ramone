using Ramone;
using System;


namespace Ramone.Hypermedia
{
  public abstract class ControlBase : IControl
  {
    public ControlBase(string name)
    {
      Name = name;
    }


    #region IControl Members

    public string Name { get; protected set; }

    
    public abstract Request Bind(ISession session);

    public abstract Request Bind(ISession session, object args);


    public abstract Response Invoke(ISession session);

    public abstract Response Invoke(ISession session, object args);


    public abstract Response<T> Invoke<T>(ISession session) where T : class;

    public abstract Response<T> Invoke<T>(ISession session, object args) where T : class;


    #region Upload

    public virtual Request Bind(ISession session, object args, object files)
    {
      throw new InvalidOperationException(string.Format("Bind(session,args,files) is not possible for {0}", this.GetType()));
    }


    public virtual Response Upload(ISession session, object args, object files)
    {
      throw new InvalidOperationException(string.Format("Upload(session,args,files) is not possible for {0}", this.GetType()));
    }


    public virtual Response<T> Upload<T>(ISession session, object args, object files) where T : class
    {
      throw new InvalidOperationException(string.Format("Upload<T>(session,args,files) is not possible for {0}", this.GetType()));
    }

    #endregion

    #endregion
  }
}

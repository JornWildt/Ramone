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

    public string Name { get; set; }

    
    public abstract Request Bind(ISession session);

    public abstract Request Bind(ISession session, object args);


    public abstract Response Invoke(ISession session);

    public abstract Response Invoke(ISession session, object args);


    public abstract Response<T> Invoke<T>(ISession session) where T : class;

    public abstract Response<T> Invoke<T>(ISession session, object args) where T : class;

    #endregion
  }
}

using Ramone;
using System;


namespace Ramone.Hypermedia
{
  public interface IControl
  {
    string Name { get; set; }

    Request Bind(ISession session);
    Request Bind(ISession session, object args);

    Response Invoke(ISession session);
    Response Invoke(ISession session, object args);

    Response<T> Invoke<T>(ISession session) where T : class;
    Response<T> Invoke<T>(ISession session, object args) where T : class;
  }
}

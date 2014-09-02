using Ramone;
using System;


namespace Ramone.Hypermedia
{
  public interface IControl
  {
    string Name { get; }

    Request Bind(ISession session);
    Request Bind(ISession session, object args);
    Request Bind(ISession session, object args, object files);

    Response Invoke(ISession session);
    Response Invoke(ISession session, object args);
    Response Upload(ISession session, object args, object files);

    Response<T> Invoke<T>(ISession session) where T : class;
    Response<T> Invoke<T>(ISession session, object args) where T : class;
    Response<T> Upload<T>(ISession session, object args, object files) where T : class;
  }
}

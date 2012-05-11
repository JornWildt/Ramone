using System;


namespace Ramone
{
  public interface IHaveContext
  {
    void RegisterContext(ISession session, Uri baseUrl);
  }
}

namespace Ramone
{
  public interface IAuthorizationDispatcher
  {
    void Add(string authorizationScheme, IAuthorizationHandler handler);
    void Remove(string authorizationScheme);
    IAuthorizationHandler Get(string authorizationScheme);
    IAuthorizationDispatcher Clone();
  }
}

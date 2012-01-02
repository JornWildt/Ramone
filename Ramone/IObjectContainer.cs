namespace Ramone
{
  public interface IObjectContainer
  {
    IObjectContainer AddComponent<I, T>() where T : class;
    T Resolve<T>();
  }
}

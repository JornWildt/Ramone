namespace Ramone
{
  public interface IHaveRequestInterceptors
  {
    IRequestInterceptorSet RequestInterceptors { get; }
  }
}

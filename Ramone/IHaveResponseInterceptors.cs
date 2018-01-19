namespace Ramone
{
  public interface IHaveResponseInterceptors
  {
    IResponseInterceptorSet ResponseInterceptors { get; }
  }
}

using System.IO;


namespace Ramone
{
  public interface IRequestStreamWrapper
  {
    Stream Wrap(RequestStreamWrapperContext context);
  }
}

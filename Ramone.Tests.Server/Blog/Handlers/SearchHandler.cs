using Ramone.Tests.Server.Blog.Resources;


namespace Ramone.Tests.Server.Blog.Handlers
{
  public class SearchHandler
  {
    public object Get()
    {
      return new Search();
    }
  }
}
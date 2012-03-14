using Ramone.Tests.Server.Blog.Resources;


namespace Ramone.Tests.Server.Blog.Handlers
{
  public class SearchDescriptionHandler
  {
    public object Get()
    {
      return new SearchDescription
      {
        Template = ""
      };
    }
  }
}
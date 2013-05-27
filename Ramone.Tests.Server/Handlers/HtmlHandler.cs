namespace Ramone.Tests.Server.Handlers
{
  public class HtmlPageResource { }

  public class HtmlHandler
  {
    public object Get()
    {
      return new HtmlPageResource();
    }
  }
}
namespace Ramone.Tests.Server.Handlers
{
  public class LinkHeaderHandler
  {
    public object Get()
    {
      return new Ramone.Tests.Server.Configuration.LinkHeader();
    }
  }
}
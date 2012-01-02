using OpenRasta.Web;


namespace Ramone.Tests.Server.Handlers
{
  public class TextNullHandler
  {
    public object Post(string data)
    {
      return new OperationResult.NoContent();
    }
  }
}

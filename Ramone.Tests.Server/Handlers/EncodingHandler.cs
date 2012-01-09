using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Handlers
{
  public class EncodingHandler
  {
    public object Get()
    {
      return string.Format("<html><body>{0}</body></html>", "ÆØÅúï");
    }
  }
}
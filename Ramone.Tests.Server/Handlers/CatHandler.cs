using Ramone.Tests.Common;
using OpenRasta.Web;


namespace Ramone.Tests.Server.Handlers
{
  public class CatHandler
  {
    public Cat Get(string name)
    {
      return new Cat { Name = name };
    }


    public OperationResult Post(Cat c)
    {
      return new OperationResult.Created
      {
        ResponseResource = c,
        RedirectLocation = typeof(Cat).CreateUri(new { name = c.Name })
      };
    }
  }
}
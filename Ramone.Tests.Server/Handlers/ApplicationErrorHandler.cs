using OpenRasta.Web;
using Ramone.Tests.Common;

namespace Ramone.Tests.Server.Handlers
{
  public class ApplicationErrorHandler
  {
    public object Get(int code)
    {
      return new OperationResult.BadRequest
      {
        ResponseResource = new ApplicationError
        {
          Message = "Error X",
          Code = code
        }
      };
    }
  }
}
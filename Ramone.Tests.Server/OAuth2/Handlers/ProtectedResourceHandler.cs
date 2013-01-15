using OpenRasta.Web;
using Ramone.Tests.Common.OAuth2;


namespace Ramone.Tests.Server.OAuth2.Handlers
{
  public class ProtectedResourceHandler
  {
    public ICommunicationContext CommunicationContext { get; set; }


    public object Get()
    {
      if (CommunicationContext.Request.Headers.ContainsKey("Authorization"))
      {
        string auth = CommunicationContext.Request.Headers["Authorization"];
        if (auth == "BEARER " + OAuth2TestConstants.CreatedAccessToken)
        {
          return new ProtectedResource { Title = "Got it" };
        }
      }

      return new OperationResult.Unauthorized();
    }
  }
}
using System;
using System.Text;
using OpenRasta.Web;
using Ramone.Tests.Common.OAuth2;


namespace Ramone.Tests.Server.OAuth2.Handlers
{
  public class OAuth2TokenEndpointHandler
  {
    public ICommunicationContext CommunicationContext { get; set; }


    
    public object Post(OAuth2AccessTokenRequest request)
    {
      object result = VerifyBasicAuth();
      if (result != null)
        return result;

      if (request.grant_type == "password")
      {
        if (request.username == OAuth2TestConstants.Username && request.password == OAuth2TestConstants.UserPassword)
        {
          return new OAuth2AccessTokenResponse
          {
            access_token = OAuth2TestConstants.CreatedAccessToken,
            token_type = "beAReR", // Mixed case => assert testing for this is case-insensitive
            additional_param = "Special"
          };
        }
        else
          return new OperationResult.BadRequest { ResponseResource = new OAuth2Error { error = "invalid_grant" } };
      }
      else
        return new OperationResult.BadRequest { ResponseResource = new OAuth2Error { error = "unsupported_grant_type" } };
    }


    protected object VerifyBasicAuth()
    {
      if (CommunicationContext.Request.Headers.ContainsKey("Authorization"))
      {
        string authorization = CommunicationContext.Request.Headers["Authorization"];
        if (authorization.StartsWith("BASIC ", StringComparison.InvariantCultureIgnoreCase))
        {
          string token = authorization.Substring(6).Trim();
          token = Encoding.ASCII.GetString(Convert.FromBase64String(token));
          string[] loginAndPassword = token.Split(':');
          if (loginAndPassword.Length == 2)
          {
            string login = loginAndPassword[0];
            string password = loginAndPassword[1];

            if (login == OAuth2TestConstants.ClientID && password == OAuth2TestConstants.ClientPassword)
              return null;
          }
        }
      }

      return new OperationResult.Unauthorized { ResponseResource = new OAuth2Error { error = "unauthorized_client" } };
    }
  }
}
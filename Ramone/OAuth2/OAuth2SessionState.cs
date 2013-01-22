using System;


namespace Ramone.OAuth2
{
  [Serializable]
  public class OAuth2SessionState
  {
    public string AuthorizationState { get; set; }
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
  }
}
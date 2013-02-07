namespace Ramone.Utility.JsonWebToken
{
  public static class Algorithms
  {
    public const string SHA256 = "SH256";
    public const string RSASHA256 = "RS256";
  }


  public class AssertionArgs
  {
    /// <summary>
    /// Which signature algorithm to use (you can select from the predefined constains in "Algorithms").
    /// </summary>
    public string Algorithm { get; set; }

    /// <summary>
    /// The unique identifier for the entity that issued the assertion.
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// A URI that identifies the party intended to process the assertion (The audience should be the URL of the Token Endpoint).
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// Space separated list of strings identifying the required scopes (as defined by the authorization server)
    /// </summary>
    public string Scope { get; set; }
  }
}

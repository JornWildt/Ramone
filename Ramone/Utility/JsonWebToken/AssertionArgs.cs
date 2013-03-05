using System;


namespace Ramone.Utility.JsonWebToken
{
  public static class Algorithms
  {
    public const string SHA256 = "HS256";
    public const string RSASHA256 = "RS256";
  }


  public class AssertionArgs
  {
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

    /// <summary>
    /// Life time of created token (used to calculate expire time).
    /// </summary>
    public TimeSpan ExpireTime { get; set; }

    /// <summary>
    /// Offset time span for issued-at time. Default is -10 seconds to allow for some time skew.
    /// </summary>
    public TimeSpan IssueTimeOffset { get; set; }

    
    public AssertionArgs()
    {
      ExpireTime = TimeSpan.FromSeconds(3600);
      IssueTimeOffset = TimeSpan.FromSeconds(-10);
    }
  }
}

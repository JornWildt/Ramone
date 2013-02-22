namespace Ramone.Utility.JsonWebToken
{
  /// <summary>
  /// Signature algorithm for Json Web Token signing
  /// </summary>
  public interface ISigningAlgorithm
  {
    /// <summary>
    /// Name of algorithm used for signing (as defined in "JSON Web Signature (JWS)" http://tools.ietf.org/html/draft-ietf-jose-json-web-signature)
    /// </summary>
    string AlgorithmName { get; }
    string Sign(string data);
  }
}

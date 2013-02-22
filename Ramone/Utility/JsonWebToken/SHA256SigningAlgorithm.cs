namespace Ramone.Utility.JsonWebToken
{
  public class SHA256SigningAlgorithm : ISigningAlgorithm
  {
    #region ISigningAlgorithm Members

    public string AlgorithmName { get { return Algorithms.SHA256; } }

    public string Sign(string data)
    {
      return JsonWebTokenUtility.HMAC_ASCII_SHA256_Base64Url(data, SHAKey);
    }

    #endregion


    protected byte[] SHAKey { get; set; }


    public SHA256SigningAlgorithm(byte[] shaKey)
    {
      SHAKey = shaKey;
    }
  }
}

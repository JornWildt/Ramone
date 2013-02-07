using System.Security.Cryptography;


namespace Ramone.Utility.JsonWebToken
{
  public class RSASHA256SigningAlgorithm : ISigningAlgorithm
  {
    #region ISigningAlgorithm Members

    public string Sign(string data)
    {
      return JsonWebTokenUtility.HMAC_ASCII_RSASHA256_Base64Url(data, CryptoProvider);
    }

    #endregion


    protected RSACryptoServiceProvider CryptoProvider { get; set; }


    public RSASHA256SigningAlgorithm(RSACryptoServiceProvider cp)
    {
      CryptoProvider = cp;
    }
  }
}

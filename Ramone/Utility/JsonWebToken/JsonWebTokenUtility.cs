using System.Security.Cryptography;


namespace Ramone.Utility.JsonWebToken
{
  public static class JsonWebTokenUtility
  {
    public static string HMAC_ASCII_SHA256_Base64Url(string input, byte[] key)
    {
      HMACSHA256 hmacsha256 = new HMACSHA256(key);
      byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
      byte[] hashBytes = hmacsha256.ComputeHash(inputBytes);
      return Base64Utility.UrlEncode(hashBytes);
    }


    public static string HMAC_ASCII_RSASHA256_Base64Url(string input, RSACryptoServiceProvider rsa)
    {
      byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
      byte[] signature = rsa.SignData(inputBytes, "SHA256");
      return Base64Utility.UrlEncode(signature);
    }


    public static string CreateJsonWebToken_SHA256(string jsonHeader, string jsonPayload, byte[] shaKey)
    {
      return CreateJsonWebToken(jsonPayload, new SHA256SigningAlgorithm(shaKey));
    }


    public static string CreateJsonWebToken_RSASHA256(string jsonPayload, RSACryptoServiceProvider cp)
    {
      return CreateJsonWebToken(jsonPayload, new RSASHA256SigningAlgorithm(cp));
    }


    public static string CreateJsonWebToken(string jsonPayload, ISigningAlgorithm sign)
    {
      string jsonHeader = string.Format(@"{{""alg"":""{0}"",""typ"":""JWT""}}", sign.AlgorithmName);
      string claim = Base64Utility.UTF8UrlEncode(jsonHeader) + "." + Base64Utility.UTF8UrlEncode(jsonPayload);
      string signature = sign.Sign(claim);
      return claim + "." + signature;
    }
  }
}

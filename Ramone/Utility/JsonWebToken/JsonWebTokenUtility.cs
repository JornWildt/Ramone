using System.Security.Cryptography;


namespace Ramone.Utility.JsonWebToken
{
  public static class JsonWebTokenUtility
  {
    public static string HMAC_ASCII_SHA256_Base64Url(string input, byte[] key)
    {
      HMACSHA256 hmacsha1 = new HMACSHA256(key);
      byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
      byte[] hashBytes = hmacsha1.ComputeHash(inputBytes);
      return Base64Utility.UrlEncode(hashBytes);
    }


    public static string HMAC_ASCII_RSASHA256_Base64Url(string input, RSACryptoServiceProvider rsa)
    {
      byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
      byte[] signature = rsa.SignData(inputBytes, "SHA256");
      return Base64Utility.UrlEncode(signature);
    }


    public static string JWT_SHA256(string jsonPayload, byte[] shaKey)
    {
      string jsonHeader = @"{""typ"":""JWT"",""alg"":""HS256""}";
      return JWT_SHA256(jsonHeader, jsonPayload, shaKey);
    }


    public static string JWT_SHA256(string jsonHeader, string jsonPayload, byte[] shaKey)
    {
      return CreateJsonWebToken(jsonHeader, jsonPayload, new SHA256SigningAlgorithm(shaKey));
    }


    public static string JWT_RSASHA1(string jsonPayload, RSACryptoServiceProvider cp)
    {
      string jsonHeader = @"{""alg"":""RS256"",""typ"":""JWT""}";
      return JWT_RSASHA256(jsonHeader, jsonPayload, cp);
    }


    public static string JWT_RSASHA256(string jsonHeader, string jsonPayload, RSACryptoServiceProvider cp)
    {
      return CreateJsonWebToken(jsonHeader, jsonPayload, new RSASHA256SigningAlgorithm(cp));
    }


    public static string CreateJsonWebToken(string jsonHeader, string jsonPayload, ISigningAlgorithm sign)
    {
      string claim = Base64Utility.UTF8UrlEncode(jsonHeader) + "." + Base64Utility.UTF8UrlEncode(jsonPayload);
      string signature = sign.Sign(claim);
      return claim + "." + signature;
    }
  }
}

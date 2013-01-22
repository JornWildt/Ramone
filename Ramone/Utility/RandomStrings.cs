using System.Text;


namespace Ramone.Utility
{
  public static class RandomStrings
  {
    static char[] LetterAndDigitCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVUWXYZ0123456789".ToCharArray();

    public static string GetRandomStringWithLettersAndDigitsOnly(int length)
    {
      return GetRandomStringFromCharacterSet(length, LetterAndDigitCharacters);
    }


    public static string GetRandomStringFromCharacterSet(int length, char[] allowedCharacters)
    {
      StringBuilder s = new StringBuilder();
      for (int i = 0; i < length; i++)
      {
        byte b = CryptographicRandomNumberGenerator.GetNextByte((byte)allowedCharacters.Length);
        s.Append(allowedCharacters[b]);
      }

      return s.ToString();
    }
  }
}

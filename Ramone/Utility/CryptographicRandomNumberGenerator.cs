using System;
using System.Security.Cryptography;


namespace Ramone.Utility
{
  public class CryptographicRandomNumberGenerator
  {
    // This type is thread safe :-)
    private static RNGCryptoServiceProvider Randomizer = new RNGCryptoServiceProvider();


    public static byte GetNextByte(byte range)
    {
      if (range <= 0)
        throw new ArgumentOutOfRangeException("range");

      // Create a byte array to hold the random value.
      byte[] randomNumber = new byte[1];
      do
      {
        // Fill the array with a random value.
        Randomizer.GetBytes(randomNumber);
      }
      while (!IsFairRoll(randomNumber[0], range));

      // Return the random number mod the range.
      return (byte)(randomNumber[0] % range);
    }


    private static bool IsFairRoll(byte roll, byte range)
    {
      // There are MaxValue / numSides full sets of numbers that can come up
      // in a single byte.  For instance, if we have a 6 sided die, there are
      // 42 full sets of 1-6 that come up.  The 43rd set is incomplete.
      int fullSetsOfValues = Byte.MaxValue / range;

      // If the roll is within this range of fair values, then we let it continue.
      // In the 6 sided die case, a roll between 0 and 251 is allowed.  (We use
      // < rather than <= since the = portion allows through an extra 0 value).
      // 252 through 255 would provide an extra 0, 1, 2, 3 so they are not fair
      // to use.
      return roll < range * fullSetsOfValues;
    }
  }
}

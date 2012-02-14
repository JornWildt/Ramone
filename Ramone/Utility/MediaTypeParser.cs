using System.Text;


namespace Ramone.Utility
{
  public static class MediaTypeParser
  {
    public static Encoding GetEncodingFromCharset(string mediaType)
    {
      Encoding enc = Encoding.Default;

      MediaType m = new MediaType(mediaType);
      if (m.Parameters.ContainsKey("charset"))
        enc = Encoding.GetEncoding(m.Parameters["charset"]);

      return enc;
    }
  }
}

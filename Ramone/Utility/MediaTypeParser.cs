using System.Text;
using System.Net.Mime;


namespace Ramone.Utility
{
  public static class MediaTypeParser
  {
    public static Encoding GetEncodingFromCharset(string mediaType, Encoding defaultEncoding)
    {
      Encoding enc = defaultEncoding;

      ContentType m = new ContentType(mediaType);
      if (m.Parameters.ContainsKey("charset"))
        enc = Encoding.GetEncoding(m.Parameters["charset"]);

      // Trying to avoid UTF8Encoding that produced byte order marks
      if (enc is UTF8Encoding)
        enc = new UTF8Encoding();

      return enc;
    }
  }
}

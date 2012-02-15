using System.Text;
using System.Net.Mime;


namespace Ramone.Utility
{
  public static class MediaTypeParser
  {
    public static Encoding GetEncodingFromCharset(string mediaType)
    {
      Encoding enc = Encoding.Default;

      ContentType m = new ContentType(mediaType);
      if (m.Parameters.ContainsKey("charset"))
        enc = Encoding.GetEncoding(m.Parameters["charset"]);

      return enc;
    }
  }
}

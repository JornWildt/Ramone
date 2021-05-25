using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ramone.Utility.Validation;
using System.Web;

namespace Ramone.Utility
{
  public class HeaderEncodingParser
  {
    public static string ParseExtendedHeader(string s)
    {
      HeaderEncodingParser parser = new HeaderEncodingParser();
      return parser.Parse(s);
    }


    public string Parse(string header)
    {
      Condition.Requires(header, "header").IsNotNull();
      string[] parts = header.Split(new char[] {'\''}, 3);

      string charset = (parts.Length == 3 ? parts[0] : null);
      string language = (parts.Length == 3 ? parts[1] : null);
      string content = parts[parts.Length - 1];

      try
      {
        Encoding enc = Encoding.GetEncoding(charset);
        return HttpUtility.UrlDecode(content, enc);
      }
      catch (Exception)
      {
        return content;
      }
    }
  }
}

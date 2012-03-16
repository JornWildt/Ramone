using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CuttingEdge.Conditions;
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

      if (parts.Length != 3)
        Error("Expected two single quotes delimiters", header);

      string charset = parts[0];
      string language = parts[1];
      string content = parts[2];

      Encoding enc = Encoding.GetEncoding(charset);
      return HttpUtility.UrlDecode(content, enc);
    }


    protected void Error(string msg, string header)
    {
      throw new FormatException(string.Format("Invalid extended header. {0} in '{1}'.", msg, header));
    }
  }
}

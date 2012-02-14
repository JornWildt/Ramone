using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mime;


namespace Ramone.Utility
{
  //public class MediaType
  //{
  //  public string FullType { get; protected set; }
  //  public string Type { get; protected set; }
  //  public string SubType { get; protected set; }
  //  public Dictionary<string, string> Parameters { get; protected set; }

  //  public MediaType(string fullType, string type, string subType, Dictionary<string,string> parameters)
  //  {
  //    FullType = fullType;
  //    Type = type;
  //    SubType = subType;
  //    Parameters = parameters;
  //  }
  //}


  public static class MediaTypeParser
  {
    public static ContentType ParseMediaType(string mediaType)
    {
      return new ContentType(mediaType);
      //if (mediaType == null)
      //  return null;
      
      //List<string> elements = mediaType.Split(';').Select(e => e.Trim()).ToList();

      //string fullType = elements[0];
      //string[] typeElements = fullType.Split('/');
      //string type = typeElements[0];
      //string subType = (typeElements.Length > 1 ? typeElements[1] : "");

      //Dictionary<string, string> parameters = new Dictionary<string, string>();

      //foreach (string parameter in elements.Skip(1))
      //{
      //  string[] parameterElements = parameter.Split('=');
      //  string key = parameterElements[0];
      //  string value = (parameterElements.Length > 1 ? parameterElements[1] : "");
      //  parameters[key] = value;
      //}

      //return new MediaType(fullType, type, subType, parameters);
    }


    public static Encoding GetEncodingFromCharset(string mediaType)
    {
      Encoding enc = Encoding.Default;

      ContentType m = MediaTypeParser.ParseMediaType(mediaType);
      if (m.Parameters.ContainsKey("charset"))
        enc = Encoding.GetEncoding(m.Parameters["charset"]);

      return enc;
    }
  }
}

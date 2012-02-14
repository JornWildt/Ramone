using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CuttingEdge.Conditions;

namespace Ramone
{
  public class MediaType : System.Net.Mime.ContentType
  {
    public string TopLevelType { get; protected set; }

    public string SubType { get; protected set; }

    public bool IsTopLevelWildcard { get { return TopLevelType == "*"; } }

    public bool IsSubTypeWildcard { get { return SubType == "*"; } }

    public bool IsWildcard { get { return IsTopLevelWildcard && IsSubTypeWildcard; } }


    public static readonly MediaType ApplicationXml = new MediaType("application/xml");

    public static readonly MediaType TextXml = new MediaType("text/xml");

    public static readonly MediaType TextHtml = new MediaType("text/html");

    public static readonly MediaType TextPlain = new MediaType("text/plain");

    public static readonly MediaType ApplicationXHtml = new MediaType("application/xhtml+xml");

    public static readonly MediaType ApplicationAtom = new MediaType("application/atom+xml");

    public static readonly MediaType ApplicationJson = new MediaType("application/json");

    public static readonly MediaType ApplicationOctetStream = new MediaType("application/octet-stream");

    public static readonly MediaType MultipartFormData = new MediaType("multipart/form-data");

    public static readonly MediaType ApplicationFormUrlEncoded = new MediaType("application/x-www-form-urlencoded");

    public static readonly MediaType Wildcard = new MediaType("*/*");


    public MediaType()
    {
    }


    public MediaType(string topLevelType, string subType)
      : base(topLevelType + "/" + subType)
    {
      TopLevelType = topLevelType;
      SubType = subType;
    }


    public MediaType(string mediaType)
      : base(mediaType)
    {
      Condition.Requires(mediaType, "mediaType").IsNotNullOrEmpty();

      string[] types = base.MediaType.Split('/');
      if (types.Length != 2)
        throw new FormatException(string.Format("Cannot instantiate MediaType from '{0}' - expected exactly one '/'.", mediaType));

      TopLevelType = types[0];
      SubType = types[1];
    }


    public MediaType(MediaType src)
      : base(src.MediaType)
    {
      TopLevelType = src.TopLevelType;
      SubType = src.SubType;
    }


    public bool Matches(string mediaType)
    {
      return Matches(new MediaType(mediaType));
    }


    public bool Matches(MediaType t)
    {
      if (t == null)
        return false;

      return       (IsTopLevelWildcard || t.IsTopLevelWildcard || TopLevelType.Equals(t.TopLevelType, StringComparison.OrdinalIgnoreCase))
                && (IsSubTypeWildcard || t.IsSubTypeWildcard || SubType.Equals(t.SubType, StringComparison.OrdinalIgnoreCase));
    }
  }
}

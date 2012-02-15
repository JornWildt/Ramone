using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CuttingEdge.Conditions;

namespace Ramone
{
  public class MediaType
  {
    public string FullType { get; private set; }

    public string TopLevelType { get; private set; }

    public string SubType { get; private set; }

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


    public MediaType(string mediaType)
    {
      Condition.Requires(mediaType, "mediaType").IsNotNull();

      string[] parameters = mediaType.Split(';');
      string mediaType2 = parameters[0].Trim();

      if (mediaType2 == string.Empty)
        throw new FormatException(string.Format("The media-type string '{0}' did not contain any media-type.", mediaType));

      string[] types = mediaType2.Split('/');
      if (types.Length != 2)
        throw new FormatException(string.Format("Cannot instantiate MediaType from '{0}' - expected exactly one '/'.", mediaType));

      FullType = mediaType2;
      TopLevelType = types[0];
      SubType = types[1];

      if (TopLevelType == string.Empty)
        throw new FormatException(string.Format("The media-type string '{0}' did not contain any top-level type.", TopLevelType));

      if (SubType == string.Empty)
        throw new FormatException(string.Format("The media-type string '{0}' did not contain any sub-level type.", SubType));
    }


    public MediaType(MediaType src)
    {
      FullType = src.FullType;
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


    public override string ToString()
    {
      return FullType;
    }


    public override bool Equals(object obj)
    {
      MediaType other = obj as MediaType;
      if (other == null)
        return false;

      // Call overloaded == operator
      return this == other;
    }


    public override int GetHashCode()
    {
      return FullType.GetHashCode();
    }


    public static bool operator ==(MediaType a, MediaType b)
    {
      // If both are null, or both are same instance, return true.
      if (System.Object.ReferenceEquals(a, b))
        return true;

      // If one is null, but not both, return false.
      // Convert to null to avoid infinite recursion
      if ((object)a == null || (object)b == null)
      {
        return false;
      }

      return a.FullType.Equals(b.FullType, StringComparison.OrdinalIgnoreCase);
    }


    public static bool operator !=(MediaType a, MediaType b)
    {
      return !(a == b);
    }
  }
}

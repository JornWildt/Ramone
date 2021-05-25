using Ramone.Utility.Validation;
using System;


namespace Ramone
{
  public class MediaType
  {
    public string TopLevelType { get; private set; }

    public string SubType { get; private set; }

    public bool IsTopLevelWildcard { get { return TopLevelType == "*"; } }

    public bool IsSubTypeWildcard { get { return SubType == "*"; } }

    public bool IsWildcard { get { return IsTopLevelWildcard && IsSubTypeWildcard; } }

    public bool IsAnyWildcard { get { return IsTopLevelWildcard || IsSubTypeWildcard; } }


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


    protected string FullType { get; private set; }


    /// <summary>
    /// Create new instance of MediaType - returns null if input is null or invalid media type string.
    /// </summary>
    /// <param name="mediaType"></param>
    /// <returns></returns>
    public static MediaType Create(string mediaType)
    {
      if (mediaType == null)
        return null;

      string error;
      MediaType type;
      if (!MediaType.TryParse(mediaType, out type, out error))
        return null;

      return type;
    }


    public MediaType()
    {
    }


    public MediaType(string mediaType)
    {
      Condition.Requires(mediaType, "mediaType").IsNotNull();

      string error;
      MediaType type;
      if (!MediaType.TryParse(mediaType, out type, out error))
        throw new FormatException(error);

      FullType = type.FullType;
      TopLevelType = type.TopLevelType;
      SubType = type.SubType;
    }


    public MediaType(MediaType src)
    {
      FullType = src.FullType;
      TopLevelType = src.TopLevelType;
      SubType = src.SubType;
    }


    public MediaType(string fullType, string topLevelType, string subType)
    {
      FullType = fullType;
      TopLevelType = topLevelType;
      SubType = subType;
    }


    public static bool TryParse(string mediaType, out MediaType result, out string error)
    {
      result = null;
      error = null;

      if (mediaType == null)
      {
        error = "The parameter 'mediaType' was null.";
        return false;
      }

      string[] parameters = mediaType.Split(';');
      string mediaType2 = parameters[0].Trim();

      if (mediaType2 == string.Empty)
      {
        error = string.Format("The media-type string '{0}' did not contain any media-type.", mediaType);
        return false;
      }

      string[] types = mediaType2.Split('/');
      if (types.Length != 2)
      {
        error = string.Format("Cannot instantiate MediaType from '{0}' - expected exactly one '/'.", mediaType);
        return false;
      }

      string fullType = mediaType2;
      string topLevelType = types[0];
      string subType = types[1];

      if (topLevelType == string.Empty)
      {
        error = string.Format("The media-type string '{0}' did not contain any top-level type.", topLevelType);
        return false;
      }

      if (subType == string.Empty)
      {
        error = string.Format("The media-type string '{0}' did not contain any sub-level type.", subType);
        return false;
      }

      result = new MediaType(fullType, topLevelType, subType);

      return true;
    }


    public override string ToString()
    {
      return FullType;
    }


    public bool Matches(string mediaType)
    {
      return Matches(new MediaType(mediaType));
    }


    public bool Matches(MediaType t)
    {
      if (t == null)
        return false;

      return (IsTopLevelWildcard || t.IsTopLevelWildcard || TopLevelType.Equals(t.TopLevelType, StringComparison.OrdinalIgnoreCase))
                && (IsSubTypeWildcard || t.IsSubTypeWildcard || SubType.Equals(t.SubType, StringComparison.OrdinalIgnoreCase));
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


    public static implicit operator MediaType(string mediaType)
    {
      return Create(mediaType);
    }


    public static explicit operator string (MediaType mediaType)
    {
      return mediaType != null ? mediaType.FullType : null;
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

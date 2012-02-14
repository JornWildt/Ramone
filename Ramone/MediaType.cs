using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CuttingEdge.Conditions;

namespace Ramone
{
  public class MediaType
  {
    public string TopLevelType { get; protected set; }

    public string SubType { get; protected set; }

    public bool IsTopLevelWildcard { get { return TopLevelType == "*"; } }

    public bool IsSubTypeWildcard { get { return SubType == "*"; } }


    public MediaType()
    {
    }


    public MediaType(string topLevelType, string subType)
    {
      TopLevelType = topLevelType;
      SubType = subType;
    }


    public MediaType(string mediaType)
    {
      Condition.Requires(mediaType, "mediaType").IsNotNullOrEmpty();

      string[] types = mediaType.Split('/');
      if (types.Length != 2)
        throw new ArgumentException(string.Format("Cannot instantiate MediaType from '{0}' - expected exactly one '/'.", mediaType));
    }


    public MediaType(MediaType src)
    {
      TopLevelType = src.TopLevelType;
      SubType = src.SubType;
    }
  }
}

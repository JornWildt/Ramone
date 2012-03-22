using System.Collections.Generic;


namespace Ramone.HyperMedia
{
  public interface ISelectable
  {
    IEnumerable<string> RelationTypes { get; }

    MediaType MediaType { get; }
  }
}

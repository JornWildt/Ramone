using System.Collections.Generic;


namespace Ramone.HyperMedia
{
  public interface ISelectable
  {
    IEnumerable<string> RelationTypes { get; }

    string MediaType { get; }
  }


  public interface ILink : ISelectable
  {
    string HRef { get; }

    string Title { get; }
  }
}

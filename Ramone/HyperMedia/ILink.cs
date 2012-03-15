namespace Ramone.HyperMedia
{
  public interface ISelectable
  {
    string RelationshipType { get; }

    string MediaType { get; }
  }


  public interface ILink : ISelectable
  {
    string HRef { get; }

    string Title { get; }
  }
}

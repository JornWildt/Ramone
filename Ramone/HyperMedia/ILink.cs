namespace Ramone.HyperMedia
{
  public interface ILink
  {
    string HRef { get; }

    string RelationshipType { get; }

    string MediaType { get; }

    string Title { get; }
  }
}

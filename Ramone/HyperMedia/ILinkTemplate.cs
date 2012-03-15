namespace Ramone.HyperMedia
{
  public interface ILinkTemplate
  {
    string Template { get; }

    string RelationshipType { get; }

    string MediaType { get; }

    string Title { get; }
  }
}

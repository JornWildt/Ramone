namespace Ramone.HyperMedia
{
  public interface ILinkTemplate : ISelectable
  {
    string Template { get; }

    string Title { get; }
  }
}

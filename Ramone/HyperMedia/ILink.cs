using System;


namespace Ramone.HyperMedia
{
  public interface ILink : ISelectable
  {
    Uri HRef { get; }

    string Title { get; }
  }
}

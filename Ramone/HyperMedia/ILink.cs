using System;


namespace Ramone.HyperMedia
{
  public interface ILink : ISelectable
  {
    string HRef { get; }

    string Title { get; }
  }
}

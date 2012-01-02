using System.Collections.Generic;


namespace Ramone.MediaTypes.Atom
{
  public interface IHaveLinks
  {
    List<AtomLink> Links { get; }
  }
}

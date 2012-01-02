using System.Collections.Generic;


namespace Ramone.Common
{
  public interface IHaveLinks
  {
    List<AtomLink> Links { get; }
  }
}

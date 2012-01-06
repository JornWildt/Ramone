using System.Collections.Generic;


namespace Ramone.MediaTypes.Atom
{
  public interface IHaveAtomLinks
  {
    AtomLinkList Links { get; }
  }
}

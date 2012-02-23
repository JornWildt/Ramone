using System;
using System.Collections.Generic;


namespace Ramone.HyperMedia
{
  // FIXME: DELETE
  public interface IHyperMedia<TBody>
  {
    IEnumerable<IResponseLink> Links { get; }
    Uri BaseUri { get; }
  }
}

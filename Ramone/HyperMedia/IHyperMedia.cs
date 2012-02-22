using System;
using System.Collections.Generic;


namespace Ramone.HyperMedia
{
  public interface IHyperMedia<TBody>
  {
    IEnumerable<IResponseLink> Links { get; }
    Uri BaseUri { get; }
  }
}

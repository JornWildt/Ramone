using System.Collections.Generic;
using System.Xml.Serialization;
using Ramone.HyperMedia;
using System;


namespace Ramone.MediaTypes.Atom
{
  public class AtomLinkList : List<AtomLink>, IHaveContext
  {
    #region IHaveContext Members

    public void RegisterContext(ISession session, Uri baseUrl)
    {
      foreach (AtomLink l in this)
        l.RegisterContext(session, baseUrl);
    }

    #endregion
  }
}

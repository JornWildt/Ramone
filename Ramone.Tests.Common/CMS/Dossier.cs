using System.Collections.Generic;
using System.Xml.Serialization;
using Ramone.Common;


namespace Ramone.Tests.Common.CMS
{
  public class Dossier : IHaveLinks
  {
    public long Id { get; set; }
    
    public string Title { get; set; }

    [XmlElement("link", Namespace=AtomLink.AtomNamespace)]
    public List<AtomLink> Links { get; set; }

    
    public Dossier()
    {
      Links = new List<AtomLink>();
    }
  }
}

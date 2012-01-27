using System.Xml.Serialization;
using Ramone.HyperMedia;
using Ramone.HyperMedia.Atom;


namespace Ramone.Tests.Common.CMS
{
  public class Dossier
  {
    public long Id { get; set; }
    
    public string Title { get; set; }

    [XmlElement("link", Namespace=HyperMediaNamespaces.Atom)]
    public AtomLinkList Links { get; set; }

    
    public Dossier()
    {
      Links = new AtomLinkList();
    }
  }
}

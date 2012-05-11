using System.Xml.Serialization;
using Ramone.MediaTypes.Atom;


namespace Ramone.Tests.Common.CMS
{
  public class Dossier : IContextContainer
  {
    public long Id { get; set; }
    
    public string Title { get; set; }

    [XmlElement("link", Namespace = AtomConstants.AtomNamespace)]
    public AtomLinkList Links { get; set; }

    
    public Dossier()
    {
      Links = new AtomLinkList();
    }
  }
}

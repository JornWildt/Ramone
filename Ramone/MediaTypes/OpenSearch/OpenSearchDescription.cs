using System.Collections.Generic;
using System.Xml.Serialization;
using Ramone.HyperMedia;


namespace Ramone.MediaTypes.OpenSearch
{
  [XmlRoot(Namespace="http://a9.com/-/spec/opensearch/1.1/")]
  public class OpenSearchDescription
  {
    public string ShortName { get; set; }
    
    public string Description { get; set; }
    
    public string Contact { get; set; }

    [XmlElement("Url")]
    public List<LinkTemplate> Urls { get; set; }
  }

 

  public class LinkTemplate : ILinkTemplate
  {
    [XmlAttribute("template")]
    public string Template { get; set; }

    [XmlAttribute("rel")]
    public string RelationshipType { get; set; }

    [XmlAttribute("type")]
    public string MediaType { get; set; }

    [XmlIgnore()]
    public string Title { get; set; }
  }
}

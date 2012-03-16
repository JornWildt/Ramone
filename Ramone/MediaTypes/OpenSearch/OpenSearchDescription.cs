using System.Collections.Generic;
using System.Xml.Serialization;
using Ramone.HyperMedia;


namespace Ramone.MediaTypes.OpenSearch
{
  /// <summary>
  /// Representation of Open Search with XML markup attributes for XML serialization.
  /// </summary>
  /// <remarks>Does not contain the complete set of Open Search features. This is mostly used as
  /// a proof of concept for link templates. Feel free to add more yourself ...</remarks>
  [XmlRoot(Namespace="http://a9.com/-/spec/opensearch/1.1/")]
  public class OpenSearchDescription
  {
    public string ShortName { get; set; }
    
    public string Description { get; set; }
    
    public string Contact { get; set; }

    [XmlElement("Url")]
    public List<OpenSearchUrl> Urls { get; set; }
  }

 

  public class OpenSearchUrl : ILinkTemplate
  {
    [XmlAttribute("template")]
    public string Template { get; set; }

    [XmlAttribute("rel")]
    public string RelationshipType { get; set; }

    [XmlAttribute("type")]
    public string MediaType { get; set; }

    [XmlIgnore()]
    public string Title { get; set; }


    public OpenSearchUrl()
    {
      RelationshipType = "results";
    }
  }
}

using System.Collections.Generic;
using System.Xml.Serialization;
using Ramone.HyperMedia;
using System;


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

 

  public class OpenSearchUrl : SelectableBase, ILinkTemplate
  {
    #region Open Search URL elements

    [XmlAttribute("template")]
    public string Template { get; set; }


    /// <summary>
    /// Space separated relation types. For XML serialization.
    /// </summary>
    [XmlAttribute("rel")]
    public string RelationType
    {
      get { return GetRelationType(); }
      set { SetRelationType(value); }
    }


    [XmlIgnore()]
    public IEnumerable<string> RelationTypes
    {
      get { return GetRelationTypes(); }
    }


    [XmlAttribute("type")]
    public string MediaTypeText
    {
      get { return GetMediaTypeText(); }
      set { SetMediaType(value); }
    }


    [XmlIgnore]
    public MediaType MediaType
    {
      get { return GetMediaType(); }
      set { SetMediaType(value); }
    }

    #endregion


    [XmlIgnore()]
    public string Title { get; set; }


    public OpenSearchUrl()
    {
      RelationType = "results";
    }
  }
}

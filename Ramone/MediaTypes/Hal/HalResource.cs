using System.Collections.Generic;
using System.Xml.Serialization;


namespace Ramone.MediaTypes.Hal
{
  public class HalResource
  {
    [XmlElement("rel")]
    public string Rel { get; set; }

    [XmlElement("href")]
    public string HRef { get; set; }

    [XmlElement("name")]
    public string Name { get; set; }

    [XmlElement("type")]
    public string Type { get; set; }

    [XmlElement("link")]
    public List<HalLink> Links { get; set; }


    public HalResource()
    {
      Links = new List<HalLink>();
    }
  }
}

using System.Xml.Serialization;


namespace Ramone.MediaTypes.Hal
{
  public class HalLink
  {
    [XmlElement("rel")]
    public string Rel { get; set; }

    [XmlElement("href")]
    public string HRef { get; set; }

    [XmlElement("name")]
    public string Name { get; set; }

    [XmlElement("title")]
    public string Title { get; set; }


    public HalLink()
    {
    }


    public HalLink(string rel, string href, string title, string name = null)
    {
      Rel = rel;
      HRef = href;
      Title = title;
      Name = name;
    }
  }
}

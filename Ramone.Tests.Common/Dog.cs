using System.Xml.Serialization;


namespace Ramone.Tests.Common
{
  // Dogs come in different versions

  [XmlRoot("Dog")]
  public class Dog1
  {
    public string Name { get; set; }
  }


  [XmlRoot("Dog")]
  public class Dog2 : Dog1
  {
    public int Weight { get; set; }
  }
}

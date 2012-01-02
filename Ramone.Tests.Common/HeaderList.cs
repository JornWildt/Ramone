using System.Collections.Generic;
using System.Xml.Serialization;


namespace Ramone.Tests.Common
{
  [XmlRoot("HeaderList")]
  public class HeaderList : List<string>
  {
  }
}

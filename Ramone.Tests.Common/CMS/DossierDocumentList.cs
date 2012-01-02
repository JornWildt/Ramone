using System.Collections.Generic;
using System.Xml.Serialization;


namespace Ramone.Tests.Common.CMS
{
  [XmlRoot("Documents")]
  public class DossierDocumentList : List<Document>
  {
  }
}

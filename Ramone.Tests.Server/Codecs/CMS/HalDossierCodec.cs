using System.Xml;
using System.Xml.Serialization;
using Ramone.Tests.Common.CMS;
using Ramone.Common;
using Ramone.MediaTypes.Hal;


namespace Ramone.Tests.Server.Codecs.CMS
{
  public class HalDossierCodec : XmlCodecBase<Dossier>
  {
    XmlSerializer Serializer = new XmlSerializer(typeof(HalDossier));


    protected override void WriteTo(Dossier d, XmlWriter writer)
    {
      HalDossier hd = new HalDossier
      {
        Id = d.Id,
        Title = d.Title
      };

      foreach (AtomLink l in d.Links)
        hd.Links.Add(new HalLink(l.RelationshipType, l.HRef, l.Title));

      Serializer.Serialize(writer, hd);
    }


    protected override Dossier ReadFrom(XmlReader reader)
    {
      return null;// (Dossier)Serializer.Deserialize(reader);
    }
  }
}
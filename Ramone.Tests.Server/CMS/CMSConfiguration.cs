using OpenRasta.Configuration;
using Ramone.Tests.Common.CMS;
using Ramone.Tests.Server.CMS.Codecs;
using Ramone.Tests.Server.CMS.Handlers;


namespace Ramone.Tests.Server.CMS
{
  public static class CMSConfiguration
  {
    public static void Configure()
    {
      ResourceSpace.Has.ResourcesOfType<Dossier>()
          .AtUri(CMSConstants.DossierPath)
          .And.AtUri(CMSConstants.DossiersPath)
          .HandledBy<DossiersHandler>()
          .TranscodedBy<DossierCodec>();

      ResourceSpace.Has.ResourcesOfType<DossierDocumentList>()
          .AtUri(CMSConstants.DossierDocumentsPath)
          .HandledBy<DossierDocumentsHandler>()
          .TranscodedBy<DossierDocumentsCodec>()
          .ForMediaType(CMSConstants.CMSMediaTypeId);

      ResourceSpace.Has.ResourcesOfType<Document>()
          .AtUri(CMSConstants.DocumentPath)
          .HandledBy<DocumentHandler>()
          .TranscodedBy<DocumentCodec>()
          .ForMediaType(CMSConstants.CMSMediaTypeId);

      ResourceSpace.Has.ResourcesOfType<Party>()
          .AtUri(CMSConstants.PartyPath)
          .HandledBy<PartyHandler>()
          .TranscodedBy<PartyCodec>()
          .ForMediaType(CMSConstants.CMSMediaTypeId);
    }
  }
}
namespace Ramone.Tests.Common.CMS
{
  public static class CMSConstants
  {
    // Note: trailing slash enables relative path to documents
    public const string DossierPath = "cms/dossiers/{id}/";
    public const string DossierDocumentsPath = "cms/dossiers/{id}/documents";
    public const string DossiersPath = "cms/dossiers";
    public const string VerifiedMethodDossiersPath = "cms/dossiers/{method}";

    public const string DocumentPath = "cms/documents/{id}";
    public const string PartyPath = "cms/party/{id}";

    public const string CMSMediaTypeId = "application/vnd.cms+xml";
    public static readonly MediaType CMSMediaType = new MediaType(CMSMediaTypeId);

    public const string DocumentsLinkRelType = "documents";
    public const string PartyLinkRelType = "party";
  }
}

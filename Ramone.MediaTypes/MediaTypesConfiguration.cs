using Ramone.MediaTypes.OpenSearch;


namespace Ramone.MediaTypes
{
  public static class MediaTypesConfiguration
  {
    public static void RegisterStandardCodecs(ICodecManager cm)
    {
      // Open search
      cm.AddXml<OpenSearchDescription>(new MediaType("application/opensearchdescription+xml"));
    }
  }
}

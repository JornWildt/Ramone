using Ramone.Tests.Common.CMS;


namespace Ramone.Tests.Server.Handlers.CMS
{
  public class DossierDocumentsHandler
  {
    public DossierDocumentList Get(long id)
    {
      return new DossierDocumentList
      {
        new Document
        {
          Id = 12,
          Title = "Document no. 12",
          ContentType = "text/plain",
          ContentLength = 4
        },
        new Document
        {
          Id = 23,
          Title = "Document no. 23",
          ContentType = "text/plain",
          ContentLength = 4
        }
      };
    }
  }
}
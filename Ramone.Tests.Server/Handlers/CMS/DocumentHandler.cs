using Ramone.Tests.Common.CMS;


namespace Ramone.Tests.Server.Handlers.CMS
{
  public class DocumentHandler
  {
    public Document Get(long id)
    {
      Document doc = new Document
      {
        Id = id,
        Title = string.Format("Document no. {0}", id),
        ContentType = "text/plain",
        ContentLength = 4
      };

      return doc;
    }
  }
}
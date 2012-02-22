using OpenRasta.Web;


namespace Ramone.Tests.Server.Handlers.Blog
{
  public class AuthorHandler
  {
    public Author Get(int id)
    {
      Author a = new Author
      {
        Id = id,
        Name = "Pete no. " + id,
        EMail = "pete" + id + "@mail.dk"
      };

      a.SelfLink = typeof(Author).CreateUri(new { Id = a.Id });

      return a;
    }
  }
}
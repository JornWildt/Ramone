using OpenRasta.Web;
using Ramone.Tests.Server.Blog.Data;
using Ramone.Tests.Server.Blog.Resources;


namespace Ramone.Tests.Server.Blog.Handlers
{
  public class BlogItemHandler
  {
    public BlogItem Get(int id)
    {
      BlogDB.PostEntry postEntry = BlogDB.Get(id);
      AuthorDB.AuthorEntry authorEntry = AuthorDB.Get(postEntry.AuthorId);

      BlogItem item = new BlogItem
      {
        Id = postEntry.Id,
        Title = postEntry.Title,
        Text = postEntry.Text,
        CreatedDate = postEntry.CreatedDate,
        AuthorName = authorEntry.Name
      };

      item.SelfLink = typeof(BlogItem).CreateUri(new { Id = postEntry.Id });
      item.UpLink = typeof(BlogList).CreateUri();
      item.AuthorLink = typeof(Author).CreateUri(new { Id = authorEntry.Id });

      if (postEntry.ImageId != null)
        item.ImageLink = typeof(Image).CreateUri(new { Id = postEntry.ImageId });

      return item;
    }
  }
}
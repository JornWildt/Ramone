using System;
using OpenRasta.Web;


namespace Ramone.Tests.Server.Handlers.Blog
{
  public class BlogItemHandler
  {
    public BlogItem Get(int id)
    {
      BlogDB.PostEntry entry = BlogDB.Get(id);

      BlogItem item = new BlogItem
      {
        Id = entry.Id,
        Title = entry.Title,
        Text = entry.Text,
        CreatedDate = entry.CreatedDate
      };

      item.SelfLink = typeof(BlogItem).CreateUri(new { id = entry.Id });
      item.UpLink = typeof(BlogList).CreateUri();

      return item;
    }
  }
}
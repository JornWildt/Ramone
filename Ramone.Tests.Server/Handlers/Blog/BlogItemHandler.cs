using System;
using OpenRasta.Web;


namespace Ramone.Tests.Server.Handlers.Blog
{
  public class BlogItemHandler
  {
    public BlogItem Get(int id)
    {
      BlogItem item = new BlogItem
      {
        Id = id,
        Title = "Blog Item No. " + id,
        Text = "Blah blah blah ..:",
        CreatedDate = DateTime.Now
      };

      item.SelfLink = typeof(BlogItem).CreateUri(new { id = id });
      item.UpLink = typeof(BlogList).CreateUri();

      return item;
    }
  }
}
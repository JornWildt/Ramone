using System.Collections.Generic;
using System.Linq;
using OpenRasta.Web;


namespace Ramone.Tests.Server.Handlers.Blog
{
  public class BlogListHandler
  {
    public BlogList Get()
    {
      BlogItemHandler itemHandler = new BlogItemHandler();

      BlogList list = new BlogList
      {
        Title = "A mixed blog",
      };

      list.Items = BlogDB.GetAll().Select(entry => new BlogItem
        {
          Id = entry.AuthorId,
          Title = entry.Title,
          Text = entry.Text,
          CreatedDate = entry.CreatedDate,
          SelfLink = typeof(BlogItem).CreateUri(new { Id = entry.Id })
        }).ToList();

      AuthorDB.AuthorEntry author = AuthorDB.Get(0);
      list.AuthorName = author.Name;
      list.AuthorLink = typeof(Author).CreateUri(new { Id = author.Id });

      return list;
    }


    public void Delete()
    {
      BlogDB.Reset();
    }
  }
}

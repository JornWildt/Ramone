using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ramone.Tests.Server.Handlers.Blog
{
  public class BlogListHandler
  {
    public BlogList Get()
    {
      BlogItemHandler itemHandler = new BlogItemHandler();

      BlogList list = new BlogList
      {
        Title = "My personal blog",
        Items = new List<BlogItem>()
        {
          itemHandler.Get(5),
          itemHandler.Get(12)
        }
      };

      return list;
    }
  }
}
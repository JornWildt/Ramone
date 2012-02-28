using System;
using System.Collections.Generic;


namespace Ramone.Tests.Blog.Resources
{
  public class Blog
  {
    public string Title { get; set; }
    public List<Post> Items { get; set; }
    public string AuthorName { get; set; }

    public Uri AuthorLink { get; set; }
    public Uri EditLink { get; set; }
    
    
    public class Post
    {
      public string Title { get; set; }
      public string Text { get; set; }

      public Uri SelfLink { get; set; }
    }
  }
}

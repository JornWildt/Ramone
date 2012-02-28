using System;
using System.Collections.Generic;
using Ramone.HyperMedia;


namespace Ramone.Tests.Blog.Resources
{
  public class Blog
  {
    public string Title { get; set; }
    public List<Post> Posts { get; set; }

    public IEnumerable<ILink> Links { get; set; }
    
    
    public class Post
    {
      public string Title { get; set; }
      public string Text { get; set; }

      public Uri SelfLink { get; set; }
    }
  }
}

using System;
using System.Collections.Generic;
using Ramone.HyperMedia;


namespace Ramone.Tests.Blog.Resources
{
  public class Post
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public DateTime CreatedDate { get; set; }
    public string AuthorName { get; set; }

    public IEnumerable<ILink> Links { get; set; }
  }
}

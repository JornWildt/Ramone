using System;
using System.Collections.Generic;


namespace Ramone.Tests.Server.Blog.Resources
{
  public class BlogList
  {
    public string Title { get; set; }
    public List<BlogItem> Items { get; set; }
    public string AuthorName { get; set; }

    public Uri AuthorLink { get; set; }
    public Uri EditLink { get; set; }
    public string SearchDescriptionLink { get; set; }
  }
}
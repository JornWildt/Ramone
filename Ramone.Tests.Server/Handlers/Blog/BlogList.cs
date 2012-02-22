using System.Collections.Generic;


namespace Ramone.Tests.Server.Handlers.Blog
{
  public class BlogList
  {
    public string Title { get; set; }
    public List<BlogItem> Items { get; set; }
  }
}
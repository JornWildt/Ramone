using System;


namespace Ramone.Tests.Blog.Resources
{
  public class Post
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public DateTime CreatedDate { get; set; }
    public string AuthorName { get; set; }

    public Uri UpLink { get; set; }
    public Uri SelfLink { get; set; }
    public Uri AuthorLink { get; set; }
    public Uri ImageLink { get; set; }
  }
}

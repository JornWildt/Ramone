using System;


namespace Ramone.Tests.Server.Blog.Resources
{
  public class Author
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string EMail { get; set; }

    public Uri SelfLink { get; set; }
  }
}
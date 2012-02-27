using System.IO;


namespace Ramone.Tests.Server.Blog.Resources
{
  public class Image
  {
    public int Id { get; set; }
    public MemoryStream Data { get; set; }
  }
}
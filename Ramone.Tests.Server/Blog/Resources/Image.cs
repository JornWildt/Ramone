using System.IO;


namespace Ramone.Tests.Server.Blog.Resources
{
  public class Image
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public OpenRasta.Web.MediaType MediaType { get; set; }
    public MemoryStream Data { get; set; }
  }
}
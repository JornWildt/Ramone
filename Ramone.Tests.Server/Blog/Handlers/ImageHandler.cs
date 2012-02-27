using OpenRasta.Web;
using Ramone.Tests.Server.Blog.Data;
using Ramone.Tests.Server.Blog.Resources;


namespace Ramone.Tests.Server.Blog.Handlers
{
  public class ImageHandler
  {
    public Image Get(int id)
    {
      ImageDB.ImageEntry entry = ImageDB.Get(id);

      Image i = new Image
      {
        Id = entry.Id,
        Name = entry.Name,
        MediaType = entry.MediaType,
        Data = entry.Data
      };

      //a.SelfLink = typeof(Image).CreateUri(new { Id = a.Id });

      return i;
    }
  }
}
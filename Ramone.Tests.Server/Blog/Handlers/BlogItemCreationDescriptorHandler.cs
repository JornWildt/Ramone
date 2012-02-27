using System.IO;
using OpenRasta.Web;
using Ramone.Tests.Server.Blog.Data;
using Ramone.Tests.Server.Blog.Resources;


namespace Ramone.Tests.Server.Blog.Handlers
{
  public class BlogItemInput
  {
    public string Title { get; set; }
    public string Text { get; set; }
    public Stream Image { get; set; }

    public string Create { get; set; }
  }


  public class BlogItemCreationDescriptorHandler
  {
    public object Get()
    {
      return new BlogItemCreationDescriptor
      {
        PostLink = typeof(BlogItemCreationDescriptor).CreateUri()
      };
    }


    public object Post(BlogItemInput input)
    {
      ImageDB.ImageEntry imageEntry = ImageDB.AddImage("unknown", (MemoryStream)input.Image);
      BlogDB.PostEntry postEntry = BlogDB.AddPost(input.Title, input.Text, 1, imageEntry.Id);

      BlogItemHandler h = new BlogItemHandler();
      BlogItem createdItem = h.Get(postEntry.Id);

      return new OperationResult.Created
      {
        ResponseResource = createdItem,
        RedirectLocation = typeof(BlogItem).CreateUri(new { id = postEntry.Id })
      };
    }
  }
}
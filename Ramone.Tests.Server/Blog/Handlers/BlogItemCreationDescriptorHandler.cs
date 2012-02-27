using System.IO;
using OpenRasta.Web;
using Ramone.Tests.Server.Blog.Data;
using Ramone.Tests.Server.Blog.Resources;
using OpenRasta.IO;


namespace Ramone.Tests.Server.Blog.Handlers
{
  public class BlogItemInput
  {
    public string Title { get; set; }
    public string Text { get; set; }
    public IFile Image { get; set; }

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
      int? imageId = null;
      MemoryStream imageData = new MemoryStream();

      if (input.Image != null && input.Image.Length > 0)
      {
        CopyStream(input.Image.OpenStream(), imageData);
        ImageDB.ImageEntry imageEntry = ImageDB.AddImage(input.Image.FileName, input.Image.ContentType, imageData);
        imageId = imageEntry.Id;
      }

      BlogDB.PostEntry postEntry = BlogDB.AddPost(input.Title, input.Text, 1, imageId);

      BlogItemHandler h = new BlogItemHandler();
      BlogItem createdItem = h.Get(postEntry.Id);

      return new OperationResult.Created
      {
        ResponseResource = createdItem,
        RedirectLocation = typeof(BlogItem).CreateUri(new { id = postEntry.Id })
      };
    }


    public static void CopyStream(Stream input, Stream output)
    {
      byte[] buffer = new byte[32768];
      int read;
      while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
      {
        output.Write(buffer, 0, read);
      }
    }
  }
}
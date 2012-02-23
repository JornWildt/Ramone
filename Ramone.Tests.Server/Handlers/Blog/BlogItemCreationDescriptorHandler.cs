using OpenRasta.Web;


namespace Ramone.Tests.Server.Handlers.Blog
{
  public class BlogItemInput
  {
    public string Title { get; set; }
    public string Text { get; set; }

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
      BlogDB.PostEntry entry = BlogDB.AddPost(input.Title, input.Text, 1);

      BlogItemHandler h = new BlogItemHandler();
      BlogItem createdItem = h.Get(entry.Id);

      return new OperationResult.Created
      {
        ResponseResource = createdItem,
        RedirectLocation = typeof(BlogItem).CreateUri(new { id = entry.Id })
      };
    }
  }
}
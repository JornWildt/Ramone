using Ramone.IO;


namespace Ramone.Tests.Blog.Resources
{
  public class CreatePostArgs
  {
    public string Title { get; set; }
    public string Text { get; set; }
    public IFile Image { get; set; }
  }
}

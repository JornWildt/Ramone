namespace Ramone.Tests.Blog
{
  public class BlogTestHelper : TestHelper
  {
    public const string BlogRootPath = "blog/list";


    protected override void SetUp()
    {
      base.SetUp();

      // Clear the in-memory database
      RamoneRequest blogRequest = Session.Bind(BlogRootPath);
      blogRequest.Delete();
    }
  }
}

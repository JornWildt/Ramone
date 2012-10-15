namespace Ramone.Tests.Blog
{
  public class BlogTestHelper : TestHelper
  {
    private static bool FirstTime = true;

    public const string BlogRootPath = "blog/list";


    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      if (FirstTime)
      {
        TestService.CodecManager.AddCodec<Resources.Blog, Codecs.Html.BlogCodec_Html>(MediaType.TextHtml);
        TestService.CodecManager.AddCodec<Resources.Post, Codecs.Html.PostCodec_Html>(MediaType.TextHtml);
        TestService.CodecManager.AddCodec<Resources.Author, Codecs.Html.AuthorCodec_Html>(MediaType.TextHtml);
        TestService.CodecManager.AddCodec<Resources.CreatePostDescriptor, Codecs.Html.CreatePostDescriptorCodec_Html>(MediaType.TextHtml);
        FirstTime = false;
      }
    }


    protected override void SetUp()
    {
      base.SetUp();

      // Clear the in-memory database
      Request blogRequest = Session.Bind(BlogRootPath);
      blogRequest.Delete().Dispose();
    }
  }
}

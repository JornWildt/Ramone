namespace Ramone.Tests.Server.Blog
{
  public static class BlogConstants
  {
    public const string BlogListPath = "blog/list";
    public const string BlogItemPath = "blog/item/{id}";
    public const string BlogItemCreationPath = "blog/create";
    public const string AuthorPath = "blog/author/{id}";
    public const string ImagePath = "blog/image/{id}";
    public const string SearchDescriptionSubPath = "searchform";
    public const string SearchDescriptionPath = "blog/" + SearchDescriptionSubPath;
    public const string SearchResult = "blog/search?q={q}";
  }
}
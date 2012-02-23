using System;
using System.Collections.Generic;


namespace Ramone.Tests.Server.Handlers.Blog
{
  public static class BlogDB
  {
    public class PostEntry
    {
      public int Id { get; set; }
      public string Title { get; set; }
      public string Text { get; set; }
      public DateTime CreatedDate { get; set; }
      public int AuthorId { get; set; }
    }


    private static List<PostEntry> Posts { get; set; }


    static BlogDB()
    {
      Posts = new List<PostEntry>();
    }


    public static int AddPost(string title, string text, int authorId)
    {
      PostEntry entry = new PostEntry
      {
        Id = Posts.Count,
        Title = title,
        Text = text,
        AuthorId = authorId,
        CreatedDate = DateTime.Now
      };

      Posts.Add(entry);

      return entry.Id;
    }


    public static IEnumerable<PostEntry> GetAll()
    {
      return Posts;
    }


    public static PostEntry Get(int id)
    {
      return Posts[id];
    }


    public static void Clear()
    {
      Posts.Clear();
    }


    public static void Reset()
    {
      BlogDB.Clear();
      AuthorDB.Clear();

      int author1Id = AuthorDB.AddAuthor("Pete Peterson", "pp@ramonerest.dk");
      int author2Id = AuthorDB.AddAuthor("Bo Borentson", "bb@ramonerest.dk");
      int author3Id = AuthorDB.AddAuthor("Chris Christofferson", "dd@ramonerest.dk");

      BlogDB.AddPost("Hot summer", "It is a hot summer this year.", author2Id);
      BlogDB.AddPost("Cold winter", "It is a cold winter this year.", author3Id);
    }
  }
}
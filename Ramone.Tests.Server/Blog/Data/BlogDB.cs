using System;
using System.Collections.Generic;
using System.IO;


namespace Ramone.Tests.Server.Blog.Data
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
      public int? ImageId { get; set; }
    }


    private static List<PostEntry> Posts { get; set; }


    static BlogDB()
    {
      Posts = new List<PostEntry>();
    }


    public static PostEntry AddPost(string title, string text, int authorId, int? imageId)
    {
      PostEntry entry = new PostEntry
      {
        Id = Posts.Count,
        Title = title,
        Text = text,
        AuthorId = authorId,
        CreatedDate = DateTime.Now,
        ImageId = imageId
      };

      Posts.Add(entry);

      return entry;
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
      ImageDB.Clear();

      AuthorDB.AuthorEntry author1 = AuthorDB.AddAuthor("Pete Peterson", "pp@ramonerest.dk");
      AuthorDB.AuthorEntry author2 = AuthorDB.AddAuthor("Bo Borentson", "bb@ramonerest.dk");
      AuthorDB.AuthorEntry author3 = AuthorDB.AddAuthor("Chris Christofferson", "cc@ramonerest.dk");

      BlogDB.AddPost("Hot summer", "It is a hot summer this year.", author2.Id, null);
      BlogDB.AddPost("Cold winter", "It is a cold winter this year.", author3.Id, null);
    }
  }
}
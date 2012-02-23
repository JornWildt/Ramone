using System.Collections.Generic;


namespace Ramone.Tests.Server.Handlers.Blog
{
  public static class AuthorDB
  {
    public class AuthorEntry
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public string EMail { get; set; }
    }


    private static List<AuthorEntry> Authors { get; set; }


    static AuthorDB()
    {
      Authors = new List<AuthorEntry>();
    }


    public static AuthorEntry AddAuthor(string name, string email)
    {
      AuthorEntry entry = new AuthorEntry
      {
        Id = Authors.Count,
        Name = name,
        EMail = email
      };

      Authors.Add(entry);

      return entry;
    }


    public static AuthorEntry Get(int id)
    {
      return Authors[id];
    }


    public static void Clear()
    {
      Authors.Clear();
    }
  }
}
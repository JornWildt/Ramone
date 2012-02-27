using System.Collections.Generic;
using System.IO;


namespace Ramone.Tests.Server.Blog.Data
{
  public static class ImageDB
  {
    public class ImageEntry
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public OpenRasta.Web.MediaType MediaType { get; set; }
      public MemoryStream Data { get; set; }
    }


    private static List<ImageEntry> Images { get; set; }


    static ImageDB()
    {
      Images = new List<ImageEntry>();
    }


    public static ImageEntry AddImage(string name, OpenRasta.Web.MediaType mediaType, MemoryStream data)
    {
      ImageEntry entry = new ImageEntry
      {
        Id = Images.Count,
        Name = name,
        MediaType = mediaType,
        Data = data
      };

      Images.Add(entry);

      return entry;
    }


    public static ImageEntry Get(int id)
    {
      return Images[id];
    }


    public static void Clear()
    {
      Images.Clear();
    }
  }
}
using System.IO;


namespace Ramone
{
  public static class FileExtensions
  {
    public static void SaveToFile(this Resource resource, string filename)
    {
      using (Stream w = new FileStream(filename, FileMode.OpenOrCreate))
      {
        resource.Response.GetResponseStream().CopyTo(w);
      }
    }
  }
}

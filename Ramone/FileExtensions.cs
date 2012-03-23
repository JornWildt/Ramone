using System.IO;


namespace Ramone
{
  public static class FileExtensions
  {
    public static void SaveToFile(this Response resource, string filename)
    {
      using (Stream w = new FileStream(filename, FileMode.OpenOrCreate))
      {
        resource.WebResponse.GetResponseStream().CopyTo(w);
      }
    }
  }
}

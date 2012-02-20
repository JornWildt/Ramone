namespace Ramone.Tests.Common
{
  public class FormUrlEncodedData
  {
    public string Title { get; set; }
    public int Age { get; set; }
    public FormUrlEncodedSubData SubData { get; set; }
  }
  
  
  public class FormUrlEncodedSubData
  {
    public string Name { get; set; }
  }
}

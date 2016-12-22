namespace Ramone.Tests.Common
{
  // Use this to indicate that null values should be included in serialization
  public interface ISerializeWithNullValues
  {
  };


  public class FormUrlEncodedData : ISerializeWithNullValues
  {
    public string Title { get; set; }
    public int Age { get; set; }
    public FormUrlEncodedSubData SubData { get; set; }
    public int? NullValue { get; set; }
  }
  
  
  public class FormUrlEncodedSubData
  {
    public string Name { get; set; }
  }
}

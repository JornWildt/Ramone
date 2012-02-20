using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Handlers
{
  public class FormUrlEncodedHandler
  {
    public object Get()
    {
      return new FormUrlEncodedData
      {
        Title = "Abc",
        Age = 15,
        SubData = new FormUrlEncodedSubData { Name = "Grethe" }
      };
    }
  }
}
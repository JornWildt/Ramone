using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Handlers
{
  public class FormUrlEncodedHandler
  {
    public object Get()
    {
      return new FormUrlEncodedData
      {
        Title = "Abc"
      };
    }
  }
}
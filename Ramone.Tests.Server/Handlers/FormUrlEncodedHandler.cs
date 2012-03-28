using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Handlers
{
  public class FormUrlEncodedHandler
  {
    public object Get(string mode = null)
    {
      return new FormUrlEncodedData
      {
        Title = (mode == "intl" ? "ÆØÅ" : "Abc"),
        Age = 15,
        SubData = new FormUrlEncodedSubData { Name = (mode == "intl" ? "Güntør" : "Grete") }
      };
    }
  }
}
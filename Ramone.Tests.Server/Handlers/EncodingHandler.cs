using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Handlers
{
  public class EncodingHandler
  {
    public object Get()
    {
      return new EncodingData
      {
        Data = string.Format("<html><body>{0}</body></html>", "ÆØÅúï")
      };
    }


    public object Post(EncodingData data)
    {
      return new EncodingData
      {
        Data = data.Data
      };
    }
  }
}
using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Handlers
{
  public class EncodingHandler
  {
    public object Get(string type)
    {
      if (type == "html" || type == "xml")
        return new EncodingData
        {
          Data = "<html><body>ÆØÅúï´`'</body></html>"
        };
      else
        return new EncodingData
        {
          Data = "{ Name: \"ÆØÅúï´`'\\\"\" }"
        };
    }


    public object Post(string type, EncodingData data)
    {
      return new EncodingData
      {
        Data = data.Data
      };
    }
  }
}
using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Handlers
{
  public class CatHandler
  {
    public Cat Get(string name)
    {
      return new Cat { Name = name };
    }


    public Cat Post(Cat c)
    {
      return c;
    }
  }
}
using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Handlers
{
  public class DogHandler
  {
    public object Get(string name)
    {
      return new Dog2 { Name = name, Weight = 25 };
    }
  }
}
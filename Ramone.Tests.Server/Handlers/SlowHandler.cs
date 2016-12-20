using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Handlers
{
  public class SlowHandler
  {
    public object Get(int sec = 4)
    {
      System.Threading.Thread.Sleep(sec * 1000);
      return new SlowResource { Time = sec };
    }


    public object Post()
    {
      System.Threading.Thread.Sleep(4000);
      return new SlowResource { Time = 4 };
    }


    public object Post(SlowResource r)
    {
      System.Threading.Thread.Sleep(4000);
      return new SlowResource { Time = r.Time };
    }
  }
}
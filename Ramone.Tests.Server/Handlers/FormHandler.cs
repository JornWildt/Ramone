using System;
using OpenRasta.Web;
using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Handlers
{
  public class TestForm
  {
    public Uri ActionUrl { get; set; }
  }


  public class FormHandler
  {
    public object Get()
    {
      return new TestForm
      {
        ActionUrl = typeof(TestForm).CreateUri()
      };
    }


    public object Post(FormArgs args)
    {
      return args;
    }
  }
}
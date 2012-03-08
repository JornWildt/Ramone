using System;
using OpenRasta.Web;
using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Handlers
{
  public class TestForm : FormArgs
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


    public object Post(TestForm args)
    {
      if (args.MultiSelect != null)
        args.MultiSelectValue = string.Join(",", args.MultiSelect);
      return args;
    }
  }
}
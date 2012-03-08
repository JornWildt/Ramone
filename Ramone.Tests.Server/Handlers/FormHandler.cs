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
    public object Get(string actionUrlMode)
    {
      Uri actionUrl = null;
      if (actionUrlMode == "absolute")
        actionUrl = typeof(TestForm).CreateUri();

      return new TestForm
      {
        ActionUrl = actionUrl
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
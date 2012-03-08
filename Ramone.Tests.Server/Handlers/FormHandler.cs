using System;
using OpenRasta.Web;
using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Handlers
{
  public class TestForm : FormArgs
  {
    public string ActionUrl { get; set; }
  }


  public class FormHandler
  {
    public object Get(string actionUrlMode)
    {
      string actionUrl = null;
      if (actionUrlMode == "absolute")
        actionUrl = typeof(TestForm).CreateUri(new { actionUrlMode = actionUrlMode }).AbsoluteUri;
      else if (actionUrlMode == "relative")
        actionUrl = typeof(TestForm).CreateUri(new { actionUrlMode = actionUrlMode }).AbsolutePath;

      return new TestForm
      {
        ActionUrl = actionUrl
      };
    }


    public object Post(string actionUrlMode, TestForm args)
    {
      if (args.MultiSelect != null)
        args.MultiSelectValue = string.Join(",", args.MultiSelect);
      return args;
    }
  }
}
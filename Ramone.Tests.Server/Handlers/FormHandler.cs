using System;
using OpenRasta.Web;
using Ramone.Tests.Common;
using System.Web;


namespace Ramone.Tests.Server.Handlers
{
  public class TestForm : FormArgs
  {
    public string ActionUrl { get; set; }
  }


  public class FormHandler
  {
    public object Get(string actionUrlMode, string encType)
    {
      string actionUrl = null;
      if (actionUrlMode == "absolute")
        actionUrl = typeof(TestForm).CreateUri(new { actionUrlMode = actionUrlMode, encType = encType }).AbsoluteUri;
      else if (actionUrlMode == "relative")
        actionUrl = typeof(TestForm).CreateUri(new { actionUrlMode = actionUrlMode, encType = encType }).AbsolutePath;

      return new TestForm
      {
        ActionUrl = actionUrl,
        EncType = (encType == "multipart" ? MediaType.MultipartFormData.FullType : MediaType.ApplicationFormUrlEncoded.FullType)
      };
    }


    public object Post(string actionUrlMode, string encType, TestForm args)
    {
      if (args.MultiSelect != null)
        args.MultiSelectValue = string.Join(",", args.MultiSelect);
      args.EncType = (HttpContext.Current.Request.ContentType.StartsWith(MediaType.MultipartFormData.FullType) ? "multipart" : "urlencoded");
      return args;
    }
  }
}
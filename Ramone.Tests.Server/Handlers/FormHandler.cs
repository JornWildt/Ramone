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
    public object Get(string actionUrlMode, string encType, string charset)
    {
      string actionUrl = null;
      if (actionUrlMode == "absolute")
        actionUrl = typeof(TestForm).CreateUri(new { actionUrlMode = actionUrlMode, encType = encType, charset = charset }).AbsoluteUri;
      else if (actionUrlMode == "relative")
        actionUrl = typeof(TestForm).CreateUri(new { actionUrlMode = actionUrlMode, encType = encType, charset = charset }).AbsolutePath;

      return new TestForm
      {
        ActionUrl = actionUrl,
        EncType = (encType == "multipart" ? (string)MediaType.MultipartFormData : (string)MediaType.ApplicationFormUrlEncoded),
        Charset = charset
      };
    }


    public object Post(string actionUrlMode, string encType, string charset, TestForm args)
    {
      if (args.MultiSelect != null)
        args.MultiSelectValue = string.Join(",", args.MultiSelect);
      args.EncType = (HttpContext.Current.Request.ContentType.StartsWith((string)MediaType.MultipartFormData) ? "multipart" : "urlencoded");
      args.Charset = HttpContext.Current.Request.ContentEncoding.WebName;
      return args;
    }
  }
}
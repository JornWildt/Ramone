using System;
using OpenRasta.Web;
using Ramone.Tests.Common;
using System.Web;
using Template = Tavis.UriTemplates.UriTemplate;


namespace Ramone.Tests.Server.Handlers
{
  public class TestForm : FormArgs
  {
    public string ActionUrl { get; set; }
  }


  public class FormHandler
  {
    public ICommunicationContext CommunicationContext { get; set; }

    public object Get(string actionUrlMode, string encType, string charset, string method, string InputText, string Select)
    {
      InputText = HttpUtility.UrlDecode(InputText);
      Select = HttpUtility.UrlDecode(Select);
      string actionUrl = null;
      object parameters = new { actionUrlMode = actionUrlMode, encType = encType, charset = charset, method = method };
      Uri newUri = BindingExtensions.BindTemplate(CommunicationContext.ApplicationBaseUri, new Template(Constants.FormSimplePath), parameters);

      if (actionUrlMode == "absolute")
        actionUrl = newUri.AbsoluteUri;
      else if (actionUrlMode == "relative")
        actionUrl = newUri.AbsolutePath;

      TestForm args = new TestForm();

      args.InputText = InputText;
      args.Select = Select;
      args.ActionUrl = actionUrl;
      args.EncType = (encType == "multipart" ? (string)MediaType.MultipartFormData : (string)MediaType.ApplicationFormUrlEncoded);
      args.Charset = charset;
      args.Method = method;

      return args;
    }


    public object Post(string actionUrlMode, string encType, string charset, TestForm args)
    {
      if (args.MultiSelect != null)
        args.MultiSelectValue = string.Join(",", args.MultiSelect);
      args.EncType = (HttpContext.Current.Request.ContentType.StartsWith((string)MediaType.MultipartFormData) ? "multipart" : "urlencoded");
      return args;
    }
  }
}
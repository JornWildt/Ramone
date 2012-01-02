using System;
using System.Web.UI;

namespace Ramone.Tests.Server.WebForms
{
  public partial class Errors : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      int code = int.Parse(Request.Params["code"]);
      string description = Request.Params["description"];
      Response.StatusCode = code;
      Response.StatusDescription = description;
      Response.Clear();
      Response.End();
    }
  }
}

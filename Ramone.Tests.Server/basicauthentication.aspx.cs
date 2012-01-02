using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Ramone.Tests.Server
{
  public partial class basicauthentication : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      bool authorized = false;
      if (Request.Headers["Authorization"] != null)
      {
        string b64 = Request.Headers["Authorization"].Substring(5);
        string[] unamepasswd = Encoding.Default.GetString(Convert.FromBase64String(b64)).Split(':');
        if (unamepasswd[0] == "John" && unamepasswd[1] == "magic")
          authorized = true;
      }
       
      if (!authorized)
      {
        Response.Headers.Add("WWW-Authenticate", "Basic realm=\"Ramone resource\"");
        Response.StatusCode = 401;
        Response.StatusDescription = "Not authorized";
        Response.Clear();
        Response.End();
        return;
      }
    }
  }
}
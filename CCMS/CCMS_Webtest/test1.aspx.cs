using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ccms.utils;
using ccms.managers;

namespace ccms
{
    public partial class test1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //build the logout button:
            //this.logoutlink.NavigateUrl = Request.ServerVariables["URL"] + "?logout";

            HttpCookie sessionCookie = Request.Cookies["CCMSSession"];
            if (sessionCookie != null)
            {
                User user = (new SessionManager((new DBSession()))).getLoggedInUser(sessionCookie.Value);

                //Label1.Text = user.fullName;
            }
            else
            {
               // Label1.Text = "No user logged in";
            }

            if (Request.ServerVariables["URL"].IndexOf("?logout") != -1 ||
               Request.ServerVariables["URL"].IndexOf("&logout") != -1)
            {
                //Label1.Text += "logout requested";
            }
        }
    }
}

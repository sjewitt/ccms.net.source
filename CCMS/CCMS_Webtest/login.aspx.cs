using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccms;
using ccms.managers;
using ccms.utils;
/**
 * Login logic:
 * 
 * 
 */
namespace ccms
{
    public partial class login : System.Web.UI.Page
    {
        User user = null;
        DBSession session = null;
        UserManager userManager = null;
        HttpCookie sessionCookie = null;
        HttpCookie sessionModeCookie = null;
        SessionManager sessionManager = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = null;
            session = new DBSession();
            userManager = new UserManager(session);
            sessionManager = new SessionManager(session);

            //is a session already present?
            sessionCookie = Request.Cookies["CCMSSession"];
            //handle logout request:
            if (Request.QueryString["loginmode"]!= null && Request.QueryString["loginmode"].Equals("logout"))
            {
                this.logout(Request.QueryString["redirecturl"]);
            }

            
            if (sessionCookie != null)
            {
                //the user **MIGHT** be null at this point if a logout request has already been processed:
                user = sessionManager.getLoggedInUser(sessionCookie.Value);
                if (user != null)
                {
                    this.pnl_loggedIn.Visible = true;
                    this.lbl_loggedIn.Text = "User '" + user.fullName + "' already logged in.";
                    this.lbl_loggedIn.Visible = true;
                    this.pnl_loginform.Visible = false;
                }

                //if a login request is sent while a stale cookie is present, we must handle that as well:
                //if(user == null && )
                ///{

                //}
            }
        }

        /*
         Handle login request:
         */
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //attempt login:
            LoginWrapper loginWrapper = new LoginWrapper();
            loginWrapper.Username = this.txtUsername.Text;
            loginWrapper.Password = this.txtPassword.Text;
            user = userManager.loginUser(loginWrapper);

            if (user != null)
            {
                //http://asp.net2.aspfaq.com/xml-serialization/simple-serialization.html
                //SessionManager sessionManager = new SessionManager(session);
                //first request
                //HttpCookie sessionCookie = Request.Cookies["CCMSSession"];
                //if (sessionCookie == null)
                //{
                    //generate the cookie:
                    sessionCookie = new HttpCookie("CCMSSession");
                    sessionCookie.Value = Session.SessionID;

                    sessionModeCookie = new HttpCookie("CCMSSessionMode");
                    sessionModeCookie.Value = Request.QueryString["loginmode"];

                    Response.Cookies.Add(sessionCookie);
                    Response.Cookies.Add(sessionModeCookie);

                    sessionManager.createSession(user, Session.SessionID);
                    Response.Redirect(Request.QueryString["redirecturl"]);
                //}
                //else
                //{
                    //get from session database table based on the cookie session ID:
                //    sessionManager.getLoggedInUser(sessionCookie.Value);
                //}

                //this.Visible = false;

                //this.loginform.Text += "request session: " + Session.SessionID + "<br />"
                //this.lblDebug.Text += "request session: " + Session.SessionID + "<br />"
                // + "user: " + user.fullName + "<br />"
                // + "CCMS Session" + sessionCookie.Value + "<br />"; 
            }
            else
            {
                this.lblDebug.Text += "Cannot log in! Please check username and pasword";
            }

        }

        protected void lnkBtn_logOut_Click(object sender, EventArgs e)
        {
            this.logout("login.aspx");
        }


        protected void logout(string redirectUrl)
        {
            //kill the session:
            sessionManager.expireSession(sessionCookie.Value);
            
            //kill the cookie:
            Response.Cookies["CCMSSession"].Expires = DateTime.Now.AddYears(-10);
            Response.Cookies.Remove("CCMSSession");
            Response.Cookies["CCMSSessionMode"].Expires = DateTime.Now.AddYears(-10);
            Response.Cookies.Remove("CCMSSessionMode");
            Response.Redirect(redirectUrl);
        }

    }
}
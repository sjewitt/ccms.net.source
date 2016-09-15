using System.ServiceModel;
using System.ServiceModel.Web;



using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccms.managers;
using ccms.utils;
using System.Web.Services;
using System;


namespace ccms
{
    [ServiceContract]
    public interface ILoginService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "login", ResponseFormat = WebMessageFormat.Json,RequestFormat=WebMessageFormat.Json)]
        LoginWrapper GetLogin(LoginWrapper loginWrapper);
    }
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LoginService" in code, svc and config file together.
    public class LoginService : ILoginService
    {
        public LoginWrapper GetLogin(LoginWrapper loginWrapper)
        {
            //do login logic here:
            //attempt login:
            User user = null;
            DBSession session = null;
            UserManager userManager = null;
            //HttpCookie sessionCookie = null;
            //HttpCookie sessionModeCookie = null;
            SessionManager sessionManager = null;

            user = null;
            session = new DBSession();
            userManager = new UserManager(session);
            sessionManager = new SessionManager(session);

            loginWrapper.Success = false;
            //check we can log in with the supplied creds:
            user = userManager.loginUser(loginWrapper);

            if (user != null)
            {
                loginWrapper.Success = true;
                loginWrapper.SessionKey = DateTime.Now.ToBinary().ToString();

                //call back to login page? This has Session on it...

            }
            return loginWrapper;    //POST to ccms.aspx? - reload on successful AJAX call?
        }
    }
}

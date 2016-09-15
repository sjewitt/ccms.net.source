using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccms.managers;

namespace ccms
{
    public partial class editor_create_page : System.Web.UI.Page
    {
        private ObjectFactory of;
        private Content content = null;
        private ContentInstance instance = null;

        protected PageManager pm = null;
        protected SessionManager sm = null;
        private NodeManager nm = null;
        private ContentManager cm = null;
        private UserManager um = null;
        protected User user = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //load content on page load
            this.of = new ObjectFactory();
            this.cm = of.getContentManager();
            this.nm = of.getNodeManager();
            this.um = of.getUserManager();
            this.pm = of.getPageManager();
            this.sm = new SessionManager(of.session);

            //get session cookie and mode cookie:
            HttpCookie sessionId = Request.Cookies["CCMSSession"];

            if (sm.checkSession(sessionId))
            {
                this.pnl_notLoggedIn.Visible = false;

                if (!IsPostBack)
                {
                    this.pnl_form.Visible = true;
                    this.pnl_done.Visible = false;

                    user = sm.getLoggedInUser(sessionId.Value);
                    //handle initial state
                }
                else
                {
                    //page_load after postback.
                }
                
                

                //get max page ID:
                int pageId = pm.getMaxPageId();
                this.TextBox1.Text = "NewPage-" + (pageId + 1);
            }
            else
            {
                //not logged in, show alert.
            }
        }
    }
}
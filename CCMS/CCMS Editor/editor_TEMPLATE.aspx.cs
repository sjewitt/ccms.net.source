using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccms.managers;

namespace ccms
{
    /// <summary>
    /// This is a template code block for pages that require different content for 
    /// authorised and non-authorised users. It also contains a check for initial
    /// load and subsequent postback so that for subnissions/re-population of fields
    /// etc. can be handled.
    /// </summary>
    public partial class editor_TEMPLATE : System.Web.UI.Page
    {
        //private ccms.Content currentContent;
        private ObjectFactory of;
        private Content content = null;
        private ContentInstance instance = null;


        protected PageManager pm = null;
        protected SessionManager sm = null;
        private NodeManager nm = null;
        private ContentManager cm = null;
        private UserManager um = null;
        protected User user = null;

        /// <summary>
        /// This is 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //load content on page load
            this.of = new ObjectFactory();
            this.cm = of.getContentManager();
            this.nm = of.getNodeManager();
            this.um = of.getUserManager();
            this.sm = new SessionManager(of.session);

            //get session cookie and mode cookie:
            HttpCookie sessionId = Request.Cookies["CCMSSession"];

            if (sm.checkSession(sessionId))
            {
                user = sm.getLoggedInUser(sessionId.Value);
                this.pnl_logged_in.Visible = true;
                this.pnl_not_logged_in.Visible = false;

                if (!IsPostBack)
                {
                    //handle initial state
                }
                else
                {
                    //page_load after postback.
                }
            }
            else
            {
                //not logged in, show alert.
                this.pnl_logged_in.Visible = false;
                this.pnl_not_logged_in.Visible = true;
            }
        }
    }
}
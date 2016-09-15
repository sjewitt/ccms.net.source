using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ccms.managers;
using ccms;
using ccms.utils;

namespace ccms
{
    public partial class dom_managenav : System.Web.UI.Page
    {
        public ObjectFactory of = null;
        public UserManager um = null;
        public User currentUser = null;
        public SessionManager sm = null;
        public DBSession session = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.session = new DBSession();
            ObjectFactory of = new ObjectFactory();

            this.sm = new SessionManager(this.session);
            this.um = of.getUserManager();

            //get session cookie and mode cookie:
            HttpCookie sessionId = Request.Cookies["CCMSSession"];

            if (sessionId != null && sm.checkSession(sessionId))
            {
                this.of = new ObjectFactory();
                this.um = of.getUserManager();
                //load admin as temp option here. Use cookie as for editor...
                this.currentUser = um.getCurrentUser(sessionId.Value);
            }
        }
    }
}
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
using ccms.managers;
using ccms.utils;
using System.Text.RegularExpressions;
using ccms;
using System.Collections.Generic;

namespace ccms
{
    public partial class editor_remove_content : System.Web.UI.Page
    {
        private DBSession session = null;
        private ObjectFactory of;
        private Content content = null;
        private ContentInstance instance = null;


        protected PageManager pm = null;
        protected SessionManager sm = null;
        private NodeManager nm = null;
        private IList<Node> nodeList;
        private ContentManager cm = null;
        private UserManager um = null;
        private User user = null;
        private int pageid = -1;
        private int slotid = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.session = new DBSession();
            of = new ObjectFactory();
            this.pm = of.getPageManager();
            this.sm = new SessionManager(this.session);
            
            //load current ContentTypes:
            this.cm = of.getContentManager();
            this.um = of.getUserManager();

            //get session cookie and mode cookie:
            HttpCookie sessionId = Request.Cookies["CCMSSession"];
            
            //set reload handler on button:

            if (sm.checkSession(sessionId))
            {
                if (Int32.TryParse(Request.QueryString["slotid"], out slotid) && Int32.TryParse(Request.QueryString["pageid"], out pageid))
                {
                    //TODO: Add name of content here

                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Int32.TryParse(Request.QueryString["slotid"], out slotid) && Int32.TryParse(Request.QueryString["pageid"], out pageid))
            {
                //TODO: Add name of content here
                //put on click event:
                PageContentMap map = new PageContentMap();
                map.contentId = -1;
                map.pageId = pageid;
                map.slotNum = slotid;
                if (pm.removeContentMapping(map))
                {
                    lbl_result.Text = "Mapping removed OK.";
                };
            }
        }
    }
}
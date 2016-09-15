//using System;
//using System.Collections.Generic;

//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//using ccms.managers;
//using ccms;
//using ccms.utils;

//namespace ccms
//{
//    public partial class editor_viewtree : System.Web.UI.Page
//    {
//        NodeManager nm = null;
//        DBSession session = null;
//        public EditUtils editUtils = null;
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            session = new DBSession();
//            nm = new NodeManager(session);
//            editUtils = new EditUtils();
//            //load the entire tree:
//            Node root = nm.getRootNode(true);
//            this.editUtils.buildViewtreeAsDOM(root);
//        }

//        protected void Button1_Click(object sender, EventArgs e)
//        {
//        }
//    }
//}
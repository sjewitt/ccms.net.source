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

namespace CCMS_editor
{
    public partial class edit_content : System.Web.UI.Page
    {
        private ccms.Content currentContent;
        private ObjectFactory of;
        private ContentManager cm;
        protected void Page_Load(object sender, EventArgs e)
        {
            //load content on page load
            this.of = new ObjectFactory();
        }
    }
}

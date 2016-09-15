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
    public partial class create_content : System.Web.UI.Page
    {
        private DBSession session = null;
        private ObjectFactory of = null;
        private string editContentUrl = "";
        private EditUtils editUtils;
        private ContentManager cm;
        private PageManager pm;
        private UserManager userManager = null;
        private SessionManager sm = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            /*
             * check for logged-in user by session cookie. We will use the user perms to determine the 
             * actions:
             * */
            this.session = new DBSession();
            of = new ObjectFactory();
            this.pm = of.getPageManager();
            this.sm = new SessionManager(this.session);
                       
            this.createcontent_editnew.Visible = false;
            this.editContentUrl = this.createcontent_editnew.NavigateUrl;
            this.editUtils = new EditUtils();
            
            //load current ContentTypes:
            this.cm = of.getContentManager();
            this.userManager = of.getUserManager();

            //get session cookie and mode cookie:
            HttpCookie sessionId = Request.Cookies["CCMSSession"];

            if (sm.checkSession(sessionId))
            {
                //enable panel:
                this.pnl_createContentForm.Visible = true;

                //disable not logged in message:
                this.pnl_notLoggedIn.Visible = false;

                //build dropdown of content types
                IList<ContentType> cTypes = this.cm.getContentTypes();
                IEnumerator enumerator = cTypes.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    ContentType _cType = (ContentType)enumerator.Current;
                    this.dropdown_contentType.Items.Add(new ListItem(_cType.Name, _cType.Id.ToString()));
                }

                //strip off params if found:
                if (this.editContentUrl.IndexOf("?") != -1)
                {
                    char[] splitOn = { '?' };
                    this.editContentUrl = this.editContentUrl.Split(splitOn)[0];
                }
            }
        }

        protected void newcontent_submit_Click(object sender, EventArgs e)
        {
            try
            {
                ContentManager contentManager = new ContentManager(this.session);
                Content newContent = new Content();
                newContent.authGroup = 0;
                newContent.contentType = 0;
                newContent.createdDate = DateTime.Now;
                newContent.createdUser = 1; //get from current user session
                newContent.Description = this.newcontent_description.Text;
                string pattern = "[0123456789]{4}-[0123456789]{2}-[0123456789]{2}"; //date string pattern

                Regex rgx = new Regex(pattern);

                if ( (rgx.Match(this.newcontent_validfrom.Text)).Success )
                {
                    newContent.StartDate = this.editUtils.getDateFromString(this.newcontent_validfrom.Text);
                }
               
                if ((rgx.Match(this.newcontent_validtill.Text)).Success)
                {
                    newContent.EndDate = this.editUtils.getDateFromString(this.newcontent_validtill.Text);
                }
 
                newContent.name = this.newcontent_name.Text;

                int newContentId = contentManager.createContentItem(newContent, "");
                
                if (newContentId > 0)
                {
                    //if we havecome from a slot, assign the new content to that slot:
                    int slotid = -1;
                    int pageid = -1;
                    if (Int32.TryParse(Request.QueryString["pageid"], out pageid) && Int32.TryParse(Request.QueryString["slotid"], out slotid))
                    {
                        PageContentMap map = new PageContentMap();
                        map.contentId = newContentId;
                        map.pageId = pageid;
                        map.slotNum = slotid;
                        pm.addContentMapping(map);
                    }
                    this.createcontent_statusmsg.Text = "Content '" + newContent.name + "' created OK";
                    this.createcontent_editnew.NavigateUrl = this.editContentUrl + "?contentid=" + newContentId;
                    this.createcontent_editnew.Visible = true;
                }
                else
                {
                    this.createcontent_statusmsg.Text = "Error creating new Content.";
                }
            }
            catch(Exception ex)
            {
                this.createcontent_statusmsg.Text = "Error creating new Content. (" + ex.Message + ")";
            }
        }

        protected void newcontent_validtill_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

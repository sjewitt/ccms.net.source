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

namespace CCMS_editor
{
    public partial class create_content : System.Web.UI.Page
    {
        private DBSession session = null;
        private string editContentUrl = "";
        private EditUtils editUtils;
        private ContentManager cm;

        protected void Page_Load(object sender, EventArgs e)
        {
            ObjectFactory of = new ObjectFactory();
            this.session = new DBSession();
            this.createcontent_editnew.Visible = false;
            this.editContentUrl = this.createcontent_editnew.NavigateUrl;
            this.editUtils = new EditUtils();

            //load current ContentTypes:
            this.cm = of.getContentManager();

            //build dropdown of content types
            IList<ContentType> cTypes = this.cm.getContentTypes();
            IEnumerator enumerator = cTypes.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                ContentType _cType = (ContentType)enumerator.Current;
                this.dropdown_contentType.Items.Add(new ListItem(_cType.Name,_cType.Id.ToString()));
            }

            //strip off params if found:
            if (this.editContentUrl.IndexOf("?") != -1)
            {
                char[] splitOn = { '?' };
                this.editContentUrl = this.editContentUrl.Split(splitOn)[0];
            }
        }

        protected void newcontent_submit_Click(object sender, EventArgs e)
        {
            try
            {
                ContentManager contentManager = new ContentManager(this.session);
                ccms.Content newContent = new ccms.Content();
                newContent.authGroup = 0;
                newContent.contentType = 0;
                newContent.createdDate = DateTime.Now;
                newContent.createdUser = 1; //get from current user session
                newContent.Description = this.newcontent_description.Text;
                string pattern = "[0123456789]{4}-[0123456789]{2}-[0123456789]{2}";

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
    }
}

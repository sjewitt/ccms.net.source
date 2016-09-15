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
using ccms;
using System.Collections.Generic;

namespace ccms
{
    public partial class edit_content : System.Web.UI.Page
    {
        //private ccms.Content currentContent;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            //load content on page load
            this.of = new ObjectFactory();
            this.cm = of.getContentManager();
            this.nm = of.getNodeManager();
            this.um = of.getUserManager();
            this.sm = new SessionManager(of.session);
            
            //********************************
            //TODO: Load user from session:
            //user = new ccms.User(1);   //TEMP!!!
            //********************************


            //get session cookie and mode cookie:
            HttpCookie sessionId = Request.Cookies["CCMSSession"];

            if (sm.checkSession(sessionId))
            {
                user = sm.getLoggedInUser(sessionId.Value);
                //TODO: Catch error if content ID does not exist
                //TODO: Properly compare if saved has ocurred before updating/changiong versions: 
                //load content object:
                Int32 contentid = 0;
                Int32 instanceid = 0;
                if (Int32.TryParse(Request.QueryString["contentid"], out contentid))
                {
                    //load the STATE dropdown:

                    content = cm.getContentItem(contentid);  //try/catch here
                    if (content != null)
                    {
                        instance = cm.getActiveInstance(content);

                        int currInstId = -1;
                        if (Int32.TryParse(Request.QueryString["instanceid"], out currInstId))
                        {
                            instance = cm.getInstance(content, currInstId);
                        }

                        instanceid = instance.versionId;

                        //populate the textarea with the content data on first load:
                        if (this.txt_content.Text.Length == 0)
                        {
                            this.txt_content.Text = instance.data;
                        }

                        //handle display of STATE:
                        //sel_state.ClearSelection();
                        if (!sel_state.SelectedValue.Equals(instance.state + ""))
                        {
                            int state = 0;
                            if (Int32.TryParse(this.sel_state.SelectedValue, out state))
                            {
                                instance.state = state;
                            }
                        }

                        int selectedIndex = -1;
                        for (int a = 1; a <= State.STATES.Length; a++)
                        {
                            ListItem _temp = new ListItem(State.STATES[a - 1], a + "");
                            if (instance.state == a)
                            {
                                selectedIndex = a - 1;
                                //_temp.Selected = true;
                            }
                            this.sel_state.Items.Add(_temp);
                        }
                        this.sel_state.SelectedIndex = selectedIndex;
                        //populate the instancelist:
                        IList<ContentInstance> instanceList = cm.getInstanceList(content);

                        //populate the instance dropdown (maybe do this differently?)

                        this.sel_instance.Items.Clear();
                        this.sel_instance.ClearSelection();

                        selectedIndex = -1;
                        for (int a = 0; a < instanceList.Count; a++)
                        {
                            ListItem _current = new ListItem();
                            ContentInstance _currInst = cm.getInstance(content, instanceList[a].versionId);
                            _current.Value = instanceList[a].versionId + "";
                            _current.Text = _currInst.updatedDate
                                    + " [version:" + _currInst.versionId
                                    + ", "
                                    + State.STATES[_currInst.state - 1]
                                    + "]";
                            //select the current instance if passed on querystring:
                            if (_currInst.versionId == instanceid)
                            {
                                selectedIndex = a;
                                // _current.Selected = true;
                            }
                            this.sel_instance.Items.Add(_current);
                        }
                        this.sel_instance.SelectedIndex = selectedIndex;
                    }
                }
            }
            else
            {
                //not logged in:
            }
        }

        public string getLinkList()
        {
            /*
             { title: "page2", value: "/ccms.aspx?nodeid=2" }
            */ 
            nodeList = nm.getNodes();
            string output = "";
            List<string> _temp = new List<string>();
            for (int i = 0; i < nodeList.Count;i++ )
            {
                _temp.Add("{title:\"" + nodeList[i].getPage().linkText + "\" ,value:\"ccms.aspx?nodeid=" + nodeList[i].id + "\"}");
            }
            output = string.Join(",",_temp.ToArray());

            //and add linklist to tinymce initialiser:
            return output; 
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //does stuff happen?
        }

        /// <summary>
        /// Update current content instance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //TODO: check the current user has perms to update the current instance - it may be that they don't...
        protected void btn_submitText_Click(object sender, EventArgs e)
        {
            try
            {
                instance.data = this.txt_content.Text;
                this.cm.updateInstance(instance);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void btn_newInstance_Click(object sender, EventArgs e)
        {
            ContentInstance _newInst = this.cm.createContentInstance(instance, State.ATWORK, user);
            if (_newInst != null)
            {
                //reload page:
                Response.Redirect(Request.ServerVariables["SCRIPT_NAME"]
                    + "?contentid="
                    + Request.QueryString["contentid"]
                    + "&instanceid="
                    + _newInst.versionId);
            };
        }

        protected void btn_setState_Click(object sender, EventArgs e)
        {
            instance.data = this.txt_content.Text;
            int state = 0;
            if(Int32.TryParse(this.sel_state.SelectedValue,out state))
            {
                instance.state = state;
            }
            if (this.cm.updateInstance(instance))
            {
                //reload page:
                Response.Redirect(Request.ServerVariables["SCRIPT_NAME"]
                    + "?contentid="
                    + Request.QueryString["contentid"]
                    + "&instanceid="
                    + Request.QueryString["instanceid"]);
            }
            //throw error here
        }
    }
}

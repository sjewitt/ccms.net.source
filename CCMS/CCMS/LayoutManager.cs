using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ccms.utils;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Collections;

namespace ccms.managers
{
    public class LayoutManager
    {
        public LayoutManager(DBSession session)
        {
            this.session = session;
            //this.templateBasePath = TemplateBasePath;
            this.of = new ObjectFactory();
        }

        //March 2016
        public LayoutManager(DBSession session,string TemplateBasePath)
        {
            this.session = session;
            this.TemplateBasePath = TemplateBasePath;
            this.of = new ObjectFactory();
        }

        public static string BASETEMPLATEPATH = "templates";
        private ObjectFactory of;
        private DBSession session;
        private string templateBasePath;

        public string TemplateBasePath
        {
            get { return templateBasePath; }
            set { templateBasePath = value; }
        }

        

        /// <summary>
        /// Returns the layout ID for the supplied node.
        /// If the node inherits from an ancestor, the 
        /// tree will be walked until the layout ID is
        /// found.
        /// 
        /// TODO: as per notes on CCMS.ASP
        /// </summary>
        /// <param name="node"></param>
        /// <returns>int layoutId</returns>
        public Layout getLayout(Node node)
        {
            try
            {
                int layoutId = 0;
                if (node.layoutId != 0)
                {
                    layoutId = node.layoutId;
                }
                else
                {
                    Node currNode = node;
                    NodeManager nm = of.getNodeManager();
                    while (layoutId == 0)
                    {
                        //get parent node:
                        currNode = nm.getParentNode(currNode);
                        layoutId = currNode.layoutId;
                    }
                }
                return this.getLayout(layoutId);
            }
            catch (Exception ex)
            {
                throw (new Exception("Cannot retrieve layout for node " + node.id + ": " + ex.Message));
                //return null;
            }
        }

        public Layout getLayout(int layoutId)
        {
            try
            {
                Layout layout = new Layout(layoutId,this.TemplateBasePath);

                //SqlConnection conn = new SqlConnection(this.session.dbConnStr);
                //conn.Open();

                //SqlDataReader reader = null;

                //SqlCommand cmd = new SqlCommand("select layout_url from layout where id = " + layoutId + ";", conn);
                //reader = cmd.ExecuteReader();
                //reader.Read();
                //this._sourcepath = ConfigurationManager.AppSettings["TemplateBasePath"] + reader.GetString(0);
                //this._id = layoutId;
                //reader.Close();
                //conn.Close();

                ////are we web or local FS:
                //this._deploymentType = ConfigurationManager.AppSettings["DeploymentType"];
                //if (this._deploymentType.Equals("web"))
                //{
                //    //call ASP.NET method to get the base path
                //}
                //else if (this._deploymentType.Equals("local"))
                //{
                //    //use the filepath as is:

                //}
                //else
                //{
                //    throw new Exception("cannot determine deployment configuration. Ensure that 'DeploymentType' is added as a key and 'local' or 'web' are set as the value.");
                //}

                ////and load the source
                ////load the file:
                //StreamReader streamReader = new StreamReader(this._sourcepath);
                //this._source = streamReader.ReadToEnd();
                //streamReader.Close();
               

                return layout;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Parse the template sourcecode to retrieve the Slot(s). Each slot has
        /// an ID, which maps to the slot ID held in the corresponding PageContentMap 
        /// object.
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        public List<Slot> getSlots(Layout layout, Node node)
        {
            /*
             I probably want a getSlots(Page page) method as well, incorporating the treewalk template 
             * retrieval method)
             */
            string pattern = "{CMS_CONTENT_[0-9]}";
            //string result = "";
            List<Slot> slots = new List<Slot>();
            Regex regexp = new Regex(pattern);

            MatchCollection slotTags = regexp.Matches(layout.source);

            /*
             for each matched string, extract the slot number. This matches with the slotNum from the 
             * PageContentMap
             */
            for (int a = 0; a < slotTags.Count; a++)
            {
                //result += slotTags[a].Value + "\r\n";
                slots.Add(new Slot(slotTags[a].Value, node.id));
            }
            return slots;
        }

        //public Template getTemplate(Template layout)
        //{
        //    return new Template(layout);
        //}

        //public Template getTemplate(Template layout,string absolutePath)
        //{
        //    return new Template(layout,absolutePath);
        //}

        //public Template getTemplate(Node node)
        //{
        //    Template layout = this.getLayout(node);
        //    return new Template(layout);
        //}

        /// <summary>
        /// Replace CCMS tokens in supplied Template with Content mapped 
        /// with PageContentMap.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="map"></param>
        /// <returns>The parsed template source</returns>

        public string getParsedLayoutSource(Layout layout, Node node, User loggedInUser, int renderMode, System.Web.UI.Page currentHTTPPage)
        {
            try
            {
                ObjectFactory of = new ObjectFactory();
                string htmlOutput = layout.source;

                //TODO: initialise these when the TemplateManager is initialised:
                Page page = (of.getPageManager()).getPage(node);
                User pageLastEditUser = (of.getUserManager()).getUser(page.updatedUser);

                //CORE PROPS:
                try
                {
                    //configure an array of these in the config file:
                    htmlOutput = htmlOutput.Replace("{CMS_CORE_TITLE}", page.title);
                    htmlOutput = htmlOutput.Replace("{CMS_CORE_DESCRIPTION}", page.description);
                    htmlOutput = htmlOutput.Replace("{CMS_CORE_KEYWORDS}", page.keywords);
                    htmlOutput = htmlOutput.Replace("{CMS_CORE_LINKTEXT}", page.linkText);
                    htmlOutput = htmlOutput.Replace("{CMS_CORE_DATE}", page.updatedDate.ToString());
                    htmlOutput = htmlOutput.Replace("{CMS_CORE_AUTHOR}", pageLastEditUser.fullName);
                    htmlOutput = htmlOutput.Replace("{CMS_LOGINSCRIPTS}", this.getLoginScripts()); //TEMP
                    htmlOutput = htmlOutput.Replace("{CMS_LOGINMASK}", "<div id=\"loginmask\" style=\"display:none;\">TEST</div>"); //TEMP
                    
                    if (loggedInUser == null)
                    {
                        htmlOutput = htmlOutput.Replace("{CMS_EDITLINKS}", ""); //TEMP
                        htmlOutput = htmlOutput.Replace("{CMS_EDITSCRIPTS}", ""); //placeholder for login dialogue
                    }
                    //TODO: Authorisation/editing stuff:
                    if (loggedInUser != null && renderMode == CCMSEngine.RENDERMODE_EDIT)
                    {
                        //do stuff here around rendering the edit links and any authenticated user stuff:
                        htmlOutput = htmlOutput.Replace("{CMS_EDITLINKS}", this.getEditLinks(loggedInUser, node, currentHTTPPage)); //TEMP
                        
                        //insert edit scripts, JQuery, edit CSS etc.
                        htmlOutput = htmlOutput.Replace("{CMS_EDITSCRIPTS}", 
                            "<script type=\"text/javascript\" src=\"/scripts/common_functions.js\" />\n"
                            ); //TEMP
                    }
                    if (loggedInUser != null && renderMode == CCMSEngine.RENDERMODE_SESSION)
                    {
                        htmlOutput = htmlOutput.Replace("{CMS_EDITLINKS}", "Session mode for "+loggedInUser.fullName); //TEMP
                        htmlOutput = htmlOutput.Replace("{CMS_EDITSCRIPTS}", ""); //
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error mapping placeholders: " + ex.Message + ". Current result: " + htmlOutput);
                }
                //get the PageContentMap List
                IList<PageContentMap> map = (of.getContentManager()).getPageContentMap(page);

                //get the Slot List
                IList<Slot> slots = this.getSlots(layout, node);

                //iterate over the Slot array, replacing the CCMS tag with the corresponding Content
                //as mapped:
                ContentInstance currContentInstance;
                string currText;

                /*
                 Map content slot to content via SlotContentMap.
                 * NOTE: If there is not content, there is no map entry.
                 * Therefore, we must account for this.
             
                 */
                bool mapFound = false;
                /*
                 Iterate over the slot array:
                 */
                try
                {
                    for (int a = 0; a < slots.Count; a++)
                    {
                        mapFound = false;

                        /*
                         for each slot, check if there is a matching content item in the map array:
                         */
                        for (int b = 0; b < map.Count; b++)
                        {
                            /*
                             if mapping found, replace the slot tag with whatever the matching content is:
                             */
                            if (slots[a].slotId == map[b].slotNum)
                            {
                                mapFound = true;
                                currContentInstance = (of.getContentManager()).getActiveInstance((of.getContentManager()).getContentItem(map[b].contentId));
                                //currText = (of.getContentManager()).getActiveInstance((of.getContentManager()).getContentItem(map[b].contentId)).data;
                                currText = currContentInstance.data;
                                if (currText == null) currText = "";
                                htmlOutput = htmlOutput.Replace(slots[a].slotTag, this.getSlotContentEditOptions(loggedInUser, slots[a], currContentInstance, node, renderMode) + currText);
                            }
                        }
                        /*
                         If no mapping is found, replace slot tag with empty string:
                         */
                        if (!mapFound)
                        {
                            //htmlOutput = htmlOutput.Replace(slots[a].slotTag, "[empty]");
                            htmlOutput = htmlOutput.Replace(slots[a].slotTag, this.getSlotContentEditOptions(loggedInUser, slots[a], (new ContentInstance()), node, renderMode));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("slot processing error: " + ex.StackTrace);
                }

                //CUSTOMISED LAYOUT:
                //handled at the front-end


                return htmlOutput;
            }
            catch (Exception ex)
            {
                return("Error caught: " + ex.StackTrace);
            }
        }



        /**
         * Get the edit links appropriate to the logged-in user:
         */
        private string getEditLinks(User loggedInUser,Node currentNode,System.Web.UI.Page currentHTTPPage)
        {
            {
                try
                {
                    string editBannerStr = "";

                    if (loggedInUser != null && loggedInUser.permissions > 0)
                    {
                        //int nodeid = 0;
                        //string param = "?";
                        //  if(Request.QueryString("nodeid") && (new String(Request.QueryString("nodeid"))) != "undefined"){
                        //    nodeid = new String(Request.QueryString("nodeid"));
                        //    param = "?nodeid=" + nodeid + "&";
                        //  }
                        editBannerStr = "<link rel=\"stylesheet\" type=\"text/css\" href=\"/ccms.editor/styles/editstyles.css\" />\n";
                        editBannerStr += "<script type=\"text/javascript\" src=\"/ccms.editor/Scripts/editscripts.js\"></script>";
                        editBannerStr += "<div id=\"editbanner\">\n";

                        //  //generate javascript handlers:
                        //  //editBannerStr += this.getActionsJavascript(page);
                        editBannerStr += "<p id=\"userdisplay\">" + loggedInUser.fullName + "  [<a href=\"" + currentHTTPPage.Request["SCRIPT_NAME"] + "?loginmode=logout\">logout</a>]</p>";

                        //  //add options here depending on permissions:

                        ArrayList editOptions = new ArrayList();
                        string url = "";
                        if ((loggedInUser.permissions & Permissions.ADMINISTRATOR) > 0)
                        {
                            url = "/ccms_asp/admin/admin.aspx"; //URL to appropriate edit page
                            editOptions.Add("\t<span class=\"editoption\"><a title=\"User management, permissions management, content subtype management. \" href=\"#\" onclick=\"popup('" + url + "')\">ADMIN</a></span>\n");
                        }
                        if ((loggedInUser.permissions & Permissions.ADMINISTRATOR)>0 || (loggedInUser.permissions & Permissions.UPLOADBINARY)>0)
                        {
                            url = "/ccms_asp/editor/uploadbinary.asp";
                            editOptions.Add("\t<span class=\"editoption\"><a title=\"Upload images and cocuments.\" href=\"#\" onclick=\"popup('" + url + "')\">Add binaries</a></span>\n");
                        }
                        if ((loggedInUser.permissions & Permissions.CREATEPAGE) > 0 || (loggedInUser.permissions & Permissions.ADMINISTRATOR)>0)
                        {  //only allow this if has Permissions.EDITVIEWTREE?
                            url = "/ccms_asp/editor/createpage.asp";
                            editOptions.Add("\t<span class=\"editoption\"><a title=\"Create a new page and optionally assign it to a viewtree position\" href=\"#\" onclick=\"popup('" + url + "')\">Create Page</a></span>\n");
                          }
                        if ((loggedInUser.permissions & Permissions.EDITVIEWTREE)>0 ||( loggedInUser.permissions & Permissions.ADMINISTRATOR)>0)
                        {  //only allow this if has Permissions.EDITVIEWTREE?
                            url = "/ccms_asp/editor/managepages.asp";
                            editOptions.Add("\t<span class=\"editoption\"><a title=\"Lists all pages and offers options to remove, edit, add to viewtree.\" href=\"#\" onclick=\"popup('" + url + "')\">Manage Pages</a></span>\n");
                        }
                        if ((loggedInUser.permissions & Permissions.EDITVIEWTREE)>0 || (loggedInUser.permissions & Permissions.ADMINISTRATOR)>0)
                        {  //only allow this if has Permissions.EDITVIEWTREE?
                            url = "/ccms_asp/editor/assignlayouts.asp?nodeid=" + currentNode.id;
                            editOptions.Add("\t<span class=\"editoption\"><a title=\"Assign layout to viewtree.\" href=\"#\" onclick=\"popup('" + url + "')\">Assign Layouts</a></span>\n");
                        }

                        if ((loggedInUser.permissions & Permissions.EDITPAGE)>0 || (loggedInUser.permissions & Permissions.ADMINISTRATOR)>0)
                        {  //only allow this if has Permissions.EDITVIEWTREE?
                            url = "/ccms_asp/editor/managenavigation.asp?nodeid=" + currentNode.id;
                            editOptions.Add("\t<span class=\"editoption\"><a title=\"Manage ordering of pages etc.\" href=\"#\" onclick=\"popup('" + url + "')\">Manage Navigation</a></span>\n");
                            //opens siblingorder.asp
                        }

                        if ((loggedInUser.permissions & Permissions.CREATECONTENT)>0 || (loggedInUser.permissions & Permissions.ADMINISTRATOR)>0)
                        {
                            url = "/ccms_asp/editor/createcontent.asp";
                            editOptions.Add("\t<span class=\"editoption\"><a title=\"Create content.\" href=\"#\" onclick=\"popup('" + url + "')\">Create content</a></span>\n");
                        }
                        //MANAGE EXISTING CONTENT:
                        if ((loggedInUser.permissions & Permissions.EDITCONTENT)>0 || (loggedInUser.permissions & Permissions.ADMINISTRATOR)>0)
                        {
                            url = "/ccms_asp/editor/managecontent.asp";
                            editOptions.Add("\t<span class=\"editoption\"><a title=\"Manage content.\" href=\"#\" onclick=\"popup('" + url + "')\">Manage content</a></span>\n");
                        }

                        if ((loggedInUser.permissions & Permissions.EDITPAGE)>0 || (loggedInUser.permissions & Permissions.ADMINISTRATOR)>0)
                        {
                            url = "/ccms_asp/editor/pageproperties.asp?pageid=" + currentNode.pageId;
                            editOptions.Add("\t<span class=\"editoption\"><a title=\"Edit the core properties of a page.\" href=\"#\" onclick=\"popup('" + url + "')\">Edit Page Properties</a></span>\n");
                        }
                        if ((loggedInUser.permissions & Permissions.DELETEPAGE)>0 || (loggedInUser.permissions & Permissions.ADMINISTRATOR)>0)
                        {
                            url = "/ccms_asp/editor/removeviewtreebranch.asp?nodeid=" + currentNode.id;
                            editOptions.Add("\t<span class=\"editoption\"><a title=\"Remove current page from viewtree.\" href=\"#\" onclick=\"popup('" + url + "')\">Remove page</a></span>\n"); //make not visible? Hmmm...
                            //NOTE: This is quite a complex SQL - it needs to recurse down to all childs as well! - ie walk the viewtree from HERE and generate SQL for each 
                            //entry. A TRANSACTION should then occur - perhaps flagging as inactive?
                        }
                        string[] editOptsArray = (string[])editOptions.ToArray(typeof(string));
                        editBannerStr += String.Join("|", editOptsArray);
                        //And render a logout link:
                        //editBannerStr += "<a href=\"" + currentHTTPPage.Request["SCRIPT_NAME"]  + "?loginmode=logout\">logout</a>";
                        editBannerStr += "</div>\n";

                    }
                    return (editBannerStr);
                }
                catch (Exception ex)
                {
                    return (ex.ToString());
                }
            }
        }
        /*
         * Get the dropdown and associated client-side javaswcript for each slot on the template.
         * determine the options based on the current user perms and 
         * whether or not there is any content in the slot:
         */
        private string getSlotContentEditOptions(User loggedInUser,Slot currentSlot,ContentInstance currentContentInstance,Node currentNode,int currentMode)
        {
            try
            {
                string SlotEditOutput = "";
                if (loggedInUser != null && currentMode == CCMSEngine.RENDERMODE_EDIT)
                {
                    //if (currentContentInstance == null)
                    SlotEditOutput = "\n\n<!-- BEGIN EDIT CODE FOR USED SLOT " + currentSlot.slotId + " \nSLOT " + currentSlot.slotId + ", CONTENT " + currentContentInstance.contentId + "\n-->\n";
                    SlotEditOutput += this.getEditJavascript(currentSlot.slotId)+"\n";
                    SlotEditOutput += "<form name=\"editslot" + currentSlot.slotId + "\" action=\"\" class=\"editdropdown\">\n";
                    SlotEditOutput += "<input type=\"hidden\" name=\"slotnum\" value=\"" + currentSlot.slotId + "\" />";
                    SlotEditOutput += "<input type=\"hidden\" name=\"contentid\" value=\"" + currentContentInstance.contentId + "\" />";
                    SlotEditOutput += "<input type=\"hidden\" name=\"pageid\" value=\"" + currentNode.getPage().id + "\" />\n";
                    SlotEditOutput += "<select name=\"action\">\n";
                    SlotEditOutput += "<option> - Choose Action: - </option>\n";

                    /*
                    BIG TODO: 
                    Sort out permission patterns here:
                    */
                    //if there is content offer options to edit, replace or remove:
                    if (currentContentInstance.contentId > 0)
                    {
                        if ((loggedInUser.permissions & Permissions.EDITCONTENT) > 0 || (loggedInUser.permissions & Permissions.ADMINISTRATOR) > 0)
                        {
                            SlotEditOutput += "<option value=\"edit\">Edit current content</option>\n";
                        }
                        if ((loggedInUser.permissions & Permissions.EDITPAGE) > 0 || (loggedInUser.permissions & Permissions.ADMINISTRATOR) > 0)
                        { 
                            SlotEditOutput += "<option value=\"remove\">Remove content</option>\n";
                            SlotEditOutput += "<option value=\"replace\">Replace content</option>\n";
                        }
                        if ((loggedInUser.permissions & Permissions.CREATECONTENT) > 0 || (loggedInUser.permissions & Permissions.ADMINISTRATOR) > 0)
                        { 
                            SlotEditOutput += "<option value=\"contentproperties\">Properties</option>\n";
                        }
                    }

                    //otherwise offer options to create new or add existing
                    else
                    {
                        if ((loggedInUser.permissions & Permissions.CREATECONTENT)>0 || (loggedInUser.permissions & Permissions.ADMINISTRATOR)>0)
                        {
                            SlotEditOutput += "<option value=\"create\">Create new content</option>\n";
                        }
                        if ((loggedInUser.permissions & Permissions.EDITPAGE) > 0 || (loggedInUser.permissions & Permissions.ADMINISTRATOR)>0)
                        {
                            SlotEditOutput += "<option value=\"add\">Add existing content</option>\n";
                        }
                    }

                    SlotEditOutput += "</select>\n";
                    SlotEditOutput += "<input type=\"button\" value=\"ok\" onclick=\"doedit" + currentSlot.slotId + "(document.forms.editslot" + currentSlot.slotId + ");\" />\n";
                    SlotEditOutput += "</form>\n";
                    SlotEditOutput += "<!-- END EDIT CODE FOR SLOT " + currentSlot.slotId + " -->\n\n";
                }
                return SlotEditOutput;
            }
            catch (Exception ex)
            {
                return(RenderUtils.getErrorPage(currentNode,ex));
            }
        }

        //handle slot-editing options:
        private string getEditJavascript(int slotId)
        {
            try
            {
                //get root of editor javascript library:
                string js_root = "ccms.editor/";//ConfigurationManager.AppSettings["EditorRoot"];
                
                //JS handler:
                string output = "";
                output += "<script type=\"text/javascript\">\n";
                output += "function doedit" + slotId + "(form){\n";
                output += "\tvar slotId = form.slotnum.value;\n";
                output += "\tvar contentId = form.contentid.value;\n";
                output += "\tvar pageId = form.pageid.value;\n";
                output += "\tvar action = form.action[form.action.selectedIndex].value;\n";

                /*
                 * TODO: Sort out action flags for content and page properties
                 */

                //trigger the edit content window:
                output += "\tif(action == 'edit')popup('" + js_root + "editor_edit_content.aspx?contentid=' + contentId);\n";

                //trigger the content properties window:
                output += "\tif(action == 'props')popup('" + js_root + "editor_content_props.aspx?contentid=' + contentId);\n";

                //trigger the replace content window:
                output += "\tif(action == 'replace')popup('" + js_root + "editor_assign_content.aspx?slotid=' + slotId + '&pageid=' + pageId + '&doaction=replace');\n";

                //trigger the create content window:
                output += "\tif(action == 'create') popup('" + js_root + "editor_create_content.aspx?slotid=' + slotId + '&pageid=' + pageId);\n";

                //trigger the clear slot method call:
                output += "\tif(action == 'remove') popup('" + js_root + "editor_remove_content.aspx?slotid=' + slotId + '&pageid=' + pageId + '&doaction=remove');\n";

                //trigger the clear slot method call:
                output += "\tif(action == 'add') {popup('" + js_root + "editor_assign_content.aspx?slotid=' + slotId + '&pageid=' + pageId + '&doaction=add');\n}";

                //07.11.09: trigger edit content properties box:
                output += "\tif(action == 'contentproperties')popup('" + js_root + "editor_content_properties.aspx?contentid=' + contentId);\n";

                output += "}\n";
                output += "</script>\n";
                return(output);
            }
            catch(Exception ex)
            {
                return("");
            }
        }

        /*
         * render login JQuery stuff into {CMS_LOGINSCRIPTS}
         * Just the javascript include for login mask overlay
         */
        private string getLoginScripts()
        {
            string _out = "<script type=\"text/javascript\" src=\"/scripts/jquery-1.10.2.js\"></script>\n"
                            + "<script type=\"text/javascript\" src=\"/scripts/jquery-ui-1.10.4.js\"></script>\n"
                            + "<link type=\"text/css\" rel=\"stylesheet\" href=\"/styles/jquery-ui-1.10.4.css\" />\n"
                            + "<script type=\"text/javascript\" src=\"/scripts/loginmask.js\"></script>\n";
            return _out;
        }
    }
}

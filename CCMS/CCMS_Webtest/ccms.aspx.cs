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
using ccms;
using ccms.utils;
using System.Collections.Generic;
using ccms.managers;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace ccms
{
    public partial class CCMSRenderer : System.Web.UI.Page
    {
        User user = null;
        DBSession session = null;
        UserManager userManager = null;
        HttpCookie sessionCookie = null;
        HttpCookie sessionModeCookie = null;
        SessionManager sessionManager = null;
        LoginWrapper loginWrapper = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            //we need this in all cases:
            CCMSEngine engine = new CCMSEngine();


            if (Request.QueryString["calltype"] != null)
            {
                /*
                 * trigger REST data response
                 */
                int contentId;
                if (Int32.TryParse(Request.QueryString["contentid"], out contentId))
                {
                    switch (Request.QueryString["calltype"])
                    {
                        case "xml":
                            //http://stackoverflow.com/questions/4123590/serialize-an-object-to-xml
                             XmlSerializer xsSubmit = new XmlSerializer(typeof(ContentInstance));
                             var subReq = engine.contentManager.getActiveInstance(engine.contentManager.getContentItem(contentId));
                             using(StringWriter sww = new StringWriter())
                             using(XmlWriter writer = XmlWriter.Create(sww))
                             {
                                 xsSubmit.Serialize(writer, subReq);
                                 var xml = sww.ToString(); // Your XML
                                 Response.AddHeader("Content-type", "text/xml");
                                 Response.Write(xml);
                             }
                             
                            break;

                        case "json":
                            //see http://stackoverflow.com/questions/6201529/turn-c-sharp-object-into-a-json-string-in-net-4
                            var json = new JavaScriptSerializer().Serialize(engine.contentManager.getActiveInstance(engine.contentManager.getContentItem(contentId)));
                            Response.AddHeader("Content-type", "application/json");
                            Response.Write(json);
                            break;

                            /*
                        case "raw":
                            ContentInstance i = engine.contentManager.getActiveInstance(engine.contentManager.getContentItem(contentId))
                            break;
                        */

                    }
                }

            }
            else
            {
                /*
                 Handle login
                 */
                user = null;
                session = new DBSession();
                userManager = new UserManager(session);
                sessionManager = new SessionManager(session);

                //is a session already present?
                sessionCookie = Request.Cookies["CCMSSession"];
                //handle logout request: in login.cs
                //if (Request.QueryString["loginmode"] != null && Request.QueryString["loginmode"].Equals("logout"))
                //{
                //    this.logout(Request.QueryString["redirecturl"]);
                //}


                if (sessionCookie != null)
                {
                    //the user **MIGHT** be null at this point if a logout request has already been processed:
                    user = sessionManager.getLoggedInUser(sessionCookie.Value);
                    if (user != null)
                    {

                    }
                }

                string htmlOutput;
                int currentRenderMode = CCMSEngine.RENDERMODE_ANONYMOUS; //anonymous
                int nodeId;
                Node node;
                User currentUser = null;
                try
                {
                    bool loginDetected = false;

                    foreach (String key in Request.QueryString.AllKeys)
                    {
                        try
                        {
                            if (key.Equals("loginmode"))
                            {
                                loginDetected = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            //catch bad parameters on each loop.
                        }
                    }

                    if (loginDetected)
                    {
                        //Response.Redirect("login.aspx?"+Request.QueryString + "&redirecturl=" + HttpUtility.UrlEncode(Request.ServerVariables["URL"]),false);   //todo - keep current URL parameter so we can open the correct page
                    }

                    //do we have POST login data?
                    if (Request.Form["Username"] != null)
                    {
                        loginWrapper = new LoginWrapper();
                        loginWrapper.Username = Request.Form["Username"];
                        loginWrapper.Password = Request.Form["Password"];   //could be empty
                        user = userManager.loginUser(loginWrapper);

                        if (user != null)
                        {
                            //generate the cookie:
                            sessionCookie = new HttpCookie("CCMSSession");
                            sessionCookie.Value = Session.SessionID;

                            loginWrapper.SessionKey = Session.SessionID;

                            sessionModeCookie = new HttpCookie("CCMSSessionMode");
                            sessionModeCookie.Value = Request.QueryString["loginmode"];

                            Response.Cookies.Add(sessionCookie);
                            Response.Cookies.Add(sessionModeCookie);

                            sessionManager.createSession(user, Session.SessionID);
                            Response.Redirect(Request.QueryString["redirecturl"]);

                        }
                        else
                        {

                        }
                    }


                    //CCMSEngine engine = new CCMSEngine();
                    sessionCookie = Request.Cookies["CCMSSession"];
                    //TODO: Must kill cookie as it persists!
                    if (sessionCookie != null)
                    {
                        currentUser = (new SessionManager((new DBSession()))).getLoggedInUser(sessionCookie.Value);
                        //CCMSSessionMode
                        HttpCookie sessionModeCookie = Request.Cookies["CCMSSessionMode"];
                        if (sessionModeCookie.Value.Equals("edit"))
                        {
                            currentRenderMode = CCMSEngine.RENDERMODE_EDIT;      //edit
                        }
                        if (sessionModeCookie.Value.Equals("session"))
                        {
                            currentRenderMode = CCMSEngine.RENDERMODE_SESSION;      //session/personalisation
                        }

                    }

                    //Get current node, or assume root if param not found:
                    if (Int32.TryParse(Request.QueryString["nodeid"], out nodeId))
                    {
                        node = new Node(nodeId);
                    }
                    else
                    {
                        node = engine.nodeManager.getRootNode();
                    }

                    htmlOutput = engine.renderNode(node, currentUser, currentRenderMode, this);
                    InsertCustomisedLayout insertCustomisedLayout = new InsertCustomisedLayout(engine.objectFactory, node);
                    htmlOutput = insertCustomisedLayout.get(htmlOutput);
                    Response.Write(htmlOutput);
                }
                catch (Exception ex)
                {
                    htmlOutput = "Error loading page:" + ex.StackTrace;
                    Response.Write(htmlOutput);
                }
            }
        }
    }

    /*
     Add customised rendering here
     */
    public class InsertCustomisedLayout
    {
        private ObjectFactory objectFactory = null;
        //private SqlConnection conn = null;
        //private SqlDataReader reader = null;

        //Stuff that is useful for navs:
        private Node root;
        private Node node;  //current page

        //nav path/sibling arrays
        //private Node[] nodelist1 = null;
        //private Node[] nodelist2 = null;
        //private Node[] nodelist3 = null;
        private Node[] path;

        //private NodeManager nm = null;
        private ContentManager cm = null;

        public InsertCustomisedLayout(ObjectFactory objectFactory, Node currentNode)
        {


            this.objectFactory = objectFactory;
            //this.nm = objectFactory.getNodeManager();
            this.cm = new ContentManager(this.objectFactory.session);

            //
            this.root = objectFactory.getNodeManager().getRootNode();
            this.node = currentNode;
            this.path = objectFactory.getNodeManager().getBreadcrumb(currentNode);
        }

        /*
         * entry point:
         * Basic methodology:
         * 
         * htmlSource = htmlSource.Replace("{YOUR_TAG}",your_string);
         * 
         * 'your_string' is the arbitrary result of a method call. This could be a navigation output,
         * a personalisation method or a more complex database routine.
         */

        public string get(string htmlSource)
        {
            //DEMO
            //main nav, top:
            htmlSource = htmlSource.Replace("{CMS_DEMO_LHNAV}", this.getDemoLHNav(this.root, this.node));

            //subnav, RH Side:
            htmlSource = htmlSource.Replace("{CMS_DEMO_SUBNAV}", this.getDemoSubnav(this.node));

            //breadcrumb, top:
            htmlSource = htmlSource.Replace("{CMS_DEMO_BREADCRUMB}", this.getDemoBreadcrumb(node));

            //news:
            int newsId = Int32.Parse(ConfigurationManager.AppSettings["newsPageID"]);
            htmlSource = htmlSource.Replace("{CMS_DEMO_NEWS}", getDemoNews(2, newsId));

            //Shaldon
            //htmlSource = htmlSource.Replace("{CMS_LATEST_NEWS_HOME}", this.getShaldonHomeNews(1, 17));
            //htmlSource = htmlSource.Replace("{CMS_CUSTOM_TOPNAV_HOME}", this.getShaldonL1Nav(this.root.getChildren(),""));
            //htmlSource = htmlSource.Replace("{CMS_CUSTOM_TOPNAV_BLUE}", this.getShaldonL1Nav(nodelist1, "blue"));
            //htmlSource = htmlSource.Replace("{CMS_CUSTOM_TOPNAV_GREEN}", this.getShaldonL1Nav(nodelist1, "green"));
            //htmlSource = htmlSource.Replace("{CMS_CUSTOM_TOPNAV_BROWN}", this.getShaldonL1Nav(nodelist1, "brown"));
            //htmlSource = htmlSource.Replace("{CMS_CUSTOM_L2NAV}",this.getShaldonL2Nav(nodelist2));

            //set up path/level intersects so we know where we are in the nav:
            //if (path.Length > 1) { nodelist1 = objectFactory.getNodeManager().getSiblings(path[1]); nodelist2 = node.getChildren(); }
            //if (path.Length > 2) { nodelist2 = objectFactory.getNodeManager().getSiblings(path[2]); nodelist3 = node.getChildren(); }
            //if (path.Length > 3) { nodelist3 = objectFactory.getNodeManager().getSiblings(path[3]); }

            //COVEN
            //htmlSource = htmlSource.Replace("{CMS_COVEN_HOMENAV}", this.getCovenHomeNav(root.getChildren()));
            //htmlSource = htmlSource.Replace("{CMS_COVEN_SECTIONHEADING}", this.getCovenSectionHeading(node, path));
            //htmlSource = htmlSource.Replace("{CMS_COVEN_LHNAV}", this.getCovenLHNav(node, path, root));
            //htmlSource = htmlSource.Replace("{CMS_COVEN_SECTIONIMAGE}", this.getCovenSectionImage(node, path));
            //htmlSource = htmlSource.Replace("{CMS_COVEN_RHNAV}", this.getCovenRHNav(node));
            return htmlSource;
        }

        //add any custom methods here, and refer to them in the get() method

        private string getDemoLHNav(Node startNode, Node currentNode)
        {
            string outStr = "";
            try
            {
                outStr = "<ul class=\"box\">";
                Node[] childs = startNode.getChildren();

                //add Home to start:
                ArrayList _temp = new ArrayList(childs);
                _temp.Reverse();
                _temp.Add(startNode);
                _temp.Reverse();
                //childs = (Node[])_temp.ToArray();

                for (int a = 0; a < _temp.Count; a++)
                {
                    Node _node = (Node)_temp[a];
                    string CSSModifier = "";
                    if (_node.id == currentNode.id)
                    {
                        CSSModifier = " class=\"nav-active\"";
                    }
                    Page page = new Page(_node.pageId);
                    outStr += "<li" + CSSModifier + "><a href='/ccms.aspx?nodeid=" + _node.id + "'>" + page.linkText + "</a>";
                    outStr += "</li>\n";

                }
                outStr += "</ul>";
                return outStr;
            }
            catch (Exception ex)
            {
                outStr = "Error occurred: " + ex.Message;
                return outStr;
            }
        }

        private string getDemoSubnav(Node currentNode)
        {
            Node[] childs = currentNode.getChildren();
            string _out = "<ul class=\"ul-style01\">";
            for (int a = 0; a < childs.Length; a++)
            {
                _out += "<li><a href=\"ccms.aspx?nodeid=" + childs[a].id + "\">" + childs[a].getPage().linkText + "</li>";
            }
            _out += "</ul>";
            return _out;
        }

        private string getDemoBreadcrumb(Node currentNode)
        {
            string _out = "";
            for (int a = 1; a < this.path.Length; a++)  //don't want index
            {
                _out += "<a href=\"ccms.aspx?nodeid=" + this.path[a].id + "\">" + this.path[a].getPage().linkText + "</a>";
                if (a < this.path.Length - 1)
                {
                    _out += "&nbsp;>&nbsp;";
                }
            }
            return _out;
        }

        private string getDemoNews(int newsSubtype, int newsDisplayPageId)
        {
            ArrayList newsList = this.cm.getContentItems(2); //get news subtype
            //IEnumerator enumerator = newsList.GetEnumerator();
            Content _curr = null;
            string output = "<dl id=\"news\">";

            /*
             * create HTML for news output block:
                     <dt><a href="#">Lorem ipsum dolor sit amet</a></dt>
                     <dd><span class="date">15. 01.</span>[summary]</dd>        
             */
            if (newsList != null)
            {
                for (int a = 0; a < newsList.Count; a++)
                {
                    _curr = (Content)newsList[a];
                    output += "<dt><a href=\"ccms.aspx?nodeid=" + newsDisplayPageId + "&contentid=" + _curr.id + "\">" + _curr.name + "</a></dt>"
                        + "<dd><span class=\"date\">&nbsp;" + getNewsFormattedDate(_curr.StartDate) + "</span>"
                        + _curr.Description + "</dd>";
                }
            }
            output += "</dl>";

            return output;
        }

        private string getNewsFormattedDate(DateTime _in)
        {
            string _out = _in.Day + ". " + _in.Month + ".";//sort out
            return _out;
        }

        /*
         COVEN
         */
        //private string getCovenHomeNav(Node[] nodeList)
        //{
        //    string output = "";
        //    try
        //    {
        //        Page currPage;
        //        string prefix = "<div class=\"homepagethumbs\"><div class=\"thumbheader\">";
        //        string suffix = "</a></div>\n";
        //        for (int a = 0; a < nodeList.Length;a++ )
        //        {
        //            currPage = new Page(nodeList[a].pageId);
        //            output += prefix 
        //                  + currPage.linkText 
        //                  + "</div><a href=\"\\ccms.aspx?nodeid="
        //                  + nodeList[a].id 
        //                  + "\" title=\"" 
        //                  + currPage.linkText 
        //                  + "\"><img src=\"/images/" 
        //                  + currPage.name
        //                  + "_home.jpg\" alt=\"" + currPage.linkText + "\" />" 
        //                  + suffix;
        //        }
        //        return output;
        //    }
        //    catch (Exception ex)
        //    {
        //        output = "Error occurred: " + ex.Message;
        //        return output;
        //    }
        //}

        //private string getCovenSectionHeading(Node node,Node[] path)
        //{
        //    try
        //    {
        //        string output = "";
        //        Page page;
        //        if(path.Length > 0)
        //        {
        //            for(int a=0;a<path.Length;a++)
        //            {
        //                if(path[a].level == 1)
        //                {
        //                    page = new Page(path[a].pageId);
        //                    output = page.linkText;
        //                }
        //            }
        //        }
        //        return output;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //private string getCovenLHNav(Node node, Node[] path, Node root)
        //{
        //    string output = "";
        //    try
        //    {

        //        if(node.level > root.level)
        //        {
        //            Node[] childs    = root.getChildren();
        //            Node sectionNode = null;

        //            //determine section:
        //            for(int a=0;a<path.Length;a++)
        //            {
        //                if(path[a].level == 1)
        //                {
        //                    sectionNode = path[a];
        //                }
        //            }

        //            if (sectionNode != null)
        //            {
        //                Page page;
        //                for (int a = 0; a < childs.Length; a++)
        //                {
        //                    if (childs[a].id != sectionNode.id)
        //                    {
        //                        page = new Page(childs[a].pageId);
        //                        output += "<div class='thumbnav'>" + page.linkText + "</div><a href='/ccms.aspx?nodeid=" + childs[a].id + "'><img src='/images/" + page.name + "_sml.jpg' alt='" + page.linkText + "' /></a>\n";
        //                    }
        //                }
        //            }
        //        }
        //        return output;
        //    }
        //    catch (Exception ex)
        //    {
        //        return output;
        //    }
        //}

        //private string getCovenSectionImage(Node node,Node[] path)
        //{
        //    try
        //    {
        //        string output = "";
        //        Page page;
        //        string linkStart = "";
        //        string linkEnd = "";

        //        if(path.Length > 1)
        //        {
        //            for(int a=0;a<path.Length;a++)
        //            {
        //                if(path[a].level == 1)
        //                {
        //                    page = new Page(path[a].pageId);
        //                    if(node.level >= 2)
        //                    {
        //                        linkStart = "\n<a href=\"/ccms.aspx?nodeid=" + path[a].id + "\" title=\"" + page.linkText + "\">\n\t";
        //                        linkEnd = "</a>";
        //                    }

        //                    output = "<div class=\"thumbheader\">\n\t" + page.linkText;
        //                    output += linkStart + "<img src='/images/" + page.name + "_lge.jpg' alt='" + page.linkText + "' width='110' height='120' />\n";
        //                    output += linkEnd + "</div>\n";
        //                }
        //            }
        //        }
        //        return output;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //private string getCovenRHNav(Node node)
        //{
        //    try
        //    {
        //        string output = "";
        //        int a = 0;
        //        Node[] l2Sibs;
        //        Page currPage = null;

        //        //get the direct children if current page is a main section page (L1):
        //        if (node.level == 1)
        //        {  
        //            //render direct children as RH nav:
        //            output = getHTMLList(node.getChildren(), node);
        //        }

        //        //get siblings:
        //        if (node.level == 2)
        //        {
        //            l2Sibs = objectFactory.getNodeManager().getSiblings(node);
        //            output = "<ul>";
        //            for(a=0;a<l2Sibs.Length;a++)
        //            {
        //                currPage = new Page(l2Sibs[a].pageId);

        //                //get child list (if any) of SELF node:
        //                if(l2Sibs[a].isCurrentNode)
        //                {
        //                    output += "<li><span class='current_link'>" + currPage.linkText + "</span>\n";
        //                    output += getHTMLList(l2Sibs[a].getChildren(),node);
        //                    output += "</li>";
        //                }

        //                //otherwise, just render siblings:
        //                else
        //                {
        //                    output += "<li><a href='/ccms.aspx?nodeid=" + l2Sibs[a].id + "'title='" + currPage.linkText + "'>"+currPage.linkText + "</a></li>\n";
        //                }
        //            }
        //            output += "</ul>";
        //            }

        //            //get parent and sibs, get sibs:
        //            if (node.level == 3)
        //            {
        //                l2Sibs = objectFactory.getNodeManager().getSiblings(node.getParent());

        //                Node l2Parent = objectFactory.getNodeManager().getBreadcrumb(node)[2];  //index is equal to level

        //                output = "<ul>";
        //                for(a=0;a<l2Sibs.Length;a++)
        //                {
        //                    currPage = new Page(l2Sibs[a].pageId);
        //                    output += "<li>";
        //                    output += "<a href=\"/ccms.aspx?nodeid=" + l2Sibs[a].id + "\" title=\"" + currPage.linkText + "\">" + currPage.linkText + "</a>";

        //                    //parent:
        //                    if(l2Parent.id == l2Sibs[a].id){
        //                    output += getHTMLList(l2Sibs[a].getChildren(),node);
        //                }
        //             output += "</li>";
        //            }
        //            output += "</ul>";
        //        }
        //        return output;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        ////utility function to return list of childs of current node as HTML list
        //private string getHTMLList(Node[] nodeList, Node self)
        //{
        //    string output = "";
        //    if(nodeList.Length > 0)
        //    {
        //        Page page = null;
        //        output += "<ul>";
        //        for(int x=0;x<nodeList.Length;x++)
        //        {
        //            page = new Page(nodeList[x].pageId);

        //            output += "<li>";
        //            if(nodeList[x].id == self.id)
        //            {
        //                output += "<span class='current_link'>" + page.linkText + "</span>";
        //            }
        //            else
        //            {
        //                output += "<a href=\"/ccms.aspx?nodeid=" + nodeList[x].id + "\" title=\"" + page.linkText + "\">" + page.linkText + "</a>";
        //            }
        //            output += "</li>";
        //        }
        //        output += "</ul>";
        //    }
        //    return output;
        //}
    }
}


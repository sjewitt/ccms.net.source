using System;
using System.Collections.Generic;
using System.Text;
using ccms.managers;
using System.Configuration;
using System.Web.Script.Serialization;

namespace ccms
{
    public class CCMSEngine
    {
        public ObjectFactory objectFactory;
        public NodeManager nodeManager;
        public PageManager pageManager;
        public ContentManager contentManager;
        public LayoutManager layoutManager;
        public User currentUser;

        //private List<Slot> coreSlots;       //core properties
        //private List<Slot> contentSlots;    //content slots
        //private List<Slot> customSlots;     //bespoke custom slots

        public static int RENDERMODE_ANONYMOUS = 0;
        public static int RENDERMODE_SESSION = 1;
        public static int RENDERMODE_EDIT = 2;

        private string templateBasePath;

        public string TemplateBasePath
        {
            get { return templateBasePath; }
            set { templateBasePath = value; }
        }

        public CCMSEngine(string TemplateBasePath)
        {
            //set basepath for later use in UI:
            this.TemplateBasePath = TemplateBasePath;
            
            /*
             initialise the various factories:
             */
            objectFactory = new ObjectFactory(TemplateBasePath);
            nodeManager = objectFactory.getNodeManager();
            pageManager = objectFactory.getPageManager();
            contentManager = objectFactory.getContentManager();
            layoutManager = objectFactory.getLayoutManager();
        }
        
        /*
         * Accepts the current node and a User object (which might be null) passed from
         * the ccms.aspx page:
         */
        public string renderNode(Node node, User user, int renderMode, System.Web.UI.Page currentHTTPPage)
        {
            try
            {
                //get the corresponding Page:
                Page currentPage = pageManager.getPage(node);

                //get layout:
                Layout currentLayout = node.getLayout();

                //get content:
                //Content[] currentContent = currentPage.getContent();

                return layoutManager.getParsedLayoutSource(currentLayout, node, user, renderMode, currentHTTPPage);
            }
            catch (Exception ex)
            {
                return "An error occurred: "+ex.Message;
            }
        }
    }
}

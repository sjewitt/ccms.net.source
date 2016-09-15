//using System;
//using System.Collections.Generic;

//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using ccms.managers;
//using System.Collections;
//using ccms.utils;
//using ccms;

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
    public partial class editor_assign_content : System.Web.UI.Page
    {
        private ObjectFactory of =  null;
        private ContentManager cm = null;
        private ArrayList contentItems = null;
        private PageManager pm = null;
        private Content selectedItem = null;
        private int slotnum = 0;
        private int pageid = 0;
        private int selectedIndex = -1;
        private int action = -1; //update or add


        protected void Page_Load(object sender, EventArgs e)
        {
            of = new ObjectFactory();
            cm = of.getContentManager();
            pm = of.getPageManager();

            //get action:
            if (Request.QueryString["doaction"] != null)
            {
                if (Request.QueryString["doaction"].Equals("add"))
                    this.action = EditUtils.CONTENTMAP_ADD;
                else if (Request.QueryString["doaction"].Equals("replace"))
                    this.action = EditUtils.CONTENTMAP_UPDATE;
                else this.action = -1;
            }

            //get selected index:
            selectedIndex = this.sel_content.SelectedIndex;

            //clear existing dropdown items (otherwise it gets longer on each postback...)
            this.sel_content.Items.Clear();
            this.sel_content.Items.Add(new ListItem("[select content item]"));
            this.lbl_contentBody.Text = "";
            this.lbl_contentTitle.Text = "";
            this.lbl_id.Text = "";
            this.lbl_versionCount.Text = "";


            //load all content
            contentItems = cm.getContentItems();
            Content _curr = new Content();
            for (int a = 0; a < contentItems.Count; a++)
            {
                _curr = (Content)contentItems[a];
                this.sel_content.Items.Add(new ListItem(_curr.name,_curr.id+""));
            }
            /*
             I ADDED AN ITEM MANUALLY!!! ARGH!
             * this.contentItems is ONE LESS in length than this.sel_content....
             */
            this.sel_content.SelectedIndex = selectedIndex;

            if (selectedIndex > 0)
            {
                this.selectedItem = (Content)contentItems[selectedIndex-1];
            }
        }

        protected void sel_content_SelectedIndexChanged(object sender, EventArgs e)
        {
            //load content from specified item:
            int contentId = -1;
            if (Int32.TryParse(this.sel_content.SelectedItem.Value, out contentId))
            {
                selectedItem = cm.getContentItem(contentId);
                this.lbl_contentTitle.Text = selectedItem.name;
                this.lbl_contentBody.Text = this.cm.getActiveInstance(selectedItem).data;
                this.lbl_id.Text = selectedItem.id + "";
                this.lbl_versionCount.Text = this.cm.getInstanceList(selectedItem).Count + "";
                this.lbl_status.Text = "Content assigned OK";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Int32.TryParse(Request.QueryString["pageid"], out pageid) && Int32.TryParse(Request.QueryString["slotid"], out slotnum))
            {
                PageContentMap map = new PageContentMap();
                map.contentId = selectedItem.id;
                map.pageId = pageid;
                map.slotNum = slotnum;
                if(this.action == EditUtils.CONTENTMAP_ADD )this.pm.addContentMapping(map);
                if (this.action == EditUtils.CONTENTMAP_UPDATE) this.pm.updateContentMapping(map);
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            if (Int32.TryParse(Request.QueryString["pageid"], out pageid) && Int32.TryParse(Request.QueryString["slotid"], out slotnum))
            {
                PageContentMap map = new PageContentMap();
                map.contentId = selectedItem.id;
                map.pageId = pageid;
                map.slotNum = slotnum;
                if (this.action == EditUtils.CONTENTMAP_ADD) this.pm.addContentMapping(map);
                if (this.action == EditUtils.CONTENTMAP_UPDATE) this.pm.updateContentMapping(map);
            }
        }
    }
}
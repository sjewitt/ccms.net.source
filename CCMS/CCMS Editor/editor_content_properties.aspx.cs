using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccms.managers;
using System.Collections;

namespace ccms
{
    public partial class editor_content_properties1 : System.Web.UI.Page
    {
        private ObjectFactory of = null;
        private ContentManager cm = null;
        private EditUtils editUtils = null;
        private Content content = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            of = new ObjectFactory();

            //get passed content ID
            int contentId = -1;
            if(Int32.TryParse(Request.QueryString["contentid"],out contentId))
            {
                cm = of.getContentManager();
                editUtils = new EditUtils();
                
                content = cm.getContentItem(contentId);
                
                if (!IsPostBack)
                {
                    //set field values to current object values:
                    this.setFieldValues(content);
                }
            }
        }
        
        protected void btn_update_Click(object sender, EventArgs e)
        {
            if (this.txt_name.Text.Length > 0)
            {
                //update the properties:
                content.contentType = Int32.Parse(this.sel_contentType.SelectedValue);
                content.Description = this.txt_description.Text;
                content.name = this.txt_name.Text;

                content.StartDate = setDateForNullInsert();
                content.EndDate = setDateForNullInsert();

                if (this.txt_validFrom.Text.Length > 0)
                {
                    content.StartDate = editUtils.getDateFromString(this.txt_validFrom.Text);
                }

                if (this.txt_validTo.Text.Length > 0)
                {
                    content.EndDate = editUtils.getDateFromString(this.txt_validTo.Text);
                }
                this.cm.updateContentProperties(content);
                
                //set field values to updated object values:
                this.setFieldValues(content);
            }
            //throw error here?
            Response.Write("VALUE IN NAME BOX:" + this.txt_name.Text + "<br />");
        }
        
        private string getPaddedDatePart(int _in)
        {
            string _out = _in + "";
            if (_out.Length == 1) _out = "0" + _out;
            return _out;
        }
        
        private void setFieldValues(Content _content)
        {
            this.txt_description.Text = content.Description;
            this.txt_name.Text = content.name;

            //build dropdown of content types
            IList<ContentType> cTypes = this.cm.getContentTypes();
            IEnumerator enumerator = cTypes.GetEnumerator();

            this.sel_contentType.Items.Clear();
            int selectedIndex = -1;
            int counter = 0;
            while (enumerator.MoveNext())
            {
                ContentType _cType = (ContentType)enumerator.Current;
                this.sel_contentType.Items.Add(new ListItem(_cType.Name, _cType.Id.ToString()));
                if (_cType.Id == content.contentType)
                {
                    selectedIndex = counter;
                }
                counter++;
            }
            //set ctype:
            this.sel_contentType.SelectedIndex = selectedIndex;

            //yyyy-mm-dd
            if (content.StartDate.Year > 1000)   //or something... Defaults to 1.
                this.txt_validFrom.Text = content.StartDate.Year + "-" + getPaddedDatePart(content.StartDate.Month) + "-" + getPaddedDatePart(content.StartDate.Day);
            if (content.EndDate.Year > 1000)   //or something... Defaults to 1.
                this.txt_validTo.Text = content.EndDate.Year + "-" + getPaddedDatePart(content.EndDate.Month) + "-" + getPaddedDatePart(content.EndDate.Day);
        }

        /// <summary>
        /// return zero year datetime
        /// </summary>
        /// <returns></returns>
        private DateTime setDateForNullInsert()
        {
            DateTime _out = new DateTime(1, 1, 1);
            return _out;
        }
    }
}
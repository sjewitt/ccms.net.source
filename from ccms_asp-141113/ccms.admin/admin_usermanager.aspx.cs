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
using System.Reflection;

namespace CCMS_editor
{
    public partial class admin_usermanager : System.Web.UI.Page
    {
        private List<User> users;
        private ObjectFactory objectFactory;
        private UserManager userManager;
        private User user;
        protected void Page_Load(object sender, EventArgs e)
        {
            objectFactory = new ObjectFactory();
            userManager = objectFactory.getUserManager();
            users = userManager.getUsers();
            if (this.usermanager_userlist.Items.Count == 0)
            {
                this.usermanager_userlist.Items.Add(new ListItem(" - Select User - ", "-1"));
                IEnumerator enumerator = users.GetEnumerator();
                User _usr;

                while (enumerator.MoveNext())
                {
                    _usr = (User)enumerator.Current;
                    this.usermanager_userlist.Items.Add(new ListItem(_usr.fullName, _usr.id.ToString()));
                }
            }
            //initial checkbox display:
            if(this.user_perms_checkbox.Items.Count == 0)
            {
                User _usr = new User();
                _usr.permissions = 0;
                populatePermissionsCheckboxes(_usr);
            }

            //reset the active checkbox:
            this.chk_user_isactive.Checked = false;
        }

        protected void usermanager_userlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            //new user
            if (usermanager_userlist.Items[usermanager_userlist.SelectedIndex].Value.Equals("-1"))
            {
                this.lbl_adduser.Visible = true;
                this.lbl_edituser.Visible = false;
                this.btn_adduser.Visible = true;
                this.btn_updateuser.Visible = false;
                this.user = new User(); //empty user object
            }
            //existing user
            else
            {
                this.lbl_adduser.Visible = false;
                this.lbl_edituser.Visible = true;
                this.btn_adduser.Visible = false;
                this.btn_updateuser.Visible = true;
                int userId;
                if(Int32.TryParse(usermanager_userlist.Items[usermanager_userlist.SelectedIndex].Value,out userId))
                {
                    this.user = new User(userId);
                }
                else
                {
                    //error
                }
            }

            //and set the values of the form fields to the user properties:
            this.user_email.Text = this.user.email;
            this.user_fullname.Text = this.user.fullName;
            this.user_login.Text = this.user.login;
            this.user_password.Text = this.user.password;
            this.user_perms.Text = this.user.permissions.ToString();
            this.chk_user_isactive.Checked = this.user.active;

            populatePermissionsCheckboxes(this.user);
        }

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void populatePermissionsCheckboxes(User _usr)
        {
            //remove existing checkboxes:
            this.user_perms_checkbox.Items.Clear();

            //enumerate the Permissions static class:
            Type perms = typeof(Permissions);
            FieldInfo[] fields = perms.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (FieldInfo field in fields)
            {
                //CheckBox _chk = new CheckBox();
                //_chk.ID = "permissions";
                //_chk.Text = field.Name;
                //_chk.
                //pnl_chkboxes.Controls.Add(_chk);
                bool isChecked = false;
                if ((_usr.permissions & (int)field.GetValue(perms)) == (int)field.GetValue(perms))
                {
                    isChecked = true;
                }
                this.user_perms_checkbox.Items.Add(this.buildListItem(field.Name, field.GetValue(perms).ToString(), isChecked));
            }
        }

        private ListItem buildListItem(string label, string value, bool isChecked)
        {
            ListItem _item = new ListItem();
            _item.Text = label;
            _item.Value = value;
            _item.Selected = isChecked;
            //_item.Attributes..Add("name", "permissions");    //oerride default to create a client-side array
            
            return _item;
        }

        protected void user_isactive_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void btn_updateuser_Click(object sender, EventArgs e)
        {
            //iterate over checked checkboxes and determine the user perms:
            int newPermMask = 0;
            for (int a = 0; a < this.user_perms_checkbox.Items.Count; a++)
            {
                int val = 0;
                if(this.user_perms_checkbox.Items[a].Selected && Int32.TryParse(this.user_perms_checkbox.Items[a].Value,out val) && val != 0)
                {
                    newPermMask += val;
                }
            }
            this.user_perms.Text = newPermMask.ToString();

            this.user.email = this.user_email.Text;
            this.user.fullName = this.user_fullname.Text;
            this.user.login = this.user_login.Text;
            this.user.password = this.user_password.Text;
            this.user.permissions = newPermMask;
            this.userManager.updateUser(this.user);
            //active flag:
        }
    }
}
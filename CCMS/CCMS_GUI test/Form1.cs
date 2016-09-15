using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ccms;
using ccms.managers;
using ccms.utils;
using System.Collections;

namespace GUItest
{
    public partial class Form1 : Form
    {
        private ObjectFactory objectFactory;
        private NodeManager nodeManager;
        private UserManager userManager;
        private PageManager pageManager;
        private ContentManager contentManager;
        private LayoutManager layoutManager;
        
        private int userUpdateMode = UserManager.USER_ACTION_CREATE;
        private User user = null;
        private Page page = null;
        private Node currentNode;

        public Form1()
        {
            InitializeComponent();
            this.objectFactory = new ObjectFactory();
            this.nodeManager = objectFactory.getNodeManager();
            this.userManager = objectFactory.getUserManager();
            this.pageManager = objectFactory.getPageManager();
            this.contentManager = objectFactory.getContentManager();
            this.layoutManager = objectFactory.getLayoutManager();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            reloadUserDropdown();
            reloadNodeDropdown();
        }


        private void User_btn_apply_Click(object sender, EventArgs e)
        {
            //load a User object from the values found in the textboxes
                       
            if (this.userUpdateMode == UserManager.USER_ACTION_CREATE)
            {
                try
                {
                    this.loadUserFromUI();

                    if (this.confirmAction("You are about to create a new user. Proceed?"))
                    {
                        //this.user.update();
                        userManager.createUser(this.user);
                        User_lbl_status.Text = "User '" + user.fullName + "' created OK.";

                        //and reload the dropdown:
                        reloadUserDropdown();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating new User: " + ex.Message);
                }
            }
            else if (this.userUpdateMode == UserManager.USER_ACTION_UPDATE)
            {
                this.User_lbl_status.Text = "Updating...";
                try
                {
                    if (confirmAction("You are about to UPDATE user '" + this.user.fullName + "'. Proceed?"))
                    {
                        this.updateLoadedUser();
                        //this.user.update();
                        userManager.updateUser(this.user);
                        User_lbl_status.Text = "User '" + this.user.fullName + "' updated OK.";

                        //and reload the dropdown:
                        reloadUserDropdown();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating User '" + this.user.fullName + "': " + ex.Message);
                }
            }
        }
        
        private void User_btn_delete_Click(object sender, EventArgs e)
        {
            //this.loadUser();
            if (confirmAction("You are about to DELETE user '" + this.user.fullName + ". Proceed?'"))
            {
                userManager.deleteUser(this.user);
                //TODO: stuff around reloading the userlist...
                //disable the button:
                this.User_btn_delete.Enabled = false;
                //and reload the dropdown:
                reloadUserDropdown();
            }
        }  
    
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //build a User object from the poperties of the textboxes
        private void loadUserFromUI()
        {
            this.user = new User();
            this.user.active        = true;
            this.user.email         = User_txt_email.Text;
            this.user.fullName      = User_txt_fullname.Text;
            this.user.login         = User_txt_login.Text;
            this.user.password      = User_txt_password.Text;
            this.user.permissions   = 1;
        }

        //update the currently loaded user from textbox properties:
        private void updateLoadedUser()
        {
            this.user.email = User_txt_email.Text;
            this.user.fullName = User_txt_fullname.Text;
            this.user.login = User_txt_login.Text;
            this.user.password = User_txt_password.Text;
            reloadUserDropdown();
            comboBox1.Refresh();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.user = (User)comboBox1.Items[comboBox1.SelectedIndex];
            this.displayUserDetails(user);

            //set buttons accordingly:
            if (this.userUpdateMode == UserManager.USER_ACTION_CREATE)
            {
                User_btn_delete.Enabled = false;
                User_btn_apply.Text = "Create user";
            }
            else if (this.userUpdateMode == UserManager.USER_ACTION_UPDATE)
            {
                User_btn_delete.Enabled = true;
                User_btn_apply.Text = "Update user";
            }
        }

        /*
         Clear and reload the dropdown:
         */
        private void reloadUserDropdown()
        {
            comboBox1.Items.Clear();

            //first, add a null item at the top:
            User dummyUser = new User();
            dummyUser.fullName = "<Add new user>";
            dummyUser.id = 0;
            comboBox1.Items.Add(dummyUser);

            List<User> users = this.userManager.getUsers();
            
            //determine the object properties to map to the display and value combobox properties:
            comboBox1.ValueMember   = "id";
            comboBox1.DisplayMember = "fullName"; //display field (of User object)
            
            /*
             * this actually adds a User object to each index of the dropdown.
             * I can therefore simply refer to this, rather than reloading the user. But hey-ho...
             * */
            if (users != null)
            {
                foreach (User usr in users)
                {
                    this.comboBox1.Items.Add(usr);
                }
            }
        }


        /*
         NODE MANIPULATION:
         */
        //get all, populate a dropdown:
        private void reloadNodeDropdown()
        {
            comboBox_nodes.Items.Clear();
            IList<Node> nodes = this.nodeManager.getNodes();

            //determine the object properties to map to the display and value combobox properties:
            comboBox_nodes.ValueMember = "Id";
            comboBox_nodes.DisplayMember = "Id"; //display field (of User object)

            //tAppend each Node:
            if (nodes != null)
            {
                foreach (Node thenode in nodes)
                {
                    this.comboBox_nodes.Items.Add(thenode);
                }
            }
        }

        //get child nodes, given a parent node:
        private void comboBox_nodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentNode = (Node)comboBox_nodes.Items[comboBox_nodes.SelectedIndex];

            //get the level:
            int level = nodeManager.getNodeLevel(currentNode);
            label11.Text = level.ToString();

            bool hasChilds = nodeManager.hasChildren(currentNode);
            string x = "";

            if (hasChilds)
            {
                List<Node> childList = nodeManager.getChildNodes(currentNode);

                textBox_output_nodes.Text = "";
                int counter = 1;
                foreach (Node thenode in childList)
                {
                    x += "child " + counter + ": ID=" + thenode.id + ", PAGE ID: " + thenode.pageId + ", PARENT ID: " + thenode.parentId + ", LAYOUT ID: " + thenode.layoutId + "\r\n";
                    counter++;
                }
            }
            else
            {
                x = "Node " + currentNode.id + " has no children.";
            }
            textBox_output_nodes.Text = x;
        }

        private void displayUserDetails(User user)
        {
            //?
            if (user.id != 0)
            {
                this.userUpdateMode = UserManager.USER_ACTION_UPDATE;
            }
            else
            {
                this.userUpdateMode = UserManager.USER_ACTION_CREATE;
            }

            User_txt_email.Text = user.email;
            User_txt_fullname.Text = user.fullName;
            User_txt_login.Text = user.login;
            User_txt_password.Text = user.password;
            label9.Text = user.id.ToString();
        }

        private bool confirmAction(string msg)
        {
            bool returnVal = false;

            DialogResult status = MessageBox.Show(msg, "Confirmation", MessageBoxButtons.OKCancel);
            if (status.ToString().Equals("OK"))
            {
                returnVal = true;
            }
            return returnVal;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //get a branch from current node:
        private void button3_Click(object sender, EventArgs e)
        {
            if (currentNode != null)
            {
                textBox_output_nodes.Text = "";
                getBranch((Node)comboBox_nodes.Items[comboBox_nodes.SelectedIndex], " - ");
            }
        }

        //HTF do I actually return something from here...
        private void getBranch(Node node, string spacer)
        {
            string currLine = spacer;
            currLine += node.id + "(pageId=" + node.pageId + ")\r\n";
            textBox_output_nodes.Text += currLine;
            if(this.nodeManager.hasChildren(node))
            {
                List<Node> childs = this.nodeManager.getChildNodes(node);
                foreach( Node currNode in childs)
                {
                    this.getBranch(currNode,spacer + " - ");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (currentNode != null)    //check for it being a Node instance...
            {
                //get a page:
                this.page = pageManager.getPage(currentNode);

                tabControl1.SelectTab("tabPage3");

                string x = "";

                x+="Source node: " + this.currentNode.id + "\r\n";

                x += "createdDate: " + page.createdDate + "\r\n";
                x += "createdUser: " + page.createdUser + "\r\n";
                x += "description: " + page.description + "\r\n";
                x += "Id: " + page.id + "\r\n";
                x += "keywords: " + page.keywords + "\r\n";
                x += "layoutId: " + page.layoutId + "\r\n";
                x += "linkText: " + page.linkText + "\r\n";
                x += "Name: " + page.name + "\r\n";
                x += "state: " + page.state + "\r\n";
                x += "title: " + page.title + "\r\n";
                x += "updatedDate: " + page.updatedDate + "\r\n";
                x += "updatedUser: " + page.updatedUser + "\r\n";

                textBox_page_data.Text = x;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //get an instance of PageContentArray()
            if (this.page != null)
            {
                IList<PageContentMap> map = contentManager.getPageContentMap(this.page);
                tabControl1.SelectTab("tabPage4");
                string x = "";
                Content content;
                ContentInstance inst;
                //User edit_user;
                for (int a = 1; a <= map.Count; a++)
                {
                    x += "Content item " + a + ":\r\n";
                    x += "Content ID: "+map[a - 1].contentId + "\r\n";
                    x += "Slot ID: " + map[a - 1].slotNum + "\r\n";
                    x += "Page ID: " + map[a - 1].pageId + "\r\n";
                    x += "Slot ID: " + map[a - 1].slotNum + "\r\n";

                    //retrieve the content:
                    content = contentManager.getContentItem(map[a - 1].contentId);
                    x += " --> authGroup: " + content.authGroup + "\r\n";
                    x += " --> contentType: " + content.contentType + "\r\n";
                    x += " --> createdDate: " + content.createdDate + "\r\n";
                    x += " --> createdUser: " + content.createdUser + "\r\n";
                    x += " --> Id: " + content.id + "\r\n";
                    x += " --> Name: " + content.name + "\r\n";
                    x += " --> updatedDate: " + content.updatedDate + "\r\n";
                    x += " --> updatedUser: " + content.updatedUser + "\r\n";
                    x += " --> description: " + content.Description + "\r\n";
                    
                    ////and retrieve the active instance:
                    inst = contentManager.getActiveInstance(content);    //I am expecting only one.
                    x += " ----> contentId: " + inst.contentId + "\r\n";
                    x += " ----> data: " + inst.data + "\r\n";
                    x += " ----> editUser: " + inst.editUser + "\r\n";

                    ////and get the edit user:
                    //edit_user = userManager.getUser(inst.editUser);
                    //x += " ------> active: " + edit_user.active + "\r\n";
                    //x += " ------> email: " + edit_user.email + "\r\n";
                    //x += " ------> fullName: " + edit_user.fullName + "\r\n";
                    //x += " ------> Id: " + edit_user.id + "\r\n";
                    //x += " ------> login: " + edit_user.login + "\r\n";
                    //x += " ------> password: " + edit_user.password + "\r\n";
                    //x += " ------> permissions: " + edit_user.permissions + "\r\n";
                    //x += " ----> Id: " + inst.id + "\r\n";
                    //x += " ----> state: " + inst.state + "\r\n";
                    //x += " ----> updatedDate: " + inst.updatedDate + "\r\n";
                    //x += " ----> versionId: " + inst.versionId + "\r\n";
                }
                    textBox_contentMap.Text = x;
            }
        }

        //test display a HTML layout
        private void button5_Click(object sender, EventArgs e)
        {
            //get the list of templates (from FS? or from DB???)
            // http://www.csharp-examples.net/filestream-read-file/
            try
            {
                int layoutId;   //retrieve this from supplied node.
                if (int.TryParse(textBox3.Text, out layoutId))
                {
                    //Layout layout = layoutManager.getLayout(layoutId); //get database entry
                    //Layout template = layoutManager.getTemplate(layout);
                    //textBox2.Text = layoutManager.getParsedLayoutSource(layout,this.currentNode);
                    //webBrowser1.DocumentText = layoutManager.getParsedLayoutSource(layout,this.currentNode);

                    CCMSEngine engine = new CCMSEngine();
                    //webBrowser1.DocumentText = engine.renderNode(layoutId);
                }
                else
                {
                    textBox2.Text = "template not found.";
                    
                }
            }
            catch (Exception ex)
            {
                textBox2.Text = ex.Message;
            }
        }

        //get the full HTML from supplied NodeId
        private void button6_Click(object sender, EventArgs e)
        {
            if (this.currentNode != null)
            {
                textBox_output_nodes.Text = "Fully rendered page:\r\n";

                Node n = this.nodeManager.getNode(this.currentNode.id);  //a bit redundant I know, but simulates teh request.nodeid from URL
                Page p = this.pageManager.getPage(n);
                //Layout t = this.layoutManager.getTemplate(n);
                //string s = this.layoutManager.getParsedTemplateSource(t, n);
                //this.textBox_output_nodes.Text += s;
            }
        }

        private void textBox_page_data_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void btnCreatePage_Click(object sender, EventArgs e)
        {
            Page newpage = new Page();
            newpage.keywords = txtPageKeywords.Text;
            newpage.linkText = txtPageLinktext.Text;
            newpage.name = txtPageName.Text;
            newpage.title = txtPageTitle.Text;
            newpage.description = txtPageDescription.Text;
            pageManager.createNewPage(newpage,1);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_contentinstance_Click(object sender, EventArgs e)
        {
            Content c = this.contentManager.getContentItem(2);
            IList<ContentInstance> ciList = this.contentManager.getInstanceList(c);
            this.textBox4.Text = "";
            IEnumerator <ContentInstance>enumerator = ciList.GetEnumerator();
            ContentInstance _curr;
            while (enumerator.MoveNext())
            {
                _curr = enumerator.Current;
                this.textBox4.Text += "CONTENT ID: "+_curr.contentId + "\r\n";
                this.textBox4.Text += "INSTANCE ID: " + _curr.id + "\r\n";
                this.textBox4.Text += "VERSION ID: " + _curr.versionId + "\r\n";
                this.textBox4.Text += "EDIT USER: " + (new User(_curr.editUser)).fullName + "\r\n";
                this.textBox4.Text += "STATE: " + _curr.state + "\r\n";
                this.textBox4.Text += "UPDATED DATE: " + _curr.updatedDate + "\r\n";
                this.textBox4.Text +=  _curr.data + "\r\n";
                this.textBox4.Text += "\r\n";
            }
        }
    }
}

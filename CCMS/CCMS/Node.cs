using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using ccms.utils;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace ccms
{
    public class Node
    {
        /// <summary>
        /// ORM Mapping class.
        /// Class representing a position in the content hierarchy (viewtree).
        /// Note that a Node object has a Page object reference. In other words,
        /// the same Page object (and therefore its associated Content) may in 
        /// fact appear more than once in the viewtree.
        /// </summary>
        /// 

        private string templateBasePath;

        public string TemplateBasePath
        {
            get { return templateBasePath; }
            set { templateBasePath = value; }
        }

        public Node(int nodeId, string TemplateBasePath, bool loadTree = false)
        {
            this.TemplateBasePath = TemplateBasePath;

            try
            {
                this._session = new DBSession();
                this._conn = new SqlConnection(this._session.dbConnStr);
                this._conn.Open();

                SqlDataReader reader = null;
                //TODO: use parameters as per CCMS ASP
                //SqlCommand cmd = new SqlCommand("select id,parent_id,page_id,layout_id, ordering from viewtree where id = ? ;",this._conn);
                SqlCommand cmd = new SqlCommand("select id,parent_id,page_id,layout_id, ordering from viewtree where id = " + nodeId + ";", this._conn);

                //TODO: Add checking around num results etc:
                reader = cmd.ExecuteReader();

                //get first row:
                reader.Read();
                this._id        = reader.GetInt32(0);
                this._parentId  = reader.GetInt32(1);
                this._pageId    = reader.GetInt32(2);
                
                //check for NULL values:
                if(!reader.IsDBNull(3)) this._layoutId  = reader.GetInt32(3);
                if(!reader.IsDBNull(4)) this._ordering  = reader.GetInt32(4);

                //close the reader:
                reader.Close();

                //determine the level:
                this._level = 0;
                if(this._parentId != 0)
                {
                    Node levelCheckNode = this;
                    while (levelCheckNode.parentId != 0)
                    {
                        levelCheckNode = new Node(levelCheckNode.parentId,this.TemplateBasePath);
                        this._level++;
                    }
                }

                //does the current node have children?
                cmd = new SqlCommand("select count (*) as count from viewtree where parent_id = " + nodeId + ";", this._conn);
                reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.GetInt32(0) > 0) { 
                    this._hasChildren = true;
                    this._childCount = reader.GetInt32(0);
                    reader.Close();
                    
                    if (loadTree)
                    {
                        this.getChildren(loadTree);
                    }

                }
                
                this._conn.Close();
            }
            catch (Exception ex) 
            {
                if (this._conn.State != ConnectionState.Closed)
                {
                    this._conn.Close();
                }
                throw new Exception("Cannot instantiate Node: " + ex.Message); 
            }
        }

        private SqlConnection   _conn;
        private DBSession       _session;

        private int _id;
        public int id
        {
            get{return this._id;}
            set{this._id  = value;}
        }
        private int _parentId;
        public int parentId
        {
            get { return this._parentId; }
            set { this._parentId = value; }
        }

        private int _pageId;
        public int pageId
        {
            get{return this._pageId;}
            set { this._pageId = value; }
        }
        
        private int _layoutId = 0;
        public  int layoutId
        {
            get { return this._layoutId; }
            set { this._layoutId = value; }
        }

        private int _ordering = 0;
        public int ordering
        {
            get { return this._ordering; }
            set { this._ordering = value; }
        }

        private int _level = -1;
        public int level
        {
            get { return this._level; }
        }

        private bool _hasChildren = false;
        public bool hasChildren
        {
            get { return this._hasChildren; }
        }

        private int _childCount = 0;
        public int childCount
        {
            get { return this._childCount; }
        }

        private Node[] childs = null;
        public Node[] Childs
        {
            get { return this.childs; }
        }

        private bool _layoutInherited = false;
        public bool layoutInherited
        {
            get { return this._layoutInherited; }
        }

        private bool _isCurrentNode = false;
        public bool isCurrentNode
        {
            get { return this._isCurrentNode; }
            set { this._isCurrentNode = value; }
        }

        public Node[] getChildren(bool loadTree = false)
        {
            try
            {
                bool isAlreadyOpen = true;
                if (this._conn.State == ConnectionState.Closed)
                {
                    isAlreadyOpen = false;
                    this._conn.Open();
                }
                Node[] nodeArray = new Node[this.childCount];
                if (this.hasChildren)
                {
                    SqlCommand cmd = new SqlCommand("select id from viewtree where parent_id = " + this._id + " order by ordering;", this._conn);

                    //TODO: Add checking around num results etc:
                    SqlDataReader reader = cmd.ExecuteReader();
                    Int32 counter = 0;
                    while (reader.Read())
                    {
                        nodeArray[counter] = new Node(reader.GetInt32(0),this.TemplateBasePath, loadTree);
                        counter++;
                    }
                    if (loadTree)
                    {
                        this.childs = nodeArray;
                    }
                    reader.Close();
                }
                //if we are loading childs resursively, keep the connection open and only close once the node has fully loaded
                if (!isAlreadyOpen)
                {
                    this._conn.Close();
                } 
                return nodeArray;
            }
            catch (Exception ex)
            {
                if (this._conn.State != ConnectionState.Closed)
                {
                    this._conn.Close();
                }
                throw new Exception("Cannot retrieve associated child Node array: " + ex.Message);
            }
        }
       
        public Node getParent() {
            try
            {
                Node node = new Node(this.parentId,this.TemplateBasePath);
                return node;
            }
            catch(Exception ex)
            {throw new Exception("Cannot retrieve parent node: "+ex.Message); }
        }

        public Page getPage() {
            try
            {
                return new Page(this.pageId);
            }
            catch (Exception ex)
            {throw new Exception("Cannot retrieve associated Page object: " + ex.Message);}
        }

        /*
         * if the layout ID has not been set at the node level directly, we 
         * must walk up the node tree to retrieve a layout ID applied higher up:
        */
        public Layout getLayout()
        {
            try
            {
                Layout layout = null;
                if (this.layoutId > 0)
                {
                    layout = new Layout(this.layoutId,this.TemplateBasePath);
                }
                else
                {
                    Node node = this;
                    while (node.layoutId == 0)
                    {
                        node = new Node(node.parentId,this.TemplateBasePath);
                    }

                    layout = new Layout(node.layoutId,this.TemplateBasePath);
                }
                return layout;
            }

            catch (Exception ex)
            {throw new Exception("Cannot retrieve associated Template object: " + ex.Message);}
        }
    }
}

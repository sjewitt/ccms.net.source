using System;
using System.Collections.Generic;
using System.Text;
using ccms.utils;
using System.Data.SqlClient;

namespace ccms.managers
{
    public class NodeManager
    {

        public NodeManager(DBSession session)
        {
            this.session = session;
        }
        //March 2016
        public NodeManager(DBSession session,string TemplateBasePath)
        {
            this.session = session;
            this.templateBasePath = TemplateBasePath;
        }

        private DBSession session = null;
        private SqlConnection conn;
        SqlDataReader reader = null;
        private string templateBasePath;

        public string TemplateBasePath
        {
            get { return templateBasePath; }
            set { templateBasePath = value; }
        }



        public Node getRootNode(bool loadTree=false)
        {
            try
            {
                int rootId = 0; //determine with query

                //select id from viewtree where parent_id=0;
                this.conn = new SqlConnection(this.session.dbConnStr);
                this.conn.Open();

                SqlCommand cmd = new SqlCommand("select id from viewtree where parent_id=0;", this.conn);
                
                //TODO: Add checking around num results etc:
                reader = cmd.ExecuteReader();

                //get first row:
                reader.Read();
                rootId = reader.GetInt32(0);
                reader.Close();
                return new Node(rootId, this.TemplateBasePath, loadTree);
            }
            catch (Exception e)
            {
                throw (new Exception("Cannot retrieve root node: " + e.Message));
            }
        }

        public Node getNode(int nodeId)
        {
            try
            {
                return new Node(nodeId, this.TemplateBasePath);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot retrieve node with ID " + nodeId + ": " + e.Message);
            }
        }

        public Node getParentNode(Node node)
        {
            try
            {
                return new Node(node.parentId, this.TemplateBasePath);
            }
            catch (Exception ex)
            {
                throw (new Exception("Cannot retrieve parent node for " + node.id + "" + ex.Message));
            }
        }

        /*
        get all nodes:
        */
        public IList<Node> getNodes(int subtype=0)
        {
            try
            {
                IList<Node> nodes = new List<Node>();
                SqlConnection conn = new SqlConnection(session.dbConnStr);
                
                conn.Open();
 
                string sql = "select id from viewtree";
                if(subtype>0)
                {
                    sql = "select id from viewtree where subtype_id=@ID";
                }
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("ID", subtype);
                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    nodes.Add(new Node(reader.GetInt32(0), this.TemplateBasePath));
                }


                conn.Close();

                return nodes;
            }
            catch (Exception ex)
            {
                throw (new Exception("Cannot retrieve node list: " + ex.Message));
            }
        }

        /*
         return child nodes of a specified node:
         */
        public List<Node> getChildNodes(Node node)
        {
            try
            {
                IList<Node> nodes = new List<Node>();
                SqlConnection conn = new SqlConnection(session.dbConnStr);

                conn.Open();
                SqlCommand cmd = new SqlCommand("select id from viewtree where parent_id=" + node.id, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    nodes.Add(new Node(reader.GetInt32(0), this.TemplateBasePath));
                }
                conn.Close();

                return (List<Node>)nodes;
            }
            catch (Exception e)
            {
                throw (new Exception("Cannot retrieve child node list for " + node.id + " : " + e.Message));
            }
        }

        /*
         has node got childs?
         */
        public bool hasChildren(Node node)
        {
            try
            {
                return node.hasChildren;
            }
            catch (Exception e)
            {
                throw (new Exception("Error executing ModeManager.hasChildren(): " + e.Message));
            }
        }

        public int getNodeLevel(Node node)
        {
            return node.level;  //determined when the node is loaded
        }


        public Node[] getBreadcrumb(Node node)
        {
            List<Node> path = new List<Node>();
            Node currNode = node;
            path.Add(currNode);
            while (currNode.parentId > 0)
            {
                currNode = new Node(currNode.parentId,this.TemplateBasePath);
                path.Add(currNode);
            }

            path.Reverse();
  
            return path.ToArray();
        }

        public Node[] getSiblings(Node node)
        {
            try
            {
                Node parent = node.getParent();
                Node[] siblingArray = parent.getChildren();
                for (int a = 0; a < siblingArray.Length; a++)
                {
                    if (siblingArray[a].id == node.id)
                    {
                        siblingArray[a].isCurrentNode = true;
                    }
                    else
                    {
                        siblingArray[a].isCurrentNode = false;
                    }
                }
                return siblingArray;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}

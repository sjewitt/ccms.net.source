using System;
using System.Collections.Generic;
using System.Text;
using ccms.utils;

namespace ccms
{
    /// <summary>
    /// CCMS editor utility methods
    /// </summary>
    public class EditUtils
    {
        public static int CONTENTMAP_ADD = 1;
        public static int CONTENTMAP_UPDATE = 2;

        public string treeListHTML = null;

        public EditUtils()
        {

        }

        /// <summary>
        /// Return DateTime object based on input string yyyy-mm-dd (http://www.openjs.com/scripts/ui/calendar/) 
        /// </summary>
        /// <param name="dateString">Input string in format yyyy-mm-dd</param>
        /// <returns></returns>
        public DateTime getDateFromString(string dateString)
        {
            try
            {
                string[] dateBits = dateString.Split('-');

                if (dateBits[1].StartsWith("0")) dateBits[1] = dateBits[1].Substring(1, 1);
                if (dateBits[2].StartsWith("0")) dateBits[2] = dateBits[2].Substring(1, 1);

                DateTime date = new DateTime(
                    Int32.Parse(dateBits[0]), //yyyy
                    Int32.Parse(dateBits[1]), //mm
                    Int32.Parse(dateBits[2])  //dd
                    );
                return date;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("invalid input format", ex);
            }
        }
        /// <summary>
        /// Build a HTML DOM structure (a nested list) based on supplied node. It is
        /// expected that the Node has been loaded with the optional second loadChilds boolean parameter
        /// set to TRUE.
        /// This is a recursive function and sets this.treeListHTML.
        /// </summary>
        /// <param name="node">a ccms.Node object</param>
        /// <returns>HTML string</returns>
        public string buildViewtreeAsDOM(Node node)
        {
            this.treeListHTML = "";
            _buildTree(node);
            this.treeListHTML = "<ul>"
                + this.treeListHTML
                + "</ul>";
            return this.treeListHTML;
        }
        
        private void _buildTree(Node node)
        {
            try
            {
                Page page = new Page(node.pageId);
                this.treeListHTML += "\n\t<li id=\"node_" + node.id + "\" title=\"" + page.linkText + "\">\n";

                if(node.childCount > 0)
                {
                    this.treeListHTML += "\t\t<ul>\n";
                    for(int a=0;a<node.Childs.Length;a++)
                    {
                        _buildTree(node.Childs[a]);
                    }
                    this.treeListHTML += "\n\t\t</ul>\n";
        
                }
                this.treeListHTML += "\t</li>\n";
            }
            catch(Exception e)
            {
                
            }
        }
    }
}
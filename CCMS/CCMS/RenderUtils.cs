using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace ccms.utils
{
    public class RenderUtils
    {
        /* 
         * initialise the properties etc:
         */
        public RenderUtils()
        {
            AppSettingsReader asr = new AppSettingsReader();
            this.DBDRIVER = (string)asr.GetValue("dbdriver", typeof(string));
            this.DBSERVER = (string)asr.GetValue("dbserver", typeof(string));
            this.DBNAME = (string)asr.GetValue("dbname", typeof(string));
            this.DBUSER = (string)asr.GetValue("dbuser", typeof(string));
            this.DBPASSWORD = (string)(asr.GetValue("dbpassword", typeof(string)));
        }
        private String DBDRIVER = null;
        private String DBSERVER = null;
        private String DBNAME = null;
        private String DBUSER = null;
        private String DBPASSWORD = null;


        public String getConnectionString()
        {
            string connectionString = "Data Source=" + this.DBSERVER + ";";
            connectionString += "User ID=" + this.DBUSER + ";";
            connectionString += "Password=" + this.DBPASSWORD + ";";
            connectionString += "Initial Catalog=" + this.DBNAME;
            return connectionString;
        }

        public static string getErrorPage(Node node,Exception ex)
        {
            //Get HTML content for a simple error page:
            string output = "<html>";
            string nodeStr = node.id.ToString();
            if (node == null)
            {
                nodeStr = "root page";
            }
                
            output += "<head><title>Exception</title></head>";
            output += "<body>";
            output += "<h1>Error occurred node '" + nodeStr + "'</h1>";
            output += "INNER EXCEPTION: " + ex.InnerException + "<br />";
            output += "MESSAGE: " + ex.Message + "<br />";
            output += "SOURCE: " + ex.Source + "<br />";
            output += "STACK TRACE: "+ex.StackTrace + "<br />";
            output += "THROWN BY: " + ex.TargetSite + "<br />";
            output += "</body>";
            output += "</html>";

            return output;
        }
    }
}

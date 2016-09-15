using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using ccms.utils;
using ccms;

namespace ccms
{
    /// <summary>
    /// 
    /// </summary>
    public class Layout
    {
        /*
         * March 2016:
         * Set absolute path at runtime:
         * */
        private string templatePath;

        public string TemplatePath
        {
            get { return templatePath; }
            set { templatePath = value; }
        }


        //constructor for use if the template is stored in DB. This makes table 'layout' obsolete
        //public Layout(int layoutId)
        //{
        //    this._template = (new Template(layoutId));  //do I really need this?
        //    this._source = this.template.Data;
        //}
        
        //constructor for use if template is stored on FS.
        //TODO: retain? 
        public Layout(int layoutId,string TemplateBasePath)
        {
            //get layout template base path:

            this._session = new DBSession();
            this._conn = new SqlConnection(this._session.dbConnStr);
            this._conn.Open();

            SqlDataReader reader = null;

            SqlCommand cmd = new SqlCommand("select layout_url from layout where id = " + layoutId + ";", this._conn);
            reader = cmd.ExecuteReader();
            reader.Read();

            this._sourcepath = TemplateBasePath + reader.GetString(0);
            this._id = layoutId;
            reader.Close();

            //are we web or local FS:
            this._deploymentType = ConfigurationManager.AppSettings["DeploymentType"];
            if (this._deploymentType.Equals("web"))
            {
                //call ASP.NET method to get the base path
            }
            else if (this._deploymentType.Equals("local"))
            {
                //use the filepath as is:
            }
            else
            {
                throw new Exception("cannot determine deployment configuration. Ensure that 'DeploymentType' is added as a key and 'local' or 'web' are set as the value.");
            }

            //and load the source
            //load the file:
            StreamReader streamReader = new StreamReader(this._sourcepath);
            this._source = streamReader.ReadToEnd();
            streamReader.Close();
        }

        private DBSession _session      = null;
        private SqlConnection _conn     = null;



        private int _id;
        public int id {
            get { return this._id; }
        }

        private string _sourcepath;
        public virtual string sourcePath
        {
            get { return this._sourcepath; }
        }

        private string _deploymentType;
        public virtual string deploymentType
        {
            get { return this._deploymentType; }
        }

        private string _source;
        public virtual string source
        {
            get { return this._source; }
        }

        public Slot[] getTemplateSlots()
        {
            try
            {
                Slot[] slots = null;

                return slots;
            }
            catch (Exception ex)
            {
                throw new Exception("cannot retrieve slots from template: " + ex.Message);
            }
        }

        private Template _template;

        public Template template
        {
            get { return _template; }
            set { _template = value; }
        }

    }
}

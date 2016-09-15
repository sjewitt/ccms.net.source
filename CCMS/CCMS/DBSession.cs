using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace ccms.utils
{
    public class DBSession
    {
        public DBSession()
        {
            //Data Source=DAEDELUS;Initial Catalog=ccms_dev;User Id=sa;Password=h154lon;
            string connStr = "Data Source=" + ConfigurationManager.AppSettings["dbserver"];
            connStr += ";Initial Catalog=" + ConfigurationManager.AppSettings["dbname"];
            connStr += ";User Id=" + ConfigurationManager.AppSettings["dbuser"]; ;
            connStr += ";Password=" + ConfigurationManager.AppSettings["dbpassword"] + ";";

            this._dbConnStr = connStr;
        }

        private string _dbConnStr;
        public string dbConnStr
        {
            get { return this._dbConnStr; }
        }
    }
}

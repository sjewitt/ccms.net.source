using System;
using System.Collections.Generic;
using System.Text;
using ccms.utils;
using System.Data.SqlClient;
using System.Data;

namespace ccms
{
    public class Template
    {
        private DBSession _session = null;
        private SqlConnection _conn = null;
        

        public Template()
        {
    
        }

        public Template(int templateId)
        {
            try
            {
                this._session = new DBSession();
                this._conn = new SqlConnection(this._session.dbConnStr);
                this._conn.Open();
                this.id = templateId;

                //TODO: Check that the corresponding row exists

                SqlCommand cmd = new SqlCommand("select name, data from template where active=1 and id=@ID",this._conn);
                cmd.Parameters.AddWithValue("ID", templateId);
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();  //get first (and only) row

                if (!reader.IsDBNull(0)) this.name = reader.GetString(0);
                if (!reader.IsDBNull(1)) this.data = reader.GetString(1);

                reader.Close();
                this._conn.Close();
            }
            catch (Exception ex)
            {
                if (this._conn.State != ConnectionState.Closed)
                {
                    this._conn.Close();
                }
                throw new Exception("cannot load template: " + ex.Message);
            }

            finally
            {
                if (this._conn.State != ConnectionState.Closed)
                {
                    this._conn.Close();
                }
            }
        }


        private int id;

        public int Id
        {
            get { return id; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string data;

        public string Data
        {
            get { return data; }
            set { data = value; }
        }


    }
}

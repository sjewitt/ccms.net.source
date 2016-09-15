using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using ccms.utils;
using System.Data;

namespace ccms
{
    /// <summary>
    /// ORM mapping class.
    /// A Page represents a collection of data ultimately to be rendered
    /// in the context of a Template object.
    /// A Page object defines core HTML page properties (keywords, description etc.),
    /// code CCMS properties (created date/user, name etc.) and related Content (see 
    /// PageContentMap). 
    /// </summary>
    public class Page
    {
        public Page(int pageId)
        {
            try
            {
                this._session = new DBSession();
                this._conn = new SqlConnection(this._session.dbConnStr);
                this._conn.Open();
                this._id = pageId;

                //TODO: Check that the corresponding row exists

                SqlCommand cmd = new SqlCommand("select name, linktext, description, keywords, title, created_user, updated_user,created_date,updated_date,layout_id from page where state=" + State.ACTIVE + " and id = " + pageId + " ;", this._conn);
                
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();  //get first (and only) row

                //if(!reader.IsDBNull(3)) this._layoutId  = reader.GetInt32(3);

                if (!reader.IsDBNull(0)) this._name         = reader.GetString(0);
                if (!reader.IsDBNull(1)) this._linktext     = reader.GetString(1);
                if (!reader.IsDBNull(2)) this._description  = reader.GetString(2);
                if (!reader.IsDBNull(3)) this._keywords     = reader.GetString(3);
                if (!reader.IsDBNull(4)) this._title        = reader.GetString(4);
                if (!reader.IsDBNull(5)) this._createduser  = reader.GetInt32(5);
                if (!reader.IsDBNull(6)) this._updateduser  = reader.GetInt32(6);
                if (!reader.IsDBNull(7)) this._createddate  = reader.GetDateTime(7);
                if (!reader.IsDBNull(8)) this._updateddate  = reader.GetDateTime(8);
                if (!reader.IsDBNull(9)) this._layoutid     = reader.GetInt32(9);
                reader.Close();
                this._conn.Close();
            }
            catch (Exception ex)
            {
                if (this._conn.State != ConnectionState.Closed)
                {
                    this._conn.Close();
                }
                throw new Exception("cannot load page: " + ex.Message);
            }
        }

        //empty constructor
        public Page()
        {
        }

        private SqlConnection _conn;
        private DBSession _session;

        private int _id;
        public int id
        {
            get { return this._id; }
        }

        private int _state = -1;
        public int state
        {
            get { return this._state; }
            set { this._state = value; }
        }

        private string _name = "";
        public string name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        private string _linktext = "";
        public string linkText
        {
            get { return this._linktext; }
            set { this._linktext = value; }
        }

        private string _title = "";
        public string title
        {
            get { return this._title; }
            set { this._title = value; }
        }

        private string _description = "";
        public string description
        {
            get { return this._description; }
            set { this._description = value; }
        }

        private string _keywords = "";
        public string keywords
        {
            get { return this._keywords; }
            set { this._keywords = value; }
        }

        private DateTime _createddate;
        public DateTime createdDate
        {
            get { return this._createddate; }
        }

        private DateTime _updateddate;
        public DateTime updatedDate
        {
            get { return this._updateddate; }
        }
        
        private int _createduser;
        public int createdUser
        {
            get { return this._createduser; }
        }

        private int _updateduser;
        public int updatedUser
        {
            get { return this._updateduser; }
        }

        private int _layoutid;
        public int layoutId
        {
            get { return this._layoutid; }
            set { this._layoutid = value; }
        }

        public Content[] getContent()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            { throw new Exception("cannot retrieve content array: " + ex.Message);}
        }

        //get the slots for this page. A slot has associated content, possibly:
        public Slot[] getSlots()
        {
            

            return null;
        }
    }
}

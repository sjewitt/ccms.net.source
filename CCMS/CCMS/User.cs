using System;
using System.Collections.Generic;
using System.Text;
using ccms.managers;
using ccms.utils;
using System.Data.SqlClient;

namespace ccms
{
    /// <summary>
    /// ORM mapping class.
    /// Represents a User.
    /// </summary>
    public class User
    {
        
        
        //build new user
        public User()
        {
        }

        //load existing user
        public User(int userId)
        {
            this.loadById(userId);
            //get user details:
        }

        private int _id;
        public int id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        private string _login;
        public string login
        {
            get { return this._login; }
            set { this._login = value; }
        }

        private string _fullName;
        public string fullName
        {
            get { return this._fullName; }
            set { this._fullName = value; }
        }

        private string _email;
        public string email
        {
            get { return this._email; }
            set { this._email = value; }
        }

        private string _password;
        public string password
        {
            get { return this._password; }
            set { this._password = value; }
        }

        private bool _active;
        public bool active
        {
            get { return this._active; }
            set { this._active = value; }
        }

        private int _permissions;
        public int permissions
        {
            get { return this._permissions; }
            set { this._permissions = value; }
        }

        //TODO:
        private object _groups;
        public object Groups
        {
            get { return _groups; }
            set { _groups = value; }
        }


        public bool loadById(int userId)
        {
            try
            {
                bool returnVal = false;

                DBSession session = new DBSession();
                SqlConnection conn = new SqlConnection(session.dbConnStr);

                conn.Open();

                SqlDataReader reader = null;
                SqlCommand cmd = new SqlCommand("select id,login,password,fullname,email,active,permissions,groups from users where id="+userId, conn);

                //TODO: Add checking around num results etc:
                reader = cmd.ExecuteReader();
                reader.Read();
                
                this.id = reader.GetInt32(0);
                this.login = reader.GetString(1);
                this.password = reader.GetString(2);
                this.fullName = reader.GetString(3);
                this.email = reader.GetString(4);
                this.active = reader.GetBoolean(5);
                this.permissions = reader.GetInt32(6);
                this.Groups = reader.GetInt32(7);   //TODO: Check for null!

                conn.Close();

                return returnVal;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

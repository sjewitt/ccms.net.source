using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using ccms.utils;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using ccms;

namespace ccms.managers
{
    public class UserManager
    {
        //constants:
        public const int USER_ACTION_CREATE = 1;
        public const int USER_ACTION_UPDATE = 2;

        private DBSession session = null;
        public UserManager(DBSession session)
        {
            this.session = session;
        }

        public User loginUser(LoginWrapper loginWrapper)
        {
            try
            {
                User user = null;

                //login a user:

                SqlConnection conn = new SqlConnection(this.session.dbConnStr);
                conn.Open();

                SqlDataReader reader = null;
                SqlCommand cmd = new SqlCommand("select count (*) as count from users where active=" + State.ACTIVE + " and login='" + loginWrapper.Username + "' and password='" + loginWrapper.Password + "'", conn);
                reader = cmd.ExecuteReader();
                reader.Read();
                int count = 0;
                //string countSql = "select count (*) as count from users where active=" + State.ACTIVE + " and login='" + login + "' and password='" + password + "'";
                //recordset = connection.execute(countSql);

                count = reader.GetInt32(0);
                reader.Close();
                if (count == 1)
                {
                    cmd = new SqlCommand("select id from users where active=" + State.ACTIVE + " and login='" + loginWrapper.Username + "' and password='" + loginWrapper.Password + "'", conn);
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    int userId = reader.GetInt32(0);
                    user = new User(userId);
                    reader.Close();
                }

                conn.Close();
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<User> getUsers()
        {
            List<User> users = new List<User>();

            try
            {
                SqlConnection conn = new SqlConnection(this.session.dbConnStr);
                conn.Open();


                SqlDataReader reader = null;
                SqlCommand cmd = new SqlCommand("select id from users", conn);

                //TODO: Add checking around num results etc:
                reader = cmd.ExecuteReader();

                //get first row:
                User _user;
                while (reader.Read())
                {
                    _user = new User();
                    _user.loadById(reader.GetInt32(0));
                    users.Add(_user); //().loadById());   //.loadById(reader.GetInt32(0));
                }
                conn.Close();
                return (List<User>)users;
            }
            catch (SqlException ex)
            {
                string x = ex.Message;
                return null;
            }
            
        }

        //TODO: get logged in user by ASP.NET Session
        public User getCurrentUser()
        {
            User currentUser = null;
            SqlConnection conn = new SqlConnection(this.session.dbConnStr);
            conn.Open();

            conn.Close();
            return currentUser;
        }

        //get logged in user by bespoke session management:
        public User getCurrentUser(string sessionId)
        {
            try
            {
                User currentUser = null;
                SqlConnection conn = new SqlConnection(this.session.dbConnStr);

                conn.Open();

                SqlDataReader reader = null;
                SqlCommand cmd = new SqlCommand("select user_id from session_data where session_id = '" + sessionId + "';", conn);

                //TODO: Add checking around num results etc:
                reader = cmd.ExecuteReader();

                //get first row:
                reader.Read();
                currentUser = new User();
                currentUser.loadById(reader.GetInt32(0));
                reader.Close();
                conn.Close();
                return currentUser;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //get existing user:
        public User getUser(int userId)
        {
            try
            {
                User user = new User(userId);
                return user;
            }
            catch (Exception e) { throw (e); }
        }

        public void createUser(User user)
        {
            try
            {
                SqlConnection conn = new SqlConnection(this.session.dbConnStr);
                conn.Open();
                string sql = "insert into users(login,password,fullname,email,permissions,groups,active) values ('" + user.login + "', '" + user.password + "', '" + user.fullName + "', '" + user.email + "'," + user.permissions + ",'" + user.Groups + "','" + user.active + "');";
                Console.WriteLine(sql);
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e) { throw (e); }
        }

        public void updateUser(User user)
        {
            try
            {
                SqlConnection conn = new SqlConnection(this.session.dbConnStr);
                conn.Open();

                string updateSQL = "update users set login='" + user.login
                    + "',password='" + user.password
                    + "',fullname='" + user.fullName
                    + "',email='" + user.email
                    + "',permissions=" + user.permissions
                    + ",groups='" + user.Groups
                    + "',active='" + user.active
                    + "' where id=" + user.id + ";";
                //Console.WriteLine(updateSQL);
                SqlCommand cmd = new SqlCommand(updateSQL, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception e) { throw (e); }
        }

        public void deleteUser(User user)
        {
            try
            {
                SqlConnection conn = new SqlConnection(this.session.dbConnStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("delete from users where id=" + user.id, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e) { throw (e); }
        }

        public User evaluateUserSessionData(string serialisedXML)
        {
            try
            {

                /*
                public static object FromXml(string Xml, System.Type ObjType)
                {

                XmlSerializer ser;
                ser = new XmlSerializer(ObjType);
                StringReader stringReader;
                stringReader = new StringReader(Xml);
                XmlTextReader xmlReader;
                xmlReader = new XmlTextReader(stringReader);
                object obj;
                obj = ser.Deserialize(xmlReader);
                xmlReader.Close();
                stringReader.Close();
                return obj;

                }
                 
                 */
                User user = new User();
                XmlSerializer serializer = new XmlSerializer(user.GetType());
                StringReader stringReader = new StringReader(serialisedXML);
                XmlTextReader xmlReader = new XmlTextReader(stringReader);

                user = (User)serializer.Deserialize(xmlReader);
                xmlReader.Close();
                stringReader.Close();
                return user;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public string getSerialisedUserData(User user)
        {
            try
            {
                XmlSerializer serializer =  new XmlSerializer(user.GetType());
                StringWriter stringWriter = new StringWriter();

                serializer.Serialize(stringWriter, user);
                
                //return user as XML;
                return(stringWriter.GetStringBuilder().ToString());
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using ccms.utils;
using System.Data.SqlClient;
using System.Web;

namespace ccms.managers
{
    public class SessionManager
    {
        private DBSession session;

        public DBSession Session
        {
            get { return session; }
            set { session = value; }
        }

        public SessionManager(DBSession session)
        {
            this.Session = session;
        }

        /// <summary>
        /// Create a session stored in the database.
        /// </summary>
        /// <param name="user">The user object whose data should be stored in the session.</param>
        /// <param name="sessionId">The ASP.NET session ID to use as lookup key.</param>
        /// <returns>True on successful storage, false otherwise.</returns>
        public bool createSession(User user,string sessionId)
        {
            try
            {
                //TODO!!! Parameterise this!
                SqlConnection conn = new SqlConnection(this.session.dbConnStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into session_data(session_id,user_id,user_data,active,session_opened) values ('" 
                  + sessionId + "',"
                  + user.id +",'"
                  + (new UserManager(session)).getSerialisedUserData(user).Replace("'","''") 
                  +"',1,GETDATE());", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieve User object from session database for specified session ID.
        /// </summary>
        /// <param name="sessionId">The session ID to retrieve user data for.</param>
        /// <returns>A User object on success, or null otherwise.</returns>
        public User getLoggedInUser(string sessionId)
        {
            try
            {
                User user = null;
                //DBSession session = new DBSession();
                UserManager userManager = new UserManager(this.session);
                
                //retrieve session from DB:
                SqlConnection conn = new SqlConnection(this.session.dbConnStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select user_data from session_data where session_id = '" + sessionId + "' and active=1;", conn);

                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                string userData = reader.GetString(0);

                conn.Close();
                
                //rehydrate the XML:
                user = userManager.evaluateUserSessionData(userData);

                
                return user;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Expire session for specified session ID.
        /// </summary>
        /// <param name="sessionId">A valid session ID. Note that if the specified session is already expired
        /// or does not exist, this method will have no effect.</param>
        public void expireSession(string sessionId)
        {
            try
            {
                //retrieve session from DB:
                SqlConnection conn = new SqlConnection(this.session.dbConnStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("update session_data set active=0,session_closed=GETDATE() where session_id = '" + sessionId + "';", conn);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {

            }
        }

        //public bool checkSession(string sessionId)
        public bool checkSession(HttpCookie sessionCookie)
        {
            bool returnVal = false;
            try
            {
                if (sessionCookie != null)  //there is a session...
                {
                    User user = this.getLoggedInUser(sessionCookie.Value);
                    if (user != null)   //there is a user
                    {
                        returnVal = true;
                        //and check perms here...
                    }
                }
                return returnVal;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}

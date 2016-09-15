using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccms
{
    public class LoginWrapper
    {
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private bool success;

        public bool Success
        {
            get { return success; }
            set { success = value; }
        }

        private string sessionKey;

        public string SessionKey
        {
            get { return sessionKey; }
            set { sessionKey = value; }
        }
    }
}

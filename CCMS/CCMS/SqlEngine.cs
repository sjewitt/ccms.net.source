using System;
using System.Collections.Generic;
using System.Text;
using ccms.utils;
using System.Collections;
using System.Data.SqlClient;

namespace ccms
{
    public class SqlEngine
    {
        public SqlEngine()
        {
            dbSession = new DBSession();
        }

        private DBSession dbSession;

    }
}

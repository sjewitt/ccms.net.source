using System;
using System.Collections.Generic;
using System.Text;

namespace ccms
{
    /// <summary>
    /// State class simply defines  edit cycle status codes:
    /// ACTIVE - can be rendered
    /// ATWORK - At work
    /// PENDING - Awaiting approval
    /// EXPIRED - Content that has exceeded its valid-till date (if populated)
    /// REJECTED - Content that has been rejected.
    /// </summary>
    public static class State
    {
        public const int ACTIVE     = 1;
        public const int ATWORK     = 2;
        public const int PENDING    = 3;
        public const int EXPIRED    = 4;
        public const int REJECTED   = 5;

        public static string[] STATES = { "ACTIVE", "ATWORK", "PENDING", "EXPIRED", "REJECTED" };
    }
}

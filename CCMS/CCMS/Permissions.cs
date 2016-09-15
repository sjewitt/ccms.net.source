using System;
using System.Collections.Generic;
using System.Text;

namespace ccms
{
    public static class Permissions
    {
        public const int ADMINISTRATOR = 1;    //all rights
        public const int CHANGESTATE = 2;    //set content to active right
        public const int CREATEPAGE = 4;    //
        public const int CREATECONTENT = 8;    //
        public const int EDITPAGE = 16;   //add/remove from slots, page core props
        public const int EDITCONTENT = 32;   //
        public const int DELETEPAGE = 64;   //
        public const int DELETECONTENT = 128;  //
        public const int BROWSEONLY = 256;  //no edit rights, just logged in
        public const int EDITVIEWTREE = 512;  //edit node position in viewtree
        public const int UPLOADBINARY = 1024; //upload images/documents
        public const int MANAGELAYOUT = 2048;
    }
}

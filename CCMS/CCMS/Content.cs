using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ccms
{
    /// <summary>
    /// A Content object is the logical parent object of a collection of
    /// ContentInstance objects.
    /// </summary>
    public class Content
    {
        public Content()
        {
            //constructor
            string xx = "fish";
        }

        private string _name;
        public string name
        {
            get{return _name;}
            set {this._name  = value;}
        }

        private int _id;
        public int id
        {
            get{return _id;}
            set {this._id  = value;}
        }

        private int _authgroup;
        public int authGroup
        {
            get{return _authgroup;}
            set {this._authgroup  = value;}
        }

        private int _createduser;
        public int createdUser
        {
            get{return _createduser;}
            set {this._createduser  = value;}
        }

        private DateTime _createddate ;
        public DateTime createdDate
        {
            get{return _createddate;}
            set {this._createddate  = value;}
        }

        private int _updateduser ;
        public int updatedUser
        {
            get{return _updateduser;}
            set {this._updateduser  = value;}
        }

        private DateTime _updateddate;
        public DateTime updatedDate
        {
            get{return _updateddate;}
            set {this._updateddate  = value;}
        }

        private int _contenttype;
        public int contentType
        {
            get { return _contenttype; }
            set { this._contenttype = value; }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private DateTime startdate;
        public DateTime StartDate
        {
            get { return startdate; }
            set { this.startdate = value; }
        }

        private DateTime enddate;
        public DateTime EndDate
        {
            get { return enddate; }
            set { this.enddate = value; }
        }
    }
}

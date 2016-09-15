using System;
using System.Collections.Generic;
using System.Text;

namespace ccms
{
    public class ContentInstance
    {
        /// <summary>
        /// Represents an editable piece of text. Each Content object is associated
        /// with one or more ContentInstance objects.
        /// </summary>
        public ContentInstance()
        {
        }

        private int _id = 0;
        public virtual int id
        {
            get { return _id; }
            set { this._id = value; }
        }

        private int _contentid = 0;
        public virtual int contentId
        {
            get { return _contentid; }
            set { this._contentid = value; }
        }

        private string _name = null;
        public virtual string name
        {
            get { return _name; }
            set { this._name = value; }
        }

        private int _versionid = 0;
        public virtual int versionId
        {
            get { return _versionid; }
            set { this._versionid = value; }
        }

        private int _state;
        public virtual int state 
        {
            get { return _state; }
            set { this._state = value; }
        }

        private string _data;
        public virtual string data
        {
            get { return _data; }
            set { this._data = value; }
        }

        private int _edituser;
        public virtual int editUser
        {
            get { return _edituser; }
            set { this._edituser = value; }
        }

        private DateTime _updateddate;
        public virtual DateTime updatedDate
        {
            get { return _updateddate; }
            set { this._updateddate = value; }
        }
    }
}

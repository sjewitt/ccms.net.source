using System;
using System.Collections.Generic;
using System.Text;

namespace ccms.utils
{
    public class PageContentMap
    {
        /// <summary>
        /// ORM Mapping class.
        /// A PageContentMap instance holds the three-way mapping between a Node (the
        /// hierarchical position in the viewtree), a corresponding Page and the Content
        /// object(s) that are associated with that Page.
        /// </summary>
        public PageContentMap()
        {
        }

        private int _contentid;
        public virtual int contentId
        {
            get { return _contentid; }
            set { this._contentid = value; }
        }

        private int _pageid;
        public virtual int pageId
        {
            get { return _pageid; }
            set { this._pageid = value; }
        }

        private int _slotnum;
        public virtual int slotNum
        {
            get { return _slotnum; }
            set { this._slotnum = value; }
        }

        //required over-ridden method:
        public override bool Equals(object obj)
        {
            bool equals = false;
            if (null != obj)
            {
                PageContentMap temp = obj as PageContentMap;
                if (null != temp)
                {
                    equals = (this.contentId == temp.contentId) && (this.pageId == temp.pageId) && (this.slotNum == temp.slotNum);
                }
            }
            return equals;
        }

        //required over-ridden method:
        public override int GetHashCode()
        {
            int hash = 1122;
            hash += (0 == this.contentId ? 0 : this.contentId.GetHashCode());
            hash += (0 == this.pageId ? 0 : this.pageId.GetHashCode());
            hash += (0 == this.slotNum ? 0 : this.slotNum.GetHashCode());
            return hash;
        }
    }
}

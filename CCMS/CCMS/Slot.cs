using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ccms
{
    /// <summary>
    /// Abstraction of a content placeholder within a template string. A slot
    /// appears in the source text as {CCMS_CONTENT_n} where 'n' is the slot number.
    /// 
    /// </summary>
    public class Slot
    {
        public Slot(string slotTag,int nodeId)
        {
            //map to CMS_CONTENT_n here using the tag string.
            int slotId = -1;
            string[] split1 = { "{CMS_CONTENT_" };
            string[] split2 = { "}"};
            this._pageid = pageId;
            
            this.slotTag = slotTag;
            if (int.TryParse(
                this.slotTag.Split(split1, StringSplitOptions.None)[1].Split(split2, StringSplitOptions.None)[0], 
                out slotId))
            {
                this.slotId = slotId;
            }
            
        }
        private int _slotid;
        public int slotId
        {
            set { this._slotid = value; }
            get { return this._slotid; }
        }

        private int _pageid;
        public int pageId
        {
            set { this._pageid = value; }
            get { return this._pageid; }
        }

        private string _slottag;
        public string slotTag
        {
            set { this._slottag = value; }
            get { return this._slottag; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using ccms;

namespace ccms
{
    /// <summary>
    /// Logical representation of a CCMS tag. 
    /// </summary>
    class Tag
    {
        private Slot _slot;

        public Tag(Slot slot)
        {
            this._slot = slot;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public object getProperty(string tag)
        {
            return null;
        }

        public int getContentCreatedUser()
        {
            return -1;
        }

        public int getContentUpdatedUser()
        {
            return -1;
        }

        public DateTime getContentCreatedDate()
        {
            return new DateTime();
        }

        public DateTime getContentUpdatedDate()
        {
            return new DateTime();
        }
    }
}

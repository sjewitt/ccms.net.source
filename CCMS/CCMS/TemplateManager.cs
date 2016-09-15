using System;
using System.Collections.Generic;
using System.Text;
using ccms.utils;
using System.Collections;

namespace ccms
{
    public class TemplateManager
    {
        private DBSession session;

        /// <summary>
        /// Manage Template objects.
        /// </summary>
        /// <param name="session">Database connection object</param>
        public TemplateManager(DBSession session)
        {
            this.session = session;
        }

        /// <summary>
        /// Return a Template object by it's ID
        /// </summary>
        /// <param name="templateId">The unique ID of the Template to retrieve</param>
        /// <returns>The retrieved Template object, or null</returns>
        public Template getTemplate(int templateId)
        {
            return null;
        }

        /// <summary>
        /// Update template details in the database. Requires a valid Template object, including ID.
        /// </summary>
        /// <param name="template">Template object containing updated information. It must include an ID of 
        /// an existing template to successfully update.</param>
        /// <returns>The updated Template object, or null</returns>
        public Template updateTemplate(Template template)
        {
            return null;
        }

        /// <summary>
        /// Insert new template details in the database. Requires a valid Template object, but not including ID.
        /// </summary>
        /// <param name="template">A valid Template object. The ID is potional and is ignored by this method</param>
        /// <returns>The new Template object, or null</returns>
        public Template addTemplate(Template template)
        {
            return null;
        }

        /// <summary>
        /// get all templates in database in a List
        /// </summary>
        /// <returns></returns>
        public List<Template> getTemplates()
        {
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;
using ccms.utils;
using ccms;

/// <summary>
/// ObjectFactory does stuff...
/// The various Managers obtained from here provide both retrieval and 
/// create/update methods.
/// </summary>


namespace ccms.managers
{
    public class ObjectFactory
    {
        public ObjectFactory()
        {
            this._session = new DBSession();
        }

        //March 2016
        public ObjectFactory(string TemplateBasePath)
        {
            this._session = new DBSession();
            this.templateBasePath = TemplateBasePath;
        }

        private string templateBasePath;
        public string TemplateBasePath
        {
            get { return templateBasePath; }
            set { templateBasePath = value; }
        }

        private DBSession _session;
        public DBSession session
        {
            get { return this._session; }
        }

        public UserManager getUserManager()
        {
            return new UserManager(this.session);
        }

        public NodeManager getNodeManager()
        {
            return new NodeManager(this.session,this.TemplateBasePath);
        }

        public PageManager getPageManager()
        {
            return new PageManager(this.session);
        }

        public ContentManager getContentManager()
        {
            return new ContentManager(this.session);
        }

        public LayoutManager getLayoutManager()
        {
            return new LayoutManager(this.session, this.TemplateBasePath);
        }

        public TemplateManager getTemplateManager()
        {
            return new TemplateManager(this.session);
        }
    }
}

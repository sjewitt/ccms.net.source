using System;
using System.Collections.Generic;
using System.Text;
using ccms.utils;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Collections;

namespace ccms.managers
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentManager
    {
        private DBSession session = null;
        public ContentManager(DBSession session)
        {
            this.session = session;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns>Returns an IList of PageContentMap instances associated with the supplied 
        /// Page object.</returns>
        public IList<PageContentMap> getPageContentMap(Page page)
        {
            try
            {
                IList<PageContentMap> map = new List<PageContentMap>();
                SqlConnection conn = new SqlConnection(session.dbConnStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select content_id, page_id, slot_num from page_content_ref where page_id=" + page.id,conn);
                SqlDataReader reader = cmd.ExecuteReader();
                //"from PageContentMap map where map.pageId=" + page.id
                PageContentMap _map;
                while (reader.Read())
                {
                    _map = new PageContentMap();
                    _map.contentId = reader.GetInt32(0);
                    _map.pageId = reader.GetInt32(1);
                    _map.slotNum = reader.GetInt32(2);
                    map.Add(_map);
                }
                conn.Close();
                return map;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //TODO: I need a RenderManager to build the complex object that
        //includes the Node, the Page and associated Content object(s) + ACTIVE ContentInstance(s)

        public Content getContentItem(int contentId)
        {
            try
            {
                SqlConnection conn = new SqlConnection(session.dbConnStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select id, name, created_user,created_date,updated_user,updated_date,auth_group,content_type,description,start_date,end_date from content where id=" + contentId, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                Content content = null;
                reader.Read();
                if (reader.HasRows)
                {
                    content = new Content();
                    content.id = reader.GetInt32(0);
                    content.name = reader.GetString(1);
                    content.createdUser = reader.GetInt32(2);
                    content.createdDate = reader.GetDateTime(3);
                    content.updatedUser = reader.GetInt32(4);
                    content.updatedDate = reader.GetDateTime(5);
                    content.authGroup = reader.GetInt32(6);
                    content.contentType = reader.GetInt32(7);
                    if (!reader.IsDBNull(8)) content.Description = reader.GetString(8);
                    if (!reader.IsDBNull(9)) content.StartDate = reader.GetDateTime(9);
                    if (!reader.IsDBNull(10)) content.EndDate = reader.GetDateTime(10);
                }
                conn.Close();
                return content;
            }
            catch (Exception ex)
            {
                //Content c = new Content();
                //c.Description = ex.Message;
                return null;
            }
        }

        public List<Content> getContentItems(int contentType = -1, int[] contentIdArray = null)
        {
            List<Content> output = new List<Content>();
            //create and open a connection:
            SqlConnection conn = new SqlConnection(session.dbConnStr);
            conn.Open();
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;

                //add the new entry into the content_instance table:
                cmd.CommandText = "select id from content";
                if (contentType > -1)
                {
                    cmd.CommandText += " where content_type=@content_type";
                }
                if (contentIdArray != null)
                {
                    //check the values TODO!!!
                }
                cmd.CommandText += " order by name";
                cmd.CommandText += ";";
                cmd.Parameters.Clear();
                //string name = content.name;
                //if (name.Length > 50) name = name.Substring(0, 50);
                cmd.Parameters.AddWithValue("@content_type", contentType);

                SqlDataReader reader =  cmd.ExecuteReader();
                ccms.Content _current = null;
                int _currentContentId = -1;
                while(reader.Read())
                {
                    if (!reader.IsDBNull(0)) _currentContentId  = reader.GetInt32(0);
                    _current = this.getContentItem(_currentContentId);
                    output.Add(_current);
                }

                return output;
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool updateContentProperties(Content content)
        {
            //create and open a connection:
            SqlConnection conn = new SqlConnection(session.dbConnStr);
            conn.Open();

            try
            {
                //TODO: REST OF FIELDS!
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;

                //add the new entry into the CONTENT table:
                cmd.CommandText = "update content set name=@CONTENTNAME,updated_user=@UPDATEDUSER,auth_group=@AUTHGROUP,content_type=@CONTENTTYPE,description=@DESCRIPTION,start_date=@STARTDATE,end_date=@ENDDATE where id=@ID;";
                cmd.Parameters.Clear();
                string name = content.name;
                if (name.Length > 50) name = name.Substring(0, 50);
                cmd.Parameters.AddWithValue("@CONTENTNAME", name);
                cmd.Parameters.AddWithValue("@UPDATEDUSER", content.updatedUser);
                //cmd.Parameters.AddWithValue("@STARTDATE", content.StartDate);
                //cmd.Parameters.AddWithValue("@ENDDATE", content.EndDate);
                cmd.Parameters.AddWithValue("@AUTHGROUP", content.authGroup);
                cmd.Parameters.AddWithValue("@CONTENTTYPE", content.contentType);
                cmd.Parameters.AddWithValue("@DESCRIPTION", content.Description);
                cmd.Parameters.AddWithValue("@ID", content.id);

                try
                {
                    cmd.Parameters.AddWithValue("@STARTDATE", new SqlDateTime(content.StartDate));
                }
                catch (Exception ex)
                {
                    cmd.Parameters.AddWithValue("@STARTDATE", DBNull.Value);
                }

                try
                {
                    cmd.Parameters.AddWithValue("@ENDDATE", new SqlDateTime(content.EndDate));
                }
                catch (Exception ex)
                {
                    cmd.Parameters.AddWithValue("@ENDDATE", DBNull.Value);
                }
                if (cmd.ExecuteNonQuery() == 1)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Update failed.");
                }
            }
            catch (Exception e)
            {
                conn.Close();
                throw new Exception("Error updating content item: " + e);
            }
            finally
            {
                conn.Close();
            }
        }

        //get instancelist associated with supplied content
        public IList<ContentInstance> getInstanceList(Content content)
        {
            try
            {
                IList<ContentInstance> _list = new List<ContentInstance>();
                SqlConnection conn = new SqlConnection(session.dbConnStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select id,version_id from content_version where content_id = @contentid order by version_id;",conn);
                cmd.Parameters.AddWithValue("@contentid",content.id);
                SqlDataReader reader = cmd.ExecuteReader();
                //ContentInstance _inst;
                //int _versionId;
                //int _id;
                while (reader.Read())
                {
                    _list.Add(this.getInstance(content,reader.GetInt32(1))); //TODO - complete the ContentInstance class.
                }
                return _list;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        /// <summary>
        /// Retrieve the most recent State.ACTIVE instance.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public ContentInstance getActiveInstance(Content content)
        {
            //     var instSQL = "select top 1 (id) from content_version where content_id = " + this.id +" and state_id=" + State.ACTIVE +" order by version_id desc";
            //TODO: Parameterise the SQL
            try
            {
                ContentInstance contentInstance = null;
                SqlConnection conn = new SqlConnection(session.dbConnStr);
                conn.Open();
                //string sql = "select content_id, version_id, state_id, data, edit_user, updated_date, id from content_version where id in (select top 1 (id) from content_version where content_id = " + content.id + " and state_id=" + State.ACTIVE + " order by version_id desc)";
                string sql = "select content_id,version_id, state_id, data, edit_user, cv.updated_date, cv.id, c.name from 	content_version cv, content  c where cv.id in (select top 1 (id) from content_version where content_id = " + content.id + " and state_id=" + State.ACTIVE + " order by version_id desc) and cv.content_id = c.id;";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    contentInstance = new ContentInstance();
                    contentInstance.id = reader.GetInt32(6);
                    contentInstance.name = reader.GetString(7);
                    contentInstance.contentId = reader.GetInt32(0);
                    contentInstance.versionId = reader.GetInt32(1);
                    contentInstance.state = reader.GetInt32(2);
                    if (!reader.IsDBNull(3)) contentInstance.data = reader.GetString(3);
                    contentInstance.editUser = reader.GetInt32(4);
                    contentInstance.updatedDate = reader.GetDateTime(5);
                    conn.Close();
                }
                return contentInstance;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error returning ContentInstance: " + ex.Message);
                return null;
            }
        }

        public ContentInstance createContentInstance(ContentInstance oldInstance,int state,User user)
        {
            //create and open a connection:
            SqlConnection conn = new SqlConnection(session.dbConnStr);
            conn.Open();
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;

                //add the new entry into the content_instance table:
                cmd.CommandText = "declare @id int;"
                    + "select @id = max(version_id) +1 from content_version cv2 where cv2.content_id=@CONTENTID;"
                    + "insert into content_version"
                    + "(content_id,version_id,state_id,data,edit_user,updated_date)"
                    + "values"
                    + "(@CONTENTID,@id,@STATEID,@DATA,@USERID,GETDATE());";
                cmd.Parameters.Clear();
                //string name = content.name;
                //if (name.Length > 50) name = name.Substring(0, 50);
                cmd.Parameters.AddWithValue("@CONTENTID", oldInstance.contentId);
                //cmd.Parameters.AddWithValue("@VERSIONID", (oldInstance.versionId + 1));
                cmd.Parameters.AddWithValue("@STATEID", state);
                cmd.Parameters.AddWithValue("@DATA", oldInstance.data);
                cmd.Parameters.AddWithValue("@USERID", user.id);

                int result = cmd.ExecuteNonQuery();

                ContentInstance newInstance = null;
                if (result == 1)
                {
                    newInstance = this.getInstance(this.getContentItem(oldInstance.contentId), oldInstance.versionId + 1);
                } 
                return newInstance;
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        
        /// <summary>
        /// Retrieve a content instance
        /// </summary>
        /// <param name="content">A Content object</param>
        /// <param name="instanceId">Integer instance ID</param>
        /// <returns></returns>
        public ContentInstance getInstance(Content content, int instanceId)
        {
            try
            {
                SqlConnection conn = new SqlConnection(session.dbConnStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select content_id, version_id, state_id, data, edit_user, updated_date from content_version where version_id=@instanceid and content_id = @contentid", conn);
                cmd.Parameters.AddWithValue("@instanceid", instanceId);
                cmd.Parameters.AddWithValue("@contentid", content.id);
                
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                ContentInstance contentInstance = new ContentInstance();
                contentInstance.contentId = reader.GetInt32(0);
                contentInstance.versionId = reader.GetInt32(1);
                contentInstance.state = reader.GetInt32(2);
                if (!reader.IsDBNull(3)) contentInstance.data = reader.GetString(3);
                contentInstance.editUser = reader.GetInt32(4);
                contentInstance.updatedDate = reader.GetDateTime(5);
                conn.Close();
                return contentInstance;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Update content instance, without incrementing the version.
        /// </summary>
        /// <param name="contentInstance"></param>
        /// <returns>boolean success or failure</returns>
        public bool updateInstance(ContentInstance contentInstance)
        {
            ContentInstance _ci = contentInstance;
            try
            {
                SqlConnection conn = new SqlConnection(session.dbConnStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("update content_version"
                    + " set data=@data, "
                    + " state_id = @state,"
                    + " edit_user = @edit_user,"
                    + " updated_date=GETDATE()"
                    + " where version_id=@instanceid and content_id = @contentid", conn);
                cmd.Parameters.AddWithValue("@instanceid", contentInstance.versionId);
                cmd.Parameters.AddWithValue("@contentid", contentInstance.contentId);
                cmd.Parameters.AddWithValue("@data", contentInstance.data);
                cmd.Parameters.AddWithValue("@state", contentInstance.state);
                cmd.Parameters.AddWithValue("@edit_user", contentInstance.editUser);

                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                //handle error
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public int createContentItem(Content content,string instanceText)
        {
            int newContentId = 0;
            int returnVal = 0;

            //create and open a connection:
            SqlConnection conn = new SqlConnection(session.dbConnStr);
            conn.Open();
            
            //begin a SQL transaction
            SqlTransaction objTrans = conn.BeginTransaction();
            
            try
            {
                //TODO: REST OF FIELDS!
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = objTrans;
                
                //add the new entry into the CONTENT table:
                cmd.CommandText = "insert into content (name,created_user,created_date,updated_date,updated_user,auth_group,content_type,description,start_date,end_date) values(@CONTENTNAME,@CREATEDUSER,@CREATEDDATE,@CREATEDDATE,@CREATEDUSER,@AUTHGROUP,@CONTENTTYPE,@DESCRIPTION,@STARTDATE,@ENDDATE)";
                cmd.Parameters.Clear();
                string name = content.name;
                if(name.Length > 50) name = name.Substring(0,50);
                cmd.Parameters.AddWithValue("@CONTENTNAME", name);
                cmd.Parameters.AddWithValue("@CREATEDUSER", content.createdUser);
                cmd.Parameters.AddWithValue("@CREATEDDATE", content.createdDate);
                cmd.Parameters.AddWithValue("@AUTHGROUP", content.authGroup);
                cmd.Parameters.AddWithValue("@CONTENTTYPE", content.contentType);
                cmd.Parameters.AddWithValue("@DESCRIPTION", content.Description);
                
                try
                {
                    cmd.Parameters.AddWithValue("@STARTDATE", new SqlDateTime(content.StartDate));
                }
                catch(Exception ex)
                {
                    cmd.Parameters.AddWithValue("@STARTDATE", DBNull.Value);
                }

                try
                {
                    cmd.Parameters.AddWithValue("@ENDDATE", new SqlDateTime(content.EndDate));
                }
                catch (Exception ex)
                {
                    cmd.Parameters.AddWithValue("@ENDDATE", DBNull.Value);
                }
                cmd.ExecuteNonQuery();

                //retrieve the new ID just added:
                cmd.CommandText = null;
                cmd.CommandText = "select max (id) as id from content";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    newContentId = reader.GetInt32(0);
                    reader.Close();
                    cmd.CommandText = "insert into content_version(content_id,version_id,state_id,edit_user,updated_date,data) values (@NEWCONTENTID,1," + State.ACTIVE + ",@CREATEDUSER,GETDATE(),@CONTENT)";

                    //add entry to CONTENTINSTANCE:
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@CREATEDUSER", content.createdUser);
                    cmd.Parameters.AddWithValue("@NEWCONTENTID", newContentId);
                    cmd.Parameters.AddWithValue("@CONTENT", instanceText);
                    cmd.ExecuteNonQuery();
                    returnVal = newContentId;
                }

                if (returnVal > 0)
                {
                    objTrans.Commit();
                }
                else
                {
                    objTrans.Rollback();
                }
            }
            catch (Exception e)
            {
                conn.Close();
            }
            finally
            {
                conn.Close();
            }
            return returnVal;
        }

        public IList<ContentType> getContentTypes()
        {
            try
            {
                List<ContentType> cTypesList = new List<ContentType>();
                //create and open a connection:
                SqlConnection conn = new SqlConnection(session.dbConnStr);
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;

                //add the new entry into the CONTENT table:
                cmd.CommandText = "select id,name,description from content_types";

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ContentType _ctype = new ContentType();
                    _ctype.Id = reader.GetInt32(0);
                    _ctype.Name = reader.GetString(1);
                    _ctype.Description = reader.GetString(2);
                    cTypesList.Add(_ctype);
                }
                return cTypesList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //note - ID, if set, will be ignored.
        public bool addContentType(ContentType contentType)
        {
            return false;
        }


        public bool updateContentType(ContentType contentType)
        {
            return false;
        }


        private bool checkSqlDateRange(SqlDateTime dateToCheck)
        {
            if (dateToCheck <= SqlDateTime.MaxValue && dateToCheck >= SqlDateTime.MinValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
    }
}

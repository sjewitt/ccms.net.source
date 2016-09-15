using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ccms.utils;

namespace ccms.managers
{
    public class PageManager
    {
        private DBSession session;
        
        public PageManager(DBSession session)
        {
            this.session = session;
        }

        public Page getPage(Node node)
        {
            Page page = new Page(node.pageId);
            return page;
        }

        public bool addContentMapping(PageContentMap map)
        {
            SqlConnection conn = new SqlConnection(session.dbConnStr);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into page_content_ref (content_id,page_id,slot_num) values (@contentid,@pageid,@slotid);", conn);
                cmd.Parameters.AddWithValue("@pageid", map.pageId);
                cmd.Parameters.AddWithValue("@contentid", map.contentId);
                cmd.Parameters.AddWithValue("@slotid", map.slotNum);

                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Page content map not added");
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Page content map not added: " +ex);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public bool updateContentMapping(PageContentMap map)
        {
            SqlConnection conn = new SqlConnection(session.dbConnStr);
            try
            {
                conn.Open();
                //update page_content_ref set content_id=" + contentId + "where page_id=" + pageId + " and slot_num=" + slotId;
                SqlCommand cmd = new SqlCommand("update page_content_ref set content_id=@contentid where page_id=@pageid and slot_num=@slotid;", conn);
                cmd.Parameters.AddWithValue("@pageid", map.pageId);
                cmd.Parameters.AddWithValue("@contentid", map.contentId);
                cmd.Parameters.AddWithValue("@slotid", map.slotNum);

                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Page content map not updated");
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Page content map not updated: " + ex);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public bool removeContentMapping(PageContentMap map)
        {
            SqlConnection conn = new SqlConnection(session.dbConnStr);
            try
            {
                //delete from page_content_ref where page_id=" + pageId + " and slot_num=" + slotId + ";";

                conn.Open();
                SqlCommand cmd = new SqlCommand("delete from page_content_ref where page_id=@pageId and slot_num=@slotid;", conn);
                cmd.Parameters.AddWithValue("@pageid", map.pageId);
                cmd.Parameters.AddWithValue("@slotid", map.slotNum);

                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Page content map not updated");
                };
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        //return a List of associated Content Objects (or do I simply return the IDs???)
        public List<Content> getContent(Page page)
        {
            return null;
        }

        //utility function:
        public int getMaxPageId()
        {
            SqlConnection conn = new SqlConnection(session.dbConnStr);
            try
            {
                int _out = 0;
                conn.Open();
                SqlCommand cmd = new SqlCommand("select max(id) from page", conn);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0)) _out = reader.GetInt32(0);
                }
                return _out;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot retrieve max page ID: " + ex);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }




        //create
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newPage">Non-persistent Page object with properties Name, LinkText, Title, Description and Keywords populated.</param>
        /// <param name="createdUserId">ID of User that creates the new Page object</param>
        /// <returns>Persistent, fully populated Page object.</returns>
        public Page createNewPage(Page newPage, int createdUserId){
            //create and open a connection:
            SqlConnection conn = new SqlConnection(this.session.dbConnStr);
            conn.Open();
            SqlTransaction objTrans = conn.BeginTransaction();
            Page page = null;
            
            try
            {
                //begin transaction
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = objTrans;


                //add the new entry into the CONTENT table:
                cmd.CommandText = "insert into page (state,name,linktext,title,description,keywords,created_date,created_user) values(@PAGESTATE,@PAGENAME,@PAGELINKTEXT,@PAGETITLE,@PAGEDESCRIPTION,@PAGEKEYWORDS,@PAGECREATEDDATE,@PAGECREATEDUSER)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@PAGESTATE", State.ACTIVE);
                cmd.Parameters.AddWithValue("@PAGENAME", newPage.name);
                cmd.Parameters.AddWithValue("@PAGELINKTEXT", newPage.linkText);
                cmd.Parameters.AddWithValue("@PAGETITLE", newPage.title);
                cmd.Parameters.AddWithValue("@PAGEDESCRIPTION", newPage.description);
                cmd.Parameters.AddWithValue("@PAGEKEYWORDS", newPage.keywords);
                cmd.Parameters.AddWithValue("@PAGECREATEDDATE", DateTime.Now);
                cmd.Parameters.AddWithValue("@PAGECREATEDUSER", createdUserId);
                cmd.ExecuteNonQuery();


                //retrieve the new ID just added:
                bool OK = false;
                cmd.CommandText = null;
                cmd.CommandText = "select max (id) as id from page";
                SqlDataReader reader = cmd.ExecuteReader();
                int newId = 0;
                if (reader.Read())
                {
                    newId = reader.GetInt32(0);
                    reader.Close();
                    OK = true;
                }


                //if transaction successful, fill in the page ID and return the page:
                if (OK)
                {
                    objTrans.Commit();
                }
                else
                {
                    objTrans.Rollback();
                }
                page = new Page(newId);
            }
            catch (Exception e)
            {
                //rollback transaction
                conn.Close();
                return null;
            }
            finally
            {
                conn.Close();
            }
            return page;
        }

        //delete
    }
}

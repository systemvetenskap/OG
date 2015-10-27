using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web.Configuration;
using Npgsql;
using System.Configuration;

namespace Group3WebProject
{
    public partial class headsite : System.Web.UI.MasterPage
    {

        Classes.clsUsers user;
        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["JE"].ConnectionString;
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);

            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
           
            try
            {
                string sql = "SELECT * FROM users";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                
                //while
                //{
                //    user = new Classes.clsUsers();
                //}
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

            finally
            {
                conn.Close();
            }
        }

    }
}
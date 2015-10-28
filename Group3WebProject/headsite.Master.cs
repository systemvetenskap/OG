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
using Group3WebProject.Classes;

namespace Group3WebProject
{
    public partial class headsite : System.Web.UI.MasterPage
    {

        clsUsers user;
        List<clsUsers> userList = new List<clsUsers>();
        clsUsers sessionUser;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


                string connectionString = WebConfigurationManager.ConnectionStrings["JE"].ConnectionString;
                NpgsqlConnection conn = new NpgsqlConnection(connectionString);

                NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);


                try
                {
                    string sql = "SELECT * FROM users";
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                    conn.Open();
                    NpgsqlDataReader dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {

                        user = new Classes.clsUsers();
                        user.Id = int.Parse(dr["id"].ToString());
                        user.FirstName = dr["first_name"].ToString();
                        user.LastName = dr["last_name"].ToString();
                        user.TeamId = int.Parse(dr["team_id"].ToString());

                        userList.Add(user);

                    }

                    
                    ddl_users.DataSource = userList;
                    ddl_users.DataBind();

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

        protected void ddl_users_SelectedIndexChanged(object sender, EventArgs e)
        {
            sessionUser = new clsUsers();
            sessionUser = ddl_users.SelectedItem.Attributes;


                //[ddl_users.SelectedIndex];
            
        }

    }
}
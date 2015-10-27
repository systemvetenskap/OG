using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Npgsql;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Group3WebProject.Classes
{
    public class clsgetTests
    {
        NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);

        public DataTable getTest()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Name");
            dt.Columns.Add("Level");
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT  id, name, test_type as level FROM TEST", conn);
            conn.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                                
                dt.Rows.Add(dr["id"].ToString(), dr["name"].ToString(), "aa");
            }
            dr.Close();
            conn.Close();
            return dt;

        }
    }
}
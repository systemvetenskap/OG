using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using System.Data;
using System.Configuration;
namespace Group3WebProject.Classes
{
    public class clsLogin
    {

        public DataTable getUsers()
        {
            DataTable dt = new DataTable();
            //dt.Columns.Add("name");
            //dt.Columns.Add("id");
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
                conn.Open();
                NpgsqlDataAdapter adp = new NpgsqlDataAdapter("SELECT id, (first_name || ' '|| last_name) AS name FROM users", conn);
                adp.Fill(dt);
                conn.Close();
            }
            catch
            { }

            return dt;
        }
        public string getLevel(string userID)
        {
            string retLevel = "";

            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
                conn.Open();
               // string sql = "SELECT *  FROM users RIGHT JOIN team on users.id = team.user_id WHERE user.id='" + userID + "'";
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT *  FROM users RIGHT JOIN team on users.id = team.user_id WHERE users.id='" + userID + "'", conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    retLevel = "team";
                }
                else
                {
                    retLevel = "deltagare";
                }
                conn.Close();
            }
            catch
            { }
            return retLevel;
        }
    }
}
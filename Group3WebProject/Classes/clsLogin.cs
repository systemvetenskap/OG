using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using System.Data;
using System.Configuration;
using System.Diagnostics;

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
            { 
            
            }

            return dt;
        }
        public string getLevel(string userID)
        {
            string retLevel = "";
            string getUsers;

            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
                getUsers = "SELECT * FROM users INNER JOIN team ON users.id = team.user_id WHERE users.id = @uId";
               // string sql = "SELECT *  FROM users RIGHT JOIN team on users.id = team.user_id WHERE user.id='" + userID + "'";
                NpgsqlCommand cmd = new NpgsqlCommand(getUsers, conn);
                cmd.Parameters.AddWithValue("uId", Convert.ToInt32(userID));
                conn.Open();
                NpgsqlDataReader dr = cmd.ExecuteReader();


                if (dr.HasRows)
                {
                    retLevel = "provledare";
                }
                else
                {
                    retLevel = "deltagare";
                }
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return retLevel;
        }
    }
}
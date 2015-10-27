using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Diagnostics;
using Npgsql;
using System.Configuration;
namespace Group3WebProject.Classes
{
    public class clsRightOrNot
    {
        NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
        string dbConnectionString = "Initial Catalog='ITimingACK';Data Source='127.0.0.1';user id='sa';password='ssftiming'";
        public string saveAnswers(string quID, string quAns, string testID)
        {            
            try
            {
                string sql = "SELECT * FROM dbQuest  WHERE testID='" + testID + "' and QuestonID='" + quID + "'";
                SqlConnection cnn = new SqlConnection(dbConnectionString);
                cnn.Open();
                SqlCommand cmd = new SqlCommand(sql, cnn);

                SqlDataReader dr = cmd.ExecuteReader();
                bool exist = false;
                if (dr.Read())
                {
                    exist = true;
                }
                else
                {
                    exist = false;
                }
                dr.Close();
                if (exist == false)
                {
                    sql = "insert into dbQuest (testID, QuestonID, Answr) VALUES ('" + testID + "', '" + quID + "', '" + quAns + "')";
                }
                else
                {
                    sql = "update dbQuest SET Answr='" + quAns + "' WHERE testID='" + testID + "' and QuestonID='" + quID + "'";
                }
                cmd = new SqlCommand(sql, cnn);
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "";
        }
       
    }
}
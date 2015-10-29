using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using System.Configuration;
using System.Data;
namespace Group3WebProject.Classes
{
    public class clsStartingTest
    {
        /// <summary>
        /// Kontrollerar vilket test som ska göras retunerar ÅKU om inget har gjort tidigare. 
        /// Annars blir det ARBETS om man bara ska göra ett test 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string getOk(string userID)
        {
            DataTable dt = fixData(userID);
            if (dt == null && dt.Rows.Count > 0)
            {
                return "ÅKU";
            }

            DateTime startTime = DateTime.Parse(dt.Rows[0]["starttime"].ToString());
            DateTime timNow = DateTime.Now;
            TimeSpan diffDate = timNow - startTime;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((DateTime.Parse(dt.Rows[i]["starttime"].ToString()).Year == DateTime.Now.Year) && (dt.Rows[0]["passed"].ToString().ToUpper() == "t"))//Då har man redan gjort testet
                {
                    return "Du har redan gjort årets test och är godkänd";
                }
            }
            if (diffDate.Days < 7)
            {
                return "Du måste vänta minst 7dagar mellan proven";
            }
            return "ARBETS";
        }
        private DataTable fixData(string userID)
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
                conn.Open();
                NpgsqlDataAdapter adp = new NpgsqlDataAdapter("SELECT start_time AS starttime, passed FROM completed_test WHERE user_id='" + userID + "' ORDER BY start_time desc", conn);
                adp.Fill(dt);
                conn.Close();
            }
            catch
            { }
            return dt;
        }
        /// <summary>
        /// Vilken typ som ska vara emd och för att kopiera över datan till completed test 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userID"></param>
        public void startNew(string type, string userID)
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
                conn.Open();

                conn.Close();
            }
            catch 
            { }
        }
    }
}
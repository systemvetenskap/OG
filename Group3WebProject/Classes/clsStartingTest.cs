using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using System.Configuration;
using System.Data;
using System.Diagnostics;
namespace Group3WebProject.Classes
{
    public class clsStartingTest
    {
        /// <summary>
        /// Kontrollerar vilket test som ska göras retunerar Licens om inget har gjort tidigare. 
        /// Annars blir det ÅKU om man bara ska göra ett test 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string getOk(string userID)
        {
            DataTable dt = fixData(userID);
            if (dt == null || dt.Rows.Count < 1)
            {
                return "LICENS"; //FÖrsta gången 
            }
            DateTime startTime = DateTime.Parse(dt.Rows[0]["starttime"].ToString());
            DateTime timNow = DateTime.Now;
            TimeSpan diffDate = timNow - startTime;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            if ((DateTime.Parse(dt.Rows[0]["starttime"].ToString()).Year == DateTime.Now.Year) && (dt.Rows[0]["passed"].ToString().ToUpper() == "t"))//Då har man redan gjort testet
            {
                return "Du har redan gjort årets test och är godkänd";
            }
            // }
            Debug.WriteLine(DateTime.Parse(dt.Rows[0]["starttime"].ToString()));
            if (dt.Rows[0]["endtime"] == null || Convert.ToString(dt.Rows[0]["endtime"]) == "")
            {
                if (diffDate.TotalMinutes < 30)
                {
                    Debug.WriteLine(diffDate.TotalMinutes.ToString());
                    return "IGÅNG";
                }

            }
            if (diffDate.Days < 7)
            {
                return "Du måste vänta minst 7dagar mellan proven";
            }
            return "ÅKU"; //Årliga testet 
        }
        private DataTable fixData(string userID)
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
                conn.Open();
                NpgsqlDataAdapter adp = new NpgsqlDataAdapter("SELECT start_time AS starttime, passed, end_time AS endtime FROM completed_test WHERE user_id='" + userID + "' ORDER BY start_time desc", conn);
                adp.Fill(dt);
                conn.Close();
            }
            catch
            { }
            return dt;
        }
        /// <summary>
        /// Vilken typ som ska vara med och för att kopiera över datan till completed test 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userID"></param>
        public string startNew(string type, string userID)
        {
            DateTime startTime = DateTime.Now;
            string retAnsw = "";
            //string sql = ""
            string sql= "SELECT xml_questions As ans, id FROM test where test_type='" + type + "' and valid_through='" + startTime.Year.ToString() + "' order by id";
            Debug.WriteLine(sql);
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                
                string xml = "";
                string test = "";
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    xml = dr["ans"].ToString();
                    test = dr["id"].ToString();
                }
                else
                {
                    dr.Close();
                    conn.Close();
                    return "false";
                }
                dr.Close();
                bool aa = false;
                cmd = new NpgsqlCommand("INSERT INTO completed_test (user_id, test_id, xml_answer,start_time, passed) VALUES ('" + userID + "', '" + test + "', '" + xml + "', '" + startTime.ToString() + "', '" + aa + "') RETURNING id", conn);
                retAnsw = cmd.ExecuteScalar().ToString();
                conn.Close();
                clsRandomQue clRa = new clsRandomQue();
                clRa.randomizeTest(retAnsw);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return retAnsw;
        }
        public string getTestid(string userID) //Hämtar bara ut det senaste testID:et 
        {
            string test = "";
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT id FROM completed_test WHERE user_id='" + userID + "' ORDER BY start_time desc limit 1", conn);

                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    test = dr["id"].ToString();
                }
                else
                {
                    dr.Close();
                    conn.Close();
                    return "Finns inget test";
                }
                //Debug.WriteLine()
                dr.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return test;
        }
    }
}
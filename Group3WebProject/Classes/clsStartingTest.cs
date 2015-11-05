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
            DataTable dt = fixData(userID); //Hämtar datan vi behöver 
            bool pass = false;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (bool.Parse(dt.Rows[i]["passed"].ToString()) == true && dt.Rows[i]["type"].ToString().ToUpper() == "LICENS")
                {
                    pass = true;
                }
            }
            if ((dt == null || dt.Rows.Count < 1))
            {
                return "LICENS"; //FÖrsta gången  //Flytta ned den här en bit
            }
            //KOMMER INTE NER HIT VID LICENS 
            DateTime startTime = DateTime.Parse(dt.Rows[0]["starttime"].ToString());
            DateTime timNow = DateTime.Now;
            TimeSpan diffDate = timNow - startTime;
            if ((dt.Rows[0]["endtime"] == null || Convert.ToString(dt.Rows[0]["endtime"]) == "") && diffDate.TotalMinutes < 30)
            {
                return "IGÅNG";
            }
            else
            {
                if ((dt.Rows[0]["endtime"] == null || Convert.ToString(dt.Rows[0]["endtime"]) == ""))
                {
                    clsRightOrNot clRight = new clsRightOrNot();
                    clRight.setFail(dt.Rows[0]["comtestid"].ToString());
                }                //Sätt sluttid
            }
            if (bool.Parse(dt.Rows[0]["passed"].ToString()) == true) //Då har man klarat det sista 
            {
                if (int.Parse(dt.Rows[0]["valid"].ToString()) < DateTime.Now.Year)
                {
                    //Då ska man göra årets test
                    //Hoppar ner till 7dagar
                    return "ÅKU";
                }
                else if (int.Parse(dt.Rows[0]["valid"].ToString()) > DateTime.Now.Year)
                {
                            //Du har redan gjort det senaste provet'
                    return "Du har redan gjort det senaste provet";
                }
                else if (int.Parse(dt.Rows[0]["valid"].ToString()) == DateTime.Now.Year)
                {
                    //Då ska man göra åku för nästa år

                    string next = doNextYear(int.Parse(dt.Rows[0]["valid"].ToString()), "ÅKU");
                    
                        if (next == "false")
                        {
                            return "Du är godkänd på årets test men det har inte kommit något inför nästa år";
                        }
                        else
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["valid"].ToString() == DateTime.Now.AddYears(1).Year.ToString())
                                {
                                    return "Du är godkänd på kommande års test";
                                }
                            }
                            if (diffDate.Days <= 7 && bool.Parse(dt.Rows[0]["passed"].ToString()) == false)
                            {
                                return "Du måste vänta minst 7dagar mellan proven";
                            }
                            else
                            { 
                            return "Du kan göra nästa års test nu";
                            }
                        }                    
                }               
            }
            else
            {
                if (int.Parse(dt.Rows[0]["valid"].ToString()) < DateTime.Now.Year)
                {
                    //Då ska man göra årets test
                    //Hoppar ner till 7dagar

                    if (dt.Rows[0]["type"].ToString() == "ÅKU")
                    {
                        return "ÅKU";
                    }
                    else
                    {
                        //return "LICENS";
                    }
                }
                else if (int.Parse(dt.Rows[0]["valid"].ToString()) > DateTime.Now.Year)
                {
                    //Du har redan gjort det senaste provet'
                   // return "Du har redan gjort det senaste provet";
                    if (diffDate.Days <= 7 )
                    {
                        return "Du måste vänta minst 7dagar mellan proven";
                    }
                    else
                    {
                        return "Du kan göra nästa års test nu";
                    }
                }
              
                //Då måste vi kolla 
                //Du har haft fel på senaste provet
            }


            

            if (diffDate.Days <= 7 && bool.Parse(dt.Rows[0]["passed"].ToString()) == false)
            {
                return "Du måste vänta minst 7dagar mellan proven";
            }
            if (!pass)
            {
                return "LICENS";
            }

            return "ÅKU"; //Årliga testet 
        }
        private DataTable fixData(string userID)//Hämtar alla genomförda testen för användaren completed_test 
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
                conn.Open();
                NpgsqlDataAdapter adp = new NpgsqlDataAdapter("SELECT start_time AS starttime, passed, end_time AS endtime, test_type AS type, valid_through AS valid, test.id AS testid, completed_test.id AS comtestid FROM completed_test left join test on test.id = completed_test.test_id  WHERE user_id='" + userID + "' ORDER BY start_time desc", conn);
                //adp.Parameters.AddWithValue("uid", userID);
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
        public string startNew(string type, string userID, int year)
        {
            DateTime startTime = DateTime.Now;
            string retAnsw = "";
            //string sql = ""
            string sql = "SELECT xml_questions As ans, id FROM test where test_type= @type and valid_through= @year order by id";
         
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("type", type);
                cmd.Parameters.AddWithValue("year", year);
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
                cmd = new NpgsqlCommand("INSERT INTO completed_test (user_id, test_id, xml_answer,start_time, passed) VALUES (@uid, @test , @xml ,  @startTime , @aa ) RETURNING id", conn);
                cmd.Parameters.AddWithValue("uid", int.Parse(userID));
                cmd.Parameters.AddWithValue("test", int.Parse(test));
                cmd.Parameters.AddWithValue("xml", xml);
                cmd.Parameters.AddWithValue("startTime", startTime);
                cmd.Parameters.AddWithValue("aa", aa);


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
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT id FROM completed_test WHERE user_id= @uid ORDER BY start_time desc limit 1", conn);
                cmd.Parameters.AddWithValue("uid", int.Parse(userID));
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
        private string doNextYear(int lastPassedTest, string nextTestType) //Kontrollerar om det finns ett test inför nästa år 
        {
            string test = "";
            try
            {
                int nexYear = lastPassedTest + 1;
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT id FROM test WHERE valid_through = @year and test_type= @typ ORDER BY valid_through desc limit 1", conn);
                cmd.Parameters.AddWithValue("year", lastPassedTest);
                cmd.Parameters.AddWithValue("typ", nextTestType);
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    test = dr["id"].ToString();
                }
                else
                {
                    conn.Close();
                    test = "false";
                }
                dr.Close();
                return test;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return "false";
        }
    }
}
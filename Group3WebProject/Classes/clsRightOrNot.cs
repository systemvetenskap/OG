using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Diagnostics;
using Npgsql;
using System.Configuration;
using System.Xml;
using System.IO;
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
        /// <summary>
        /// Sparar det man har svarat i xml filen 
        /// </summary>
        /// <param name="testid"></param>
        /// <param name="qid"></param>
        /// <param name="answ"></param>
        public void valudateXML(string testId, string qid, List<string> answ)
        {

            XmlDocument doc = new XmlDocument();
            try
            {
                XmlTextReader xmlReader = new XmlTextReader(new System.IO.StringReader(getXml(testId)));
                doc.Load(xmlReader);
                XmlNodeList nodes = doc.SelectNodes("test/question");
                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes["value"].Value == qid)
                    {
                        foreach (XmlNode childNode in node.ChildNodes)
                        {
                            if (childNode.Name == "answer")
                            {
                                var match = answ.FirstOrDefault(stringToCheck => stringToCheck.Contains(childNode.Attributes["id"].Value));
                                //if (childNode.Attributes["id"].Value == answ) //Sätter det elementet til ltrue som stämmer överens 
                                if (match != null) //Sätter det elementet til ltrue som stämmer överens 
                                {
                                    childNode.Attributes["selected"].Value = "true";
                                    // Debug.WriteLine("Right ");
                                }
                                else
                                {
                                    childNode.Attributes["selected"].Value = "false"; //Sötter alltid elementet till false för att kunna köra igenom listen
                                    //  Debug.WriteLine("Fakse");
                                }
                            }
                            //Debug.WriteLine(childNode.Name);
                        }
                    }
                }
                xmlReader.Close();
                //==ADMIN==
                //   doc.Save(@"C:\inlk.xml"); //Användes för debug för att se filen lokalt MÅSTE VARA ADMIN!!!!!!!!!!!!!
                string xmlResult = "";


                StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                xw.Formatting = Formatting.Indented; //Fixar formateringen på xml:en
                doc.WriteTo(xw);
                xmlResult = sw.ToString();
                if (xmlResult != "")
                {
                    updateXML(testId, xmlResult);
                }

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.ToString());
            }
            //doc.Save(yourFilePath);

        }
        public void updateXML(string testID, string strXML)
        {
            NpgsqlConnection conn = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
            conn.Open();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE completed_test SET xml_answer= @strXML  WHERE id= @testID", conn);
                cmd.Parameters.AddWithValue("strXML", strXML);
                cmd.Parameters.AddWithValue("testID", int.Parse(testID));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            conn.Close();
        }
        public string getXml(string testID)
        {
            string result = "";
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT  id, xml_answer as qXml FROM completed_test  where id= @testID", conn);
                cmd.Parameters.AddWithValue("testID", int.Parse(testID));
                //cmd.Parameters.Add("testID", testID);
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    result = dr["qXml"].ToString();
                }
                dr.Close();
                conn.Close();
                return result.TrimStart();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return "";
           
        }
        /// <summary>
        /// Sparar resultat på provet 
        /// </summary>
        /// <param name="testID"></param>
        /// <param name="answ"></param>
        public void updateResult(string testID, bool answ)
        {

            NpgsqlConnection conn = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
            conn.Open();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE completed_test SET passed= @answ , end_time=@dateNo WHERE id= @testID ", conn);
                cmd.Parameters.AddWithValue("testID", int.Parse(testID));
                cmd.Parameters.AddWithValue("dateNo", DateTime.Now);
                cmd.Parameters.AddWithValue("answ", answ);


                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            conn.Close();
        }
        public string canHandIn(string testID)
        {

            string result = "";
            NpgsqlConnection conn = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT  id, start_time, end_time FROM completed_test  where id= @testID ", conn);
            cmd.Parameters.AddWithValue("testID", int.Parse(testID));
            //cmd.Parameters.Add("testID", testID);
            NpgsqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = dr["start_time"].ToString();
                if (dr["end_time"] != null)
                {
                    
                }
            }
            else
            {
                dr.Close();
                conn.Close();
                return "FINNS INGET TEST";
            }
            dr.Close();
            conn.Close();
            TimeSpan diffTime = DateTime.Parse(DateTime.Now.ToString()) - DateTime.Parse(result);
            if (diffTime.TotalMinutes > 29)
            {
                return "TIDEN DROG ÖVER";
            }
            return "OK";
        }
        public void setFail(string testID)
        {
            string re = canHandIn(testID);
            if (re != "TIDEN DROG ÖVER")
            {
                return;
            }
            NpgsqlConnection conn = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
            conn.Open();
            //SELECT * FROM completed_test left join test on test.id = completed_test.test_id   ORDER BY start_time desc;
            string sql = "SELECT xml_questions As ans, test.id FROM test left join completed_test on test.id = completed_test.test_id WHERE completed_test.id= @testID  order by id";
            try
            {
                string xml = "";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("testID", int.Parse(testID));
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    xml = dr["ans"].ToString();
                }
                else
                {
                    return;
                }
                dr.Close();
                cmd = new NpgsqlCommand("UPDATE completed_test SET passed= @pas, end_time= @end, xml_answer= @xml WHERE id= @testID", conn);
                cmd.Parameters.AddWithValue("pas", false);
                cmd.Parameters.AddWithValue("end", DateTime.Now);
                cmd.Parameters.AddWithValue("xml", xml);
                cmd.Parameters.AddWithValue("testID", int.Parse(testID));                
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            conn.Close();
        }
    }
}
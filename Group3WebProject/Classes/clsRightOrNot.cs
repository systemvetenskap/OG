﻿using System;
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
                        Debug.WriteLine(node.Attributes["value"].Value + " värdet   och sedan value " + answ);
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
                    Debug.WriteLine("Saved xml");
                }

                Debug.WriteLine(doc.ToString());
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
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE completed_test SET xml_answer='" + strXML + "' WHERE id='" + testID + "'", conn);
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
            NpgsqlConnection conn = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT  id, xml_answer as qXml FROM completed_test  where id='" + testID + "'", conn);
            //cmd.Parameters.Add("testID", testID);
            NpgsqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = dr["qXml"].ToString();
            }
            dr.Close();
            conn.Close();
            Debug.WriteLine("MEOTDE");
            return result.TrimStart();
        }
        public void updateResult(string testID, bool answ)
        {

            NpgsqlConnection conn = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
            conn.Open();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE completed_test SET passed='" + answ + "', end_time='" + DateTime.Now.ToString() + "' WHERE id='" + testID + "'", conn);
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
            string ret = "";

            NpgsqlConnection conn = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT  id, start_time, end_time FROM completed_test  where id='" + testID + "'", conn);
            //cmd.Parameters.Add("testID", testID);
            NpgsqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = dr["start_time"].ToString();
                if (dr["end_time"] != null)
                {
                    ret = "INLÄMNAT";
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
            Debug.WriteLine("MEOTDE");
            TimeSpan diffTime = DateTime.Parse(DateTime.Now.ToString()) - DateTime.Parse(result);
            if (diffTime.TotalMinutes > 29)
            {
                return "TIDEN DROG ÖVER";
            }
            return "OK";
        }
    }
}
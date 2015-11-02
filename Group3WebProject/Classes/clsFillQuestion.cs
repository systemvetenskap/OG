using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml;
using System.Diagnostics;
using System.IO;
using Npgsql;
namespace Group3WebProject.Classes
{
    public class clsFillQuestion
    {
        /// <summary>
        /// Läser in datan till en datatable för att ta med frågorna som finns för ´just det testidet
        /// Qid är vilken fråga som man efter¨frågar sedan så är det testID som är vilkter tessom man har valt
        /// </summary>
        /// <param name="qID"></param>
        /// <param name="testID"></param>
        /// <returns></returns>
        public Tuple<DataTable, string, int, string, string> readXML(string qID, string testID)
        {
            DataTable dt = new DataTable();
            string quest = "";
            string part = "";
            int countRi = 0;
            string img = "";
            dt.Columns.Add("name");
            dt.Columns.Add("id");
            dt.Columns.Add("sel");
            dt.Columns.Add("answ");
            try
            {
                XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(getXml(testID)));
                while (reader.Read())
                {
                    switch (reader.Name)
                    {
                        case "question":

                            if (reader.AttributeCount > 0 && qID.ToUpper() == reader.GetAttribute("value").ToUpper())//OM DETR FINN 
                            {
                                part = reader.GetAttribute("part").ToUpper();
                            }
                            else
                            {
                                reader.Skip();
                            }
                            break;
                        case "txt":
                            quest = reader.ReadString(); //Frågan sparas till en string behöver ha en tupple
                            img = reader.GetAttribute("img");
                            break;
                        case "answer":
                            //answ
                            string rightOrFalse = "";
                            dt.Rows.Add();//Nedan är svarsalternativen 
                            dt.Rows[dt.Rows.Count - 1]["id"] = reader.GetAttribute("id").ToUpper();
                            rightOrFalse = reader.GetAttribute("answ").ToUpper();
                            dt.Rows[dt.Rows.Count - 1]["answ"] = rightOrFalse;
                            dt.Rows[dt.Rows.Count - 1]["sel"] = reader.GetAttribute("selected").ToUpper();
                            dt.Rows[dt.Rows.Count - 1]["name"] = reader.ReadString();
                            if (rightOrFalse == "TRUE")
                            {
                                countRi += 1;
                            }
                            break;

                    }
                }
                reader.Close();
                return new Tuple<DataTable, string, int, string, string>(dt, quest, countRi, part, img);
                //Debug.WriteLine("Detta gick bra   " + dt.Rows.Count.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
            // return "aakk";
        }
        public string getXml(string testID)
        {
            string result = "";
            NpgsqlConnection conn = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT  id, xml_answer as qXml FROM completed_test  where id='" + testID + "'", conn);
            NpgsqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = dr["qXml"].ToString();
            }
            dr.Close();
            conn.Close();

            return result.Trim();
        }
    }
}
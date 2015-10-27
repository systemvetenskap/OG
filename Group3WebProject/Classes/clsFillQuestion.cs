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
        public DataTable readXML(string qID, string testID)
        {
            DataTable dt = new DataTable();
            string quest = "";
            string part = "";


            dt.Columns.Add("name");
            dt.Columns.Add("id");
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
                            break;
                        case "answer":
                            dt.Rows.Add();//Nedan är svarsalternativen 
                            dt.Rows[dt.Rows.Count - 1]["id"] = reader.GetAttribute("id").ToUpper();
                            dt.Rows[dt.Rows.Count - 1]["name"] = reader.ReadString();
                            break;
                       
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
            return dt;
        }
        private string getXml(string testID)
        {
            string result = "";
            NpgsqlConnection conn = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT  test.id, test.name, test_type,xml_questions as qXml FROM TEST RIGHT JOIN completed_test on TEST.id = completed_test.test_id where completed_test.id='" + testID + "'", conn);
            NpgsqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())                
            {
                result = dr["qXml"].ToString();
            }
            
            return result;
        }
    }
}
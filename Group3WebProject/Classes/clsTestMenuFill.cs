using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Diagnostics;
using System.Xml;
using Npgsql;
using System.IO;

namespace Group3WebProject.Classes
{
    public class clsTestMenuFill
    {
        public DataTable read(string testID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("name"); //Fråga 1, Fråga 2---
            dt.Columns.Add("id"); //Fråge id:et 
            dt.Columns.Add("order");
            try
            {
                XmlTextReader reader = new XmlTextReader(new StringReader(getXml(testID)));
                while (reader.Read())
                {
                    switch (reader.Name)
                    {
                        case "question":
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["order"] = reader.GetAttribute("order").ToUpper();
                            dt.Rows[dt.Rows.Count - 1]["id"] = reader.GetAttribute("value").ToUpper();
                           // Random random = new Random();
                           
                            reader.Skip();
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            
            dt.DefaultView.Sort = "order";
            dt = dt.DefaultView.ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["name"] = (i + 1).ToString();
            }

            return dt;
        }
        private string getXml(string testID)
        {
            string result = "";
            NpgsqlConnection conn = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT  id, xml_answer as qXml FROM completed_test  where id= @testID ", conn);
            cmd.Parameters.AddWithValue("testID", int.Parse(testID));
            NpgsqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = dr["qXml"].ToString();
            }
            dr.Close();
            conn.Close();
           //Debug.WriteLine(result + " asd" );
            return result.TrimStart(); //.Trim();
        }
    }
}
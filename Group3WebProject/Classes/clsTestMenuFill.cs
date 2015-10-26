using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Diagnostics;
using System.Xml;
namespace Group3WebProject.Classes
{
    public class clsTestMenuFill
    {
        public DataTable read(string xmlPath)
        {
            Debug.WriteLine("Nu har vi kommit in ");
            DataTable dt = new DataTable();
            dt.Columns.Add("name"); //Fråga 1, Fråga 2---
            dt.Columns.Add("id"); //Fråge id:et 
            try
            {
                XmlTextReader reader = new XmlTextReader(xmlPath);
                while (reader.Read())
                {
                    switch (reader.Name)
                    {
                        case "question":
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["id"] = reader.GetAttribute("value").ToUpper();
                            Random random = new Random();
                            dt.Rows[dt.Rows.Count - 1]["name"] = random.Next(0, 100).ToString();
                            reader.Skip();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["name"] = "Fråga " + (i + 1).ToString();
            }

            return dt;
        }
    }
}
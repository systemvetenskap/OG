using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml;
using System.Diagnostics;
namespace Group3WebProject.Classes
{
    public class clsFillQuestion
    {
        public DataTable readXML(string qID, string xmlPath)
        {
            DataTable dt = new DataTable();
            string quest = "";
            string part = "";
            dt.Columns.Add("name");
            dt.Columns.Add("id");
            try
            {
                XmlTextReader reader = new XmlTextReader(xmlPath);
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
                        case "qone": 
                            dt.Rows.Add();//Nedan är svarsalternativen 
                            dt.Rows[dt.Rows.Count - 1]["id"] = reader.GetAttribute("id").ToUpper();
                            dt.Rows[dt.Rows.Count - 1]["name"] = reader.ReadString();
                            break;
                        case "qtwo":
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["id"] = reader.GetAttribute("id").ToUpper();
                            dt.Rows[dt.Rows.Count - 1]["name"] = reader.ReadString();                           
                            break;
                        case "qthree":
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["id"] = reader.GetAttribute("id").ToUpper();
                            dt.Rows[dt.Rows.Count - 1]["name"] = reader.ReadString();                           
                            break;
                        case "qfour":
                            dt.Rows.Add();
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
    }
}
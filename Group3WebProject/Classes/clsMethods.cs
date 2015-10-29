using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Data;


namespace Group3WebProject.Classes
{
    public class clsMethods
    {

        /// <summary>
        /// Metoden tar emot en xml som string, räknar ut antalet rätt och returnerar en string i formatet "[antal rätt]/[antal frågor]"
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public string getResultFromXml(string xml)
        {
            string elementName;

            XmlTextReader reader = new XmlTextReader(new StringReader(xml));

            int totalQuestions = 0; //Totala antalet frågor.
            int totalTrueAnsw = 0; //Totala antalet svarsalternat som är rätt(kan vara flera per fråga).
            int rightAnswers = 0; //Provdeltagarens antal förbockade rätta svarsalternativ.

            while (reader.Read())
            {

                switch (reader.NodeType)
                {
                        
                    
                    case XmlNodeType.Element:
                        int rättaalternativ = 0;
                        int rättasvar = 0;
                        elementName = reader.Name;
                        if (elementName == "question")
                        {
                            totalQuestions++;
                        }
    
                        if (elementName == "answer" && reader.AttributeCount > 0)
                        {
                            if (reader.GetAttribute("answ") == "true")
                            {
                                rättaalternativ++;
                            }

                            if (reader.GetAttribute("answ") == "true" && reader.GetAttribute("selected") == "true")
                            {

                                rättasvar++;
                                            
                                if(rättaalternativ == rättasvar)
                                {
                                    rightAnswers++;
                                
                                }
                                            
                            }

                        }

                        break;
                }


            

        }
            

            //Poäng endast för komplett besvarade frågor, alltså med rätt antal svarsalternativ förbockade.

            string result = rightAnswers.ToString() + "/" + totalQuestions.ToString();

            return result;

        }


        //public DataTable readXML(string qID, string testID)
        //{
        //    DataTable dt = new DataTable();
        //    string quest = "";
        //    string part = "";


        //    dt.Columns.Add("name");
        //    dt.Columns.Add("id");
        //    dt.Columns.Add("sel");
        //    dt.Columns.Add("answ");
        //    try
        //    {
        //        XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(getXml(testID)));
        //        while (reader.Read())
        //        {
        //            switch (reader.Name)
        //            {
        //                case "question":

        //                    if (reader.AttributeCount > 0 && qID.ToUpper() == reader.GetAttribute("value").ToUpper())//OM DETR FINN 
        //                    {
        //                        part = reader.GetAttribute("part").ToUpper();
        //                    }
        //                    else
        //                    {
        //                        reader.Skip();
        //                    }
        //                    break;
        //                case "txt":
        //                    quest = reader.ReadString(); //Frågan sparas till en string behöver ha en tupple
        //                    break;
        //                case "answer":
        //                    //answ
        //                    dt.Rows.Add();//Nedan är svarsalternativen 
        //                    dt.Rows[dt.Rows.Count - 1]["id"] = reader.GetAttribute("id").ToUpper();
        //                    dt.Rows[dt.Rows.Count - 1]["answ"] = reader.GetAttribute("answ").ToUpper();
        //                    dt.Rows[dt.Rows.Count - 1]["sel"] = reader.GetAttribute("selected").ToUpper();
        //                    dt.Rows[dt.Rows.Count - 1]["name"] = reader.ReadString();
        //                    break;

        //            }
        //        }
        //        reader.Close();
        //        return dt;
        //        //Debug.WriteLine("Detta gick bra   " + dt.Rows.Count.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.ToString());
        //        return null;
        //    }
        //    // return "aakk";
        //}





    }
}
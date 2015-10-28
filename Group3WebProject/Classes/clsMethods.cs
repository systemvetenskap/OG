using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Configuration;
using System.IO;


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
            XmlTextReader reader = new XmlTextReader(new StringReader(xml));

            int totalQuestions = 0; //Totala antalet frågor.
            int totalTrueAnsw = 0; //Totala antalet svarsalternat som är rätt(kan vara flera per fråga).
            int rightAnswers = 0; //Provdeltagarens antal förbockade rätta svarsalternativ.

            while (reader.Read())
            {

                switch (reader.Name)
                {
                    case "question":
                        if (reader.AttributeCount > 0)
                        {
                            totalQuestions++;
                        }
                        break;

                    case "answer":
                        if (reader.AttributeCount > 0 && reader.GetAttribute("answ") == "true")
                        {
                            totalTrueAnsw++;
                        }
                        if (reader.AttributeCount > 0 && reader.GetAttribute("answ") == "true" && reader.GetAttribute("selected") == "true")
                        {
                            rightAnswers++;
                        }
                        break;

                }
            }

            int points = rightAnswers - (totalTrueAnsw - totalQuestions); //Poäng endast för komplett besvarade frågor, alltså med rätt antal svarsalternativ förbockade.

            string result = points.ToString() + "/" + totalQuestions.ToString();

            return result;

        }





    }
}
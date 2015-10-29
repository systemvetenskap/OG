﻿using System;
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
            bool correct = true;

            XmlTextReader reader = new XmlTextReader(new StringReader(xml));
            reader.WhitespaceHandling = WhitespaceHandling.None;
            int totalQuestions = 0; //Totala antalet frågor.
            int rightAnswers = 0; //Provdeltagarens antal rätta svar.

            while (reader.Read())
            {

                switch (reader.NodeType)
                {
                        
                    
                    case XmlNodeType.Element:
                        
                        elementName = reader.Name;
                        if (elementName == "question")
                        {
                            totalQuestions++;
                        }
    
                        if (elementName == "answer" && reader.AttributeCount > 0)
                        {
                            if (reader.GetAttribute("answ") != reader.GetAttribute("selected"))
                            {
                                correct = false;

                            }

                        }

                        break;


                    case XmlNodeType.EndElement:
                        string endElement = reader.Name;

                        if (endElement == "question" && correct == true)
                        {
                            rightAnswers++;
                            
                        }
                        else if (endElement == "question")
                        {
                            correct = true;
                        }

                        break;
                }


            

        }
            

            //Poäng endast för komplett besvarade frågor, alltså med rätt antal svarsalternativ förbockade.

            string result = rightAnswers.ToString() + "/" + totalQuestions.ToString();

            return result;

        }

        public List<clsQuestion> XmlToClasses(string xml)
        {
            clsAnswer a = new clsAnswer();
            clsQuestion q = new clsQuestion();
            //List<clsAnswer> answerList = new List<clsAnswer>();
            List<clsQuestion> questionList = new List<clsQuestion>();

            string elementName;
            
            string endElementName;
            

            XmlTextReader reader = new XmlTextReader(new StringReader(xml));
            reader.WhitespaceHandling = WhitespaceHandling.None;
          

            while (reader.Read())
            {

                switch (reader.NodeType)
                {


                    case XmlNodeType.Element:
                       
                        elementName = reader.Name;
                        if (elementName == "question")
                        {
                            q.value = int.Parse(reader.GetAttribute("value"));
                            q.part = reader.GetAttribute("part");
                            q.answers = new List<clsAnswer>();
                        }

                        if (elementName == "answer")
                        {
                            a.id = int.Parse(reader.GetAttribute("id"));
                            a.answ = Convert.ToBoolean(reader.GetAttribute("answ"));
                            a.selected = Convert.ToBoolean(reader.GetAttribute("selected"));
                            a.text = reader.Value;

                        }
                        if (elementName == "txt")
                        {
                            q.txt = reader.Value;
                        }
                        break;



                    case XmlNodeType.EndElement:
                        endElementName = reader.Name;
                        if(endElementName == "answer")
                        {
                            //answerList.Add(a);
                            q.answers.Add(a);
                            a = new clsAnswer();
                        }
                        if(endElementName == "question")
                        {
                            //q.answers = answerList;
                            questionList.Add(q);
                            q = new clsQuestion();
                            
                        }
                        break;
                }
            }
            return questionList;
        }
    }
}
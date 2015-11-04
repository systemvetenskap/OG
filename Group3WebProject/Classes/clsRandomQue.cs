using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Diagnostics;
using System.Xml;
using System.IO;
namespace Group3WebProject.Classes
{
    public class clsRandomQue
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }
        public void randomizeTest(string testID)
        {
            clsTestMenuFill clMen = new clsTestMenuFill();
            DataTable dt = clMen.read(testID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Random rand = new Random((int)DateTime.Now.Ticks);
                int RandomNumber;
                RandomNumber = rand.Next(1, 1200) * 1000;
                dt.Rows[i]["name"] = RandomNumber.ToString();
            }
            dt.DefaultView.Sort = "name";
            dt = dt.DefaultView.ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["name"] = (i + 1);
            }
            Classes.clsFillQuestion clQue = new Classes.clsFillQuestion();
            string xml = clQue.getXml(testID);
            setValXML(testID, xml);
        }
        private void setValXML(string testID, string xmlFil)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                XmlTextReader xmlReader = new XmlTextReader(new System.IO.StringReader(xmlFil));
                doc.Load(xmlReader);
                XmlNodeList nodes = doc.SelectNodes("test/question");
                foreach (XmlNode node in nodes)
                {
                    Random rand = new Random((int)DateTime.Now.Ticks);
                    // int RandomNumber;
                    //RandomNumber = rand.Next(1, 1200);
                    node.Attributes["order"].Value = RandomNumber(1, 90).ToString();
                    // Debug.WriteLine(node.Attributes["value"].Value + " värdet   och sedan value " + answ);
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "answer")
                        {
                            Random rand2 = new Random((int)DateTime.Now.Ticks);
                            childNode.Attributes["sort"].Value = RandomNumber(1, 65).ToString();
                        }
                    }

                }
                xmlReader.Close();
                //==ADMIN==
                // doc.Save(@"C:\Rinlk.xml"); //Användes för debug för att se filen lokalt MÅSTE VARA ADMIN!!!!!!!!!!!!!
                string xmlResult = "";
                StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                xw.Formatting = Formatting.Indented; //Fixar formateringen på xml:en
                doc.WriteTo(xw);
                xmlResult = sw.ToString();
                Classes.clsRightOrNot clsRi = new Classes.clsRightOrNot();
                if (xmlResult != "")
                {
                    clsRi.updateXML(testID, xmlResult);
                }

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.ToString());
            }
            //doc.Save(yourFilePath);
        }

    }
}
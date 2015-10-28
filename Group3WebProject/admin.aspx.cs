using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Npgsql;
using System.Configuration;
using Group3WebProject.Classes;
using System.Xml;
using System.IO;

namespace Group3WebProject
{
    public partial class admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable[] dt = GetTeamList(1);
            previousTests.DataSource = dt[0];
            previousTests.DataBind();

        }

        private DataTable[] GetTeamList(int leaderId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Namn", typeof(string));
            dt.Columns.Add("Provtyp", typeof(string));
            dt.Columns.Add("Resultat", typeof(string));
            dt.Columns.Add("Godkänd", typeof(bool));
            dt.Columns.Add("Giltig till", typeof(string));

            //dt2 används inte just nu men ska senare visa provdeltagare och vilket nästa prov de måste göra är.
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Namn", typeof(string));
            dt2.Columns.Add("Provtyp", typeof(string));

            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
            string sql = "SELECT u.first_name, u.last_name, ct.passed, ct.xml_answer, t.test_type, t.valid_through FROM completed_test ct INNER JOIN users u ON u.id = ct.user_id INNER JOIN test t ON t.id = ct.test_id";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            conn.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();


            while (dr.Read())
            {
                string name = dr["first_name"].ToString() + " " + dr["last_name"].ToString();
                string testType = dr["test_type"].ToString();
                bool passed = Convert.ToBoolean(dr["passed"]);
                string validThrough = dr["valid_through"].ToString();
                string answerXml = (dr["xml_answer"].ToString()).Trim();



                //Read och switch case nedan läser antal frågor och rätta svar i xml_answer och mellanlagrar detta i totalQuestions och rightAnswers.
                XmlTextReader reader = new XmlTextReader(new StringReader(answerXml));

                int totalQuestions = 0;
                int totalTrueAnsw = 0;
                int rightAnswers = 0;

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

                int points = rightAnswers -(totalTrueAnsw - totalQuestions);

                string result = points.ToString() + "/" + totalQuestions.ToString();

                //nedan läggs provdeltagarens statistik till i en egen rad i DataTable.
                dt.Rows.Add(name, testType, result, passed, validThrough);
            }
            conn.Close();



            
            DataTable[] dtA = new DataTable[2];
            dtA[0] = dt;
            dtA[1] = dt2;
            
            return dtA;
        }
    }
}
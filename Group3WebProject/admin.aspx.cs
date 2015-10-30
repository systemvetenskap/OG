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
using System.Web.SessionState;
using System.Diagnostics;
using System.Text;

namespace Group3WebProject
{
    public partial class admin : System.Web.UI.Page
    {
        clsMethods method = new clsMethods();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                HttpSessionState ss = HttpContext.Current.Session;
                if (HttpContext.Current.Session["userid"] != null)
                {
                    Debug.WriteLine(HttpContext.Current.Session["userid"].ToString() + " aa  ");
                    //Check if user have right credit 
                    //IF level == Provdeltahare
                    Classes.clsLogin clsLog = new Classes.clsLogin();
                    if (clsLog.getLevel(HttpContext.Current.Session["userid"].ToString()) == "provledare") //Inloggad
                    {
                        Debug.WriteLine(" DU KOM IN ");
                    }
                    else //Är inloggad med fel credinatl
                    {
                        Response.Redirect("login.aspx");
                    }
                }
                else //Har inte loggat in 
                {
                    Response.Redirect("login.aspx");
                }
            }

            DataTable[] dt = GetTeamList(int.Parse(HttpContext.Current.Session["userid"].ToString()));
            previousTests.DataSource = dt[0];
            previousTests.DataBind();

            upcomingTests.DataSource = testStats(int.Parse(HttpContext.Current.Session["userid"].ToString()), 3);
            upcomingTests.DataBind();


        }

        private DataTable[] GetTeamList(int leaderId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Namn", typeof(string));
            dt.Columns.Add("Provtyp", typeof(string));
            dt.Columns.Add("Resultat", typeof(string));
            dt.Columns.Add("Godkänd", typeof(bool));
            dt.Columns.Add("Giltigt t.o.m.", typeof(string));

            //dt2 används inte just nu men ska senare visa provdeltagare och vilket nästa prov de måste göra är.
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Namn", typeof(string));
            dt2.Columns.Add("Provtyp", typeof(string));

            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);

            string sql = "SELECT first_name, last_name, passed, xml_answer, test_type, valid_through FROM "
                            + "(SELECT DISTINCT ON (ct.user_id) u.first_name, u.last_name, u.team_id, ct.passed, ct.xml_answer, t.test_type, t.valid_through "
                            + "FROM completed_test ct "
                            + "INNER JOIN users u "
                            + "ON u.id = ct.user_id "
                            + "INNER JOIN test t "
                            + "ON t.id = ct.test_id "
                            + "ORDER BY ct.user_id, ct.start_time DESC)b "
                            + "WHERE team_id = (SELECT id FROM team WHERE user_id = @uId)";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("uId", leaderId);
            conn.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string name = dr["first_name"].ToString() + " " + dr["last_name"].ToString();
                string testType = dr["test_type"].ToString();
                bool passed = Convert.ToBoolean(dr["passed"]);
                string validThrough = dr["valid_through"].ToString();
                string answerXml = (dr["xml_answer"].ToString()).Trim();


                //nedan läggs provdeltagarens statistik till i en egen rad i DataTable.
                dt.Rows.Add(name, testType, method.getResultFromXml(answerXml), passed, validThrough);
            }
            conn.Close();



            
            DataTable[] dtA = new DataTable[2];
            dtA[0] = dt;
            dtA[1] = dt2;
            
            return dtA;
        }

        private DataTable testStats(int leaderId, int testId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Namn", typeof(string));
            dt.Columns.Add("Provtyp", typeof(string));
            //dt.Columns.Add("Resultat", typeof(string));
            dt.Columns.Add("Godkänd", typeof(bool));
            dt.Columns.Add("Giltigt t.o.m.", typeof(string));


            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);

            string sql = "SELECT first_name, last_name, passed, xml_answer, test_type, valid_through FROM "
                            + "(SELECT DISTINCT ON (ct.user_id) u.first_name, u.last_name, u.team_id, ct.passed, ct.xml_answer, ct.test_id, t.test_type, t.valid_through "
                            + "FROM completed_test ct "
                            + "INNER JOIN users u "
                            + "ON u.id = ct.user_id "
                            + "INNER JOIN test t "
                            + "ON t.id = ct.test_id "
                            + "ORDER BY ct.user_id, ct.start_time DESC)b "
                            + "WHERE team_id = (SELECT id FROM team WHERE user_id = @uId) AND test_id = @tId";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("uId", leaderId);
            cmd.Parameters.AddWithValue("tId", testId);
            conn.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string name = dr["first_name"].ToString() + " " + dr["last_name"].ToString();
                string testType = dr["test_type"].ToString();
                bool passed = Convert.ToBoolean(dr["passed"]);
                string validThrough = dr["valid_through"].ToString();
                string answerXml = (dr["xml_answer"].ToString()).Trim();

                
                
                //nedan läggs provdeltagarens statistik till i en egen rad i DataTable.
                dt.Rows.Add(name, testType, passed, validThrough);
                int questionCounter = 3;
                foreach (clsQuestion cq in method.XmlToClasses(answerXml))
                {
                    questionCounter++;
                    if (dt.Columns.Count == 4)
                    {
                        foreach (clsQuestion q in method.XmlToClasses(answerXml))
                        {
                            dt.Columns.Add("Fråga: " + q.value.ToString());

                        }
                    }
                    
			                if (cq.right())
                            {

                                dt.Rows[dt.Rows.Count - 1][questionCounter] = "Rätt";
                            }
                            else
                            {
                                dt.Rows[dt.Rows.Count - 1][questionCounter] = "Fel";
                            }

                }
            }
            conn.Close();
            return dt;
        }
    }
}
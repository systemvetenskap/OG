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
        clsMethods method = new clsMethods();
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable[] dt = GetTeamList(1);// Här är provledarens id hårdkodat till 1, ska bytas ut till inloggad ledare när sessioner stöds.
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
    }
}
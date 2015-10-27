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
            //dt.Columns.Add("Resultat", typeof(string));
            dt.Columns.Add("Godkänd", typeof(bool));
            dt.Columns.Add("Giltig till", typeof(string));

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


                dt.Rows.Add(name, testType, passed, validThrough);
            }
            conn.Close();



            
            DataTable[] dtA = new DataTable[2];
            dtA[0] = dt;
            dtA[1] = dt2;
            
            return dtA;
        }
    }
}
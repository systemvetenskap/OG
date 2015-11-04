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
                try
                {
                    HttpSessionState ss = HttpContext.Current.Session;
                    if (HttpContext.Current.Session["userid"] != null)
                    {
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

                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }

               ddlTests.DataValueField = "id";
               ddlTests.DataTextField = "name";
               ddlTests.DataSource = getTests();
               ddlTests.DataBind();
            }
            clsGetHtmlElement clGetEl = new clsGetHtmlElement();

            DataTable[] dt = GetTeamList(int.Parse(HttpContext.Current.Session["userid"].ToString()));
            //gvPreviousTests.DataSource = dt[0];
            //gvPreviousTests.DataBind();
            DataTable dt = GetTeamList(int.Parse(HttpContext.Current.Session["userid"].ToString()));
            gvPreviousTests.DataSource = dt;
            gvPreviousTests.DataBind();

            prev.InnerHtml = clGetEl.getTableFixed(GetTeamList(int.Parse(HttpContext.Current.Session["userid"].ToString())), 1);

            //gvUpcomingTests.DataSource = UpcomingTests(int.Parse(HttpContext.Current.Session["userid"].ToString()));
            //gvUpcomingTests.DataBind();

            upcom.InnerHtml = clGetEl.getTableFixed(UpcomingTests(int.Parse(HttpContext.Current.Session["userid"].ToString())), 1);


            //gvStats.DataSource = testStats(int.Parse(HttpContext.Current.Session["userid"].ToString()), 3);
            //gvStats.DataBind();
           //filen.InnerHtml = clGetEl.getTableFixed(testStats(int.Parse(HttpContext.Current.Session["userid"].ToString()), 3));

            
            
        }

        /// <summary>
        /// Metoden tar emot inloggad ledares id och hämtar dennes provdeltagares senaste provresultat.
        /// </summary>
        /// <param name="leaderId"></param>
        /// <returns></returns>
        private DataTable GetTeamList(int leaderId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Namn", typeof(string));
            dt.Columns.Add("Provtyp", typeof(string));
            dt.Columns.Add("Resultat", typeof(string));
            dt.Columns.Add("Godkänd", typeof(bool));
            dt.Columns.Add("Giltigt t.o.m.", typeof(string));



            string sql = "SELECT first_name, last_name, passed, xml_answer, test_type, valid_through FROM "
                            + "(SELECT DISTINCT ON (ct.user_id) u.first_name, u.last_name, u.team_id, ct.passed, ct.xml_answer, t.test_type, t.valid_through "
                            + "FROM completed_test ct "
                            + "INNER JOIN users u "
                            + "ON u.id = ct.user_id "
                            + "INNER JOIN test t "
                            + "ON t.id = ct.test_id "
                            + "ORDER BY ct.user_id, ct.start_time DESC)b "
                            + "WHERE team_id = (SELECT id FROM team WHERE user_id = @uId)";
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);

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
            }
            catch
            {

            }

            return dt;
        }

        private DataTable UpcomingTests(int leaderId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Namn", typeof(string));
            dt.Columns.Add("Provtyp", typeof(string));
            dt.Columns.Add("Giltigt t.o.m.", typeof(string));



            string sqlUpcomingLicensTests = "SELECT first_name, last_name FROM users u"
                                            + " WHERE u.id NOT IN (SELECT user_id FROM completed_test ct)"
                                            + " AND u.id IN(SELECT id FROM users WHERE id != @uId"
                                            + " AND team_id IN(SELECT id FROM team WHERE user_id = @uId))";

            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
            try
            {
            NpgsqlCommand cmd = new NpgsqlCommand(sqlUpcomingLicensTests, conn);
            cmd.Parameters.AddWithValue("uId", leaderId);
            conn.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string name = dr["first_name"].ToString() + " " + dr["last_name"].ToString();
                string testType = "Licens";
                

                //nedan läggs provdeltagarens statistik till i en egen rad i DataTable.
                dt.Rows.Add(name, testType, "-");
            }
            conn.Close();
            }
            catch
            {
            conn.Close();
            }

            string sqlFailedTests = "SELECT first_name, last_name, test_type, valid_through FROM"
                                    + " (SELECT DISTINCT ON(ct.user_id, t.valid_through) u.first_name, u.last_name, u.team_id, t.test_type, ct.passed, t.valid_through FROM users u"
                                    + " INNER JOIN completed_test ct"
                                    + " ON u.id = ct.user_id"
                                    + " INNER JOIN test t"
                                    + " ON ct.test_id = t.id"
                                    + " ORDER BY ct.user_id, t.valid_through, ct.passed DESC, ct.start_time DESC)a"
                                    + " WHERE valid_through >=  (SELECT date_part('year',current_date))  AND passed = false AND team_id IN (SELECT id FROM team WHERE user_id = @uId)";

            try
            {

            NpgsqlCommand cmd2 = new NpgsqlCommand(sqlFailedTests, conn);
            cmd2.Parameters.AddWithValue("uId", leaderId);
            conn.Open();
            NpgsqlDataReader dr2 = cmd2.ExecuteReader();

            while (dr2.Read())
            {
                string name = dr2["first_name"].ToString() + " " + dr2["last_name"].ToString();
                string testType = dr2["test_type"].ToString();
                string validThrough = dr2["valid_through"].ToString();

                //nedan läggs provdeltagarens statistik till i en egen rad i DataTable.
                dt.Rows.Add(name, testType, validThrough);
            }
                conn.Close();
            }
            catch
            {
            conn.Close();
            }


            string sqlExpiredPassed = "SELECT DISTINCT ON(u.id) u.first_name, u.last_name, t.valid_through, t.test_type, ct.passed, ct.id FROM users u"
                                   + " INNER JOIN completed_test ct"
                                   + " ON u.id = ct.user_id"
                                   + " INNER JOIN test t"
                                   + " ON t.id = ct.test_id"
                                   + " WHERE u.id IN(SELECT id FROM users WHERE id != @uId"
                                   + " AND u.team_id IN(SELECT id FROM team WHERE user_id = @uId))"
                                   + " AND ct.passed = true"
                                   + " AND u.id NOT IN"
                                   + ""
                                   + " (SELECT id FROM"
                                   + " (SELECT DISTINCT ON(ct.user_id, t.valid_through) u.id, u.first_name, u.last_name, u.team_id, t.test_type, ct.passed, t.valid_through FROM users u"
                                   + " INNER JOIN completed_test ct"
                                   + " ON u.id = ct.user_id"
                                   + " INNER JOIN test t"
                                   + " ON ct.test_id = t.id"
                                   + " ORDER BY ct.user_id, t.valid_through, ct.passed DESC, ct.start_time DESC)a"
                                   + " WHERE valid_through >=  (SELECT date_part('year',current_date))  AND passed = false AND team_id IN (SELECT id FROM team WHERE user_id = @uId))"
                                   + ""
                                   + " AND u.id NOT IN"
                                   + ""
                                   + " (SELECT id FROM users u"
                                   + " WHERE u.id NOT IN (SELECT user_id FROM completed_test ct)"
                                   + " AND u.id IN(SELECT id FROM users WHERE id != @uId"
                                   + " AND team_id IN(SELECT id FROM team WHERE user_id = @uId)))";

            try
            {


            NpgsqlCommand cmd3 = new NpgsqlCommand(sqlExpiredPassed, conn);
            cmd3.Parameters.AddWithValue("uId", leaderId);
            conn.Open();
            NpgsqlDataReader dr3 = cmd3.ExecuteReader();

            while (dr3.Read())
            {
                string name = dr3["first_name"].ToString() + " " + dr3["last_name"].ToString();
                string testType = dr3["test_type"].ToString();
                string validThrough = "";

                    if (int.Parse(validThrough = dr3["valid_through"].ToString()) < int.Parse(DateTime.Now.Year.ToString()))
                {
                    validThrough = DateTime.Now.Year.ToString();
                }
                else if (int.Parse(validThrough = dr3["valid_through"].ToString()) == (DateTime.Now.Year))
                {
                    int thisYear = DateTime.Now.Year;
                    
                        validThrough = (thisYear + 1).ToString();
                }

                //nedan läggs provdeltagarens statistik till i en egen rad i DataTable.
                dt.Rows.Add(name, testType, validThrough);
            }
                conn.Close();
            }
            catch
            {
            conn.Close();
            }
            return dt;
        }
        private DataTable testStats(int leaderId, int testId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Namn", typeof(string));
            dt.Columns.Add("Provtyp", typeof(string));
            dt.Columns.Add("Produkt", typeof(string));
            dt.Columns.Add("Ekonomi", typeof(string));
            dt.Columns.Add("Etik", typeof(string));
            dt.Columns.Add("Total", typeof(string));
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
            try
            {


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

                //Här plockas resultaten totalt och per provdel fram via en metod som returnerar dessa som listor i en Tuple.
                Tuple<bool, List<int>, List<int>, int, int> solution = method.PartAndTotalResult(method.XmlToClasses(answerXml));
                string totalResult = solution.Item4.ToString() + "/" + solution.Item5.ToString();
                string prodResult = solution.Item2[0].ToString() + "/" + solution.Item3[0].ToString();
                string ecoResult = solution.Item2[1].ToString() + "/" + solution.Item3[1].ToString(); ;
                string ethResult = solution.Item2[2].ToString() + "/" + solution.Item3[2].ToString(); ;
                
                
                //nedan läggs provdeltagarens statistik till i en egen rad i DataTable.
                dt.Rows.Add(name, testType, prodResult, ecoResult, ethResult, totalResult, passed, validThrough);
                int questionCounter = 7;
                foreach (clsQuestion cq in method.XmlToClasses(answerXml))
                {
                    questionCounter++;
                    if (dt.Columns.Count == 8)
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
            }
            catch
            {
                conn.Close();
            }
            return dt;
        }
        private DataTable getTests()
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
                conn.Open();
                NpgsqlDataAdapter adp = new NpgsqlDataAdapter("SELECT id, name FROM test ORDER BY id desc limit 6", conn);
                adp.Fill(dt);
                conn.Close();
            }
            catch
            {

                }
                
            return dt;
        }

        //protected void gvStats_RowDataBound(object sender, GridViewRowEventArgs e)
        //{


            


        //    //if (e.Row.RowType == DataControlRowType.DataRow)
        //    //{
        //    foreach (GridViewRow row in gvStats.Rows) //För varje rad i Gridview gör detta.
        //        {

        //            int rad = gvStats.Rows.Count; //Räknar rader i GridView.


        //            for (int y = 0; y <= rad; y++) //För varje rad
        //            {
        //                for (int i = 0; i < row.Cells.Count; i++) //För varje cell
        //                {
        //                    if (i >= 0)
        //                    {
        //                        if (row.Cells[i].Text == "R&#228;tt") //Om texten i cellen[i] är "Rätt" sätt backgrundsfärgen för cellen till grön.
        //                        {
        //                            row.Cells[i].BackColor = System.Drawing.Color.LightGreen;
        //                        }

        //                        else if (row.Cells[i].Text == "Fel") //Om texten i cellen[i] är "Fel" sätt backgrundsfärgen för cellen till tomatröd.
        //                        {
        //                            row.Cells[i].BackColor = System.Drawing.Color.Tomato;
        //                        }
        //                    }
        //                }
        //            }

         
        //        }
        //    //}

        //    e.Row.Cells[0].CssClass = "lockColumns";

        //    for (int i = 1; i < e.Row.Cells.Count; i++)
        //    {
        //        e.Row.Cells[i].CssClass = "floatColumns";
        //    }

        //    //e.Row.Cells[1].CssClass = "lockColumns";
        //}

        protected void gvStats_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void ddlTests_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnShowTest_Click(object sender, EventArgs e)
        {
           // int id = int.Parse(ddlTests.SelectedValue);
            clsGetHtmlElement clGetEl = new clsGetHtmlElement();
            filen.InnerHtml = clGetEl.getTableFixed(testStats(int.Parse(HttpContext.Current.Session["userid"].ToString()), int.Parse(ddlTests.SelectedValue)), 1);

        }

    }
}
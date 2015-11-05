using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace Group3WebProject
{
    public partial class webbtest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    
                    Classes.clsLogin clsLog = new Classes.clsLogin();
                    if (clsLog.getLevel(HttpContext.Current.Session["userid"].ToString()) == "deltagare") //Inloggad
                    {
                        //Debug.WriteLine(" DU KOM IN ");
                    }
                    else //Är inloggad med fel credinatl
                    {
                        Response.Redirect("default.aspx");
                    }
                }
                else //Har inte loggat in 
                {
                    Response.Redirect("login.aspx");
                }
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "CallHandler();", true);
                
            }
            string userid = HttpContext.Current.Session["userid"].ToString();
            Classes.clsStartingTest clStart = new Classes.clsStartingTest();
            string result = clStart.getOk(userid);
           // ViewState["testID"] = "4557";
            btnSeeLastTest.Visible = true;
            btnTest.Visible = true;
            if (result == "ÅKU")
            {
                btnTest.Text = "Starta årligt test";
                Label2.Text = "Nu kan du göra årligt test";
            }
            else if (result == "LICENS")
            {
                btnSeeLastTest.Enabled = false;
                btnSeeLastTest.Visible = false;
                btnTest.Text = "Starta Licenstest";
                Label2.Text = "Nu kan du göra licens test";
            }
            else if (result == "IGÅNG")
            {
                btnSeeLastTest.Enabled = false;
                btnSeeLastTest.Visible = false;
                btnTest.Text = "Fortsätt testet";
                Label2.Text = "Testet är igång redan";
            }
            else if(result == "Du kan göra nästa års test nu")
            {
                btnTest.Text = "Gör nästa års test";
                Label2.Text = result;

            }
            else if (result == "Du måste vänta minst 7 dagar mellan proven")
            {
                btnTest.Visible = false;
                btnSeeLastTest.Visible = false;
                Label2.Text = result;
            }
            else
            {
                btnTest.Text = "Du får vänta på nästa prov";
                btnTest.Enabled = false;
                btnTest.Visible = false;
                Label2.Text = "Du får vänta på nästa prov";
                //btnTest.Enabled = false;
            }
            if (clStart.checkLasttest(userid) == false)
            {
                btnSeeLastTest.Visible = false;
            }
            
        }
        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            if (btnTest.Text == "Se senaste testet")
            {
                Response.Redirect("webbtestresult.aspx");
            }
            else if (btnTest.Text != "Fortsätt testet")
            {
                string testNa = "";
                int year = DateTime.Now.Year;
                if (btnTest.Text == "Starta Licenstest")
                {
                    testNa = "Licens";
                }
                else if(btnTest.Text == "Starta årligt test")
                {
                    testNa = "ÅKU";
                }
                else if (btnTest.Text == "Gör nästa års test")
                {
                    testNa = "ÅKU";
                    year += 1;
                }
                else
                {
                    return;
                }
                Classes.clsStartingTest clStart = new Classes.clsStartingTest();
                string result = clStart.startNew(testNa, HttpContext.Current.Session["userid"].ToString(), year);
                if (result == "false")
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "alert('Finns inget test att välja på kontakta supporten om problemet');", true);
                    return;
                }
                ViewState.Add("testID", result);

                Response.Redirect("webbtestquestion.aspx?testID=" + ViewState["testID"].ToString());
            }
            else
            {
                Classes.clsStartingTest clStart = new Classes.clsStartingTest();
                ViewState.Add("testID", clStart.getTestid(HttpContext.Current.Session["userid"].ToString()));
                Response.Redirect("webbtestquestion.aspx?testID=" + ViewState["testID"].ToString());
            }
          
        }

        protected void btnSeeLastTest_Click(object sender, EventArgs e)
        {
            Classes.clsStartingTest clStart = new Classes.clsStartingTest();
            ViewState.Add("testID", clStart.getTestid(HttpContext.Current.Session["userid"].ToString()));
            Response.Redirect("webbtestresult.aspx");
        }

    }
}
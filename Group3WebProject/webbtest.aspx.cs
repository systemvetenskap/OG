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
                    Debug.WriteLine(HttpContext.Current.Session["userid"].ToString() + " aa  ");
                    //Check if user have right credit 
                    //IF level == Provdeltahare
                    Classes.clsLogin clsLog = new Classes.clsLogin();
                    if (clsLog.getLevel(HttpContext.Current.Session["userid"].ToString()) == "deltagare") //Inloggad
                    {
                        Debug.WriteLine(" DU KOM IN ");
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
            }
            string userid = HttpContext.Current.Session["userid"].ToString();
            Classes.clsStartingTest clStart = new Classes.clsStartingTest();
            string result = clStart.getOk(userid);
           // ViewState["testID"] = "4557";
            if (result == "ÅKU")
            {
                btnTest.Text = "Starta årligt test";
            }
            else if (result == "LICENS")
            {
                btnTest.Text = "Starta Licenstest";
            }
            else if (result == "IGÅNG")
            {
                btnTest.Text = "Fortsätt testet";
            }
            else if(result == "Du kan göra nästa års test nu")
            {
                btnTest.Text = "Gör nästa års test";
            }
            else
            {
                btnTest.Text = "Se senaste testet";
                //btnTest.Enabled = false;
            }
            Label2.Text = result;
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
                    testNa = "LICENS";
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

    }
}
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
            ViewState["testID"] = "4557";
            if (result == "ÅKU")
            {
                btnTest.Text = "Starta årligt test";
            }
            else if (result == "ARBETS")
            {
                btnTest.Text = "Starta Licens test";
            }
            else if (result == "IGÅNG")
            {
                btnTest.Text = "Fortsätt testet";
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
                HttpContext.Current.Session["seeTest"] = "true";

                //ViewState.Add("seeTest", "true");
            }
            else if (btnTest.Text != "Fortsätt testet")
            {
                Classes.clsStartingTest clStart = new Classes.clsStartingTest();
                string result = clStart.startNew("ÅKU", HttpContext.Current.Session["userid"].ToString());
                ViewState.Add("testID", result);
            }
            Response.Redirect("webbtestquestion.aspx?testID=" + ViewState["testID"].ToString());
        }

    }
}
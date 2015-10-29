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
            //if (!IsPostBack)
            //{
            //    if (HttpContext.Current.Session["userid"] != null)
            //    {
            //        Debug.WriteLine(HttpContext.Current.Session["userid"].ToString() + " aa  ");
            //        //Check if user have right credit 
            //        //IF level == Provdeltahare
            //        Classes.clsLogin clsLog = new Classes.clsLogin();
            //        if (clsLog.getLevel(HttpContext.Current.Session["userid"].ToString()) == "deltagare") //Inloggad
            //        {
            //            Debug.WriteLine(" DU KOM IN ");
            //        }
            //        else //Är inloggad med fel credinatl
            //        {
            //            Response.Redirect("default.aspx");
            //        }
            //    }
            //    else //Har inte loggat in 
            //    {
            //        Response.Redirect("login.aspx");
            //    }
                
            //}
            string userid = "3";
            Classes.clsStartingTest clStart = new Classes.clsStartingTest();
            string result = clStart.getOk(userid); 
            if (result == "ÅKU")
            {

            }
            else if (result == "ARBETS")
            {
                
            }
            else
            {
                
            }
            Label2.Text = result;
        }
        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            Classes.clsStartingTest clStart = new Classes.clsStartingTest();
            string result = clStart.startNew("ÅKU", "4");
            ViewState.Add("testID", result);
         //   Response.Redirect("webbtestquestion.aspx?testID=12");
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Group3WebProject.Classes;
using System.Data;
using System.Diagnostics;
namespace Group3WebProject
{
    public partial class webbtestresult : System.Web.UI.Page
    {
        string testID;
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
                        //Label2.Text = HttpContext.Current.Session["userid"].ToString();
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

            if (ViewState["testID"] != null || Convert.ToString(ViewState["testID"]) != "")
            {
                testID = ViewState["testID"].ToString();
            }
            else
            {
                int tstID;
                clsStartingTest clsTestID = new clsStartingTest();
                testID = clsTestID.getTestid(HttpContext.Current.Session["userid"].ToString());
                Debug.WriteLine(testID + "  ALfekroek");
                if (int.TryParse(testID, out tstID))
                {
                    ViewState["testID"] = testID;
                }
            }
            if (!IsPostBack)
            {
                clsTestMenuFill menu = new clsTestMenuFill();
                DataTable dtQuestions = menu.read(testID);
                for (int i = 0; i < dtQuestions.Rows.Count; i++)
                {
                    Label quNam = new Label();
                    quNam.Text = "<h3>" + dtQuestions.Rows[i]["name"].ToString() + "</h3>";
                    panData.Controls.Add(quNam);
                    fillData(dtQuestions.Rows[i]["id"].ToString(), testID);
                }
            }
        }
        /// <summary>
        /// Lägger till varje fråga i paneln och visar om man har svarat rätt eller fel. 
        /// </summary>
        /// <param name="queID"></param>
        /// <param name="ttID"></param>
        private void fillData(string queID, string ttID)
        {
            clsFillQuestion quest = new clsFillQuestion();
            Tuple<DataTable, string, int, string, string> Que = quest.readXML(queID, ttID);
            DataTable dt = Que.Item1;

            Label lbN = new Label();
            lbN.ID = "QUEST_" + queID;
            lbN.Text = "<h5>" + Que.Item2 + "</h5>";
            panData.Controls.Add(lbN);
            int many = 0;
            for (int i = 0; i < Que.Item1.Rows.Count; i++)
            {
                if (Que.Item1.Rows[i]["answ"].ToString().ToUpper() == "TRUE")
                {
                    many += 1;
                }
            }
            if (many > 1) //Om det finns flera valmöjligheter så sätter vi ut en checkboxliist
            {
                CheckBoxList li = new CheckBoxList();
                li.ID = queID;
                li.DataTextField = "name";
                li.DataValueField = "id";
                li.DataSource = dt;
                li.DataBind();
                li.Enabled = false;
                for (int i = 0; i < dt.Rows.Count; i++)  //KRyssar i de som redan användaren har kryssat i
                {
                    int val = 0;
                    if (dt.Rows[i]["sel"].ToString().ToUpper() == "TRUE")
                    {
                        if (int.TryParse(dt.Rows[i]["id"].ToString(), out val))
                        {
                            li.Items.FindByValue(val.ToString()).Selected = true; //Sätter alla som finns till true så att den kan vara multippella
                            li.Items.FindByValue(val.ToString()).Attributes.Add("style", "background-color: red;");
                        }
                        val = 0;
                    }
                    if (dt.Rows[i]["answ"].ToString().ToUpper() == "TRUE")
                    {
                        if (int.TryParse(dt.Rows[i]["id"].ToString(), out val))
                        {
                            li.Items.FindByValue(val.ToString()).Attributes.Add("style", "background-color: green;"); //= System.Drawing.Color.Green; //Sätter alla som finns till true så att den kan vara multippella
                        }
                        val = 0;
                    }
                }
                panData.Controls.Add(li);
            }
            else
            {
                RadioButtonList li = new RadioButtonList();
                li.ID = queID;
                li.DataTextField = "name";
                li.DataValueField = "id";
                li.DataSource = Que.Item1;
                li.DataBind();
                li.Enabled = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int val = 0;
                    if (dt.Rows[i]["sel"].ToString().ToUpper() == "TRUE")
                    {
                        if (int.TryParse(dt.Rows[i]["id"].ToString(), out val))
                        {
                            li.SelectedValue = val.ToString();
                            li.Items.FindByValue(val.ToString()).Attributes.Add("style", "background-color: red;");
                        }
                    }
                    if (dt.Rows[i]["answ"].ToString().ToUpper() == "TRUE") //Om man vill kolla på frågorna igen så markeras den grön om det är okej 
                    {
                        if (int.TryParse(dt.Rows[i]["id"].ToString(), out val))
                        {
                            li.Items.FindByValue(val.ToString()).Attributes.Add("style", "background-color: green;");
                        }
                    }
                }
                panData.Controls.Add(li);
            }
        }
    }
}
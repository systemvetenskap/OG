using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Diagnostics;
using Group3WebProject.Classes;
using System.Web.SessionState;


namespace Group3WebProject
{
    //            

    public partial class webbtestquestion : System.Web.UI.Page
    {
        string testID = "2";
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
                        Label2.Text = HttpContext.Current.Session["userid"].ToString();
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
                if (ViewState["testID"] != null || Convert.ToString(ViewState["testID"]) != "")
                {
                    testID = ViewState["testID"].ToString();
                    Debug.WriteLine("FInns något ialla fall");
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
                    else
                    {
                    }
                }
               // ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:timeToEnd(); ", true);
                Label2.Text = Label2.Text + "  testID_ " + testID;
                Classes.clsTestMenuFill clMenFill = new Classes.clsTestMenuFill();
                cmbChooseQue.DataValueField = "id";
                cmbChooseQue.DataTextField = "name";
                cmbChooseQue.DataSource = clMenFill.read(testID);
                cmbChooseQue.DataBind();
                cmbChooseQue.Enabled = false;
                if (cmbChooseQue.Items.Count > 0) //Om den inte hämtat någon data så blir det felmedelande
                {
                    ViewState["alfred"] = cmbChooseQue.SelectedItem.ToString();
                    fillquestion();
                }
                else
                {
                    Label1.Text = "Något gick fel försök igen";
                    Button1.Enabled = false;
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                    rbQuestionList.Enabled = false;
                    cmbChooseQue.Enabled = false;
                }
            }
            else
            {
                Debug.WriteLine(" FEL ET ");
                testID = ViewState["testID"].ToString();
            }
            if (btnNext.Text == "Översikt")
            {
                //Do the job to check test and stop time
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Classes.clsRightOrNot cls = new Classes.clsRightOrNot();
            List<string> liAnsw = new List<string>();
            liAnsw.Add("1");
            //cls.valudateXML(testID, "1", "1");
            Label3.Text = cls.getXml(testID);
        }
        private bool fillquestion()//Hämtar frågorna 
        {
            Classes.clsFillQuestion clFill = new Classes.clsFillQuestion();
            Tuple<DataTable, string, int, string, string> getData = clFill.readXML(cmbChooseQue.SelectedValue.ToString(), testID);
            DataTable dt = getData.Item1;
            int antVal = getData.Item3;
            bool lookAgain = bool.Parse(HttpContext.Current.Session["seeTest"].ToString());
            if (lookAgain)
            {
                rbQuestionList.Enabled = false;
                chkQuestionList.Enabled = false;
            }
            Label3.Text = "Frågan är inom området:" + getData.Item4 + " <br />" + getData.Item2;
            lblChoose.Text = " Du ska välja:" + antVal.ToString() + " frågor";
            if (getData.Item5 != "")
            {
                Label2.Text = "<img src='pictures/" + getData.Item5 + "' style='height: 250px; width: 250px;'alt='bilden' />";
            }
            try
            {
                int sumCheck = 0;
                chkQuestionList.DataSource = null;
                chkQuestionList.DataBind();
                rbQuestionList.DataSource = null;
                rbQuestionList.DataBind();
                for (int i = 0; i < dt.Rows.Count; i++) //Kollar om det finns flera som går att välja 
                {
                    if (dt.Rows[i]["answ"].ToString().ToUpper() == "TRUE")
                    {
                        sumCheck += 1;
                    }
                }
                if (sumCheck > 1)//Om det finns flera val att välja på visas den listan
                {
                    btnNext.OnClientClick = "return userValid('chkQuestionList', '" + antVal + "');";
                    btnPrevious.OnClientClick = "return userValid('chkQuestionList', '" + antVal + "');";
                    rbQuestionList.Visible = false;
                    chkQuestionList.Visible = true;
                    chkQuestionList.DataTextField = "name";
                    chkQuestionList.DataValueField = "id";
                    chkQuestionList.DataSource = dt;
                    chkQuestionList.DataBind();
                    for (int i = 0; i < dt.Rows.Count; i++)  //KRyssar i de som redan användaren har kryssat i
                    {
                        int val = 0;
                        if (dt.Rows[i]["sel"].ToString().ToUpper() == "TRUE")
                        {
                            if (int.TryParse(dt.Rows[i]["id"].ToString(), out val))
                            {
                                chkQuestionList.Items.FindByValue(val.ToString()).Selected = true; //Sätter alla som finns till true så att den kan vara multippella
                            }
                            val = 0;
                        }
                        if (dt.Rows[i]["answ"].ToString().ToUpper() == "TRUE" && lookAgain == true)
                        {
                            if (int.TryParse(dt.Rows[i]["id"].ToString(), out val))
                            {
                                chkQuestionList.Items.FindByValue(val.ToString()).Attributes.Add("style", "background-color: green;"); //= System.Drawing.Color.Green; //Sätter alla som finns till true så att den kan vara multippella
                            }
                            val = 0;
                        }

                    }
                }
                else //Om det är ett val så kommer man till en radiobuttonlist
                {
                    btnNext.OnClientClick = "return userValid('rbQuestionList', '" + antVal + "');";
                    btnPrevious.OnClientClick = "return userValid('rbQuestionList', '" + antVal + "');";
                    rbQuestionList.Visible = true;
                    chkQuestionList.Visible = false;
                    rbQuestionList.DataTextField = "name";
                    rbQuestionList.DataValueField = "id";
                    rbQuestionList.DataSource = dt;
                    rbQuestionList.DataBind();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int val = 0;
                        if (dt.Rows[i]["sel"].ToString().ToUpper() == "TRUE")
                        {
                            if (int.TryParse(dt.Rows[i]["id"].ToString(), out val))
                            {
                                rbQuestionList.SelectedValue = val.ToString();
                            }
                        }
                        if (dt.Rows[i]["answ"].ToString().ToUpper() == "TRUE" && lookAgain == true)
                        {
                            if (int.TryParse(dt.Rows[i]["id"].ToString(), out val))
                            {
                                rbQuestionList.Items.FindByValue(val.ToString()).Attributes.Add("style", "background-color: green;");
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return true;
        }

        private bool checkAnswers()//Sparar svars alternativen
        {
            Classes.clsRightOrNot cls = new Classes.clsRightOrNot();
            List<string> selDat = new List<string>();
            string sele = "";
            if (chkQuestionList.Visible == true)
            {
                if (chkQuestionList.SelectedIndex > -1)
                {
                    foreach (ListItem item in chkQuestionList.Items)
                    {
                        if (item.Selected)
                        {
                            sele = (item.Value);
                            selDat.Add(item.Value);
                            Debug.WriteLine(sele);
                        }
                    }
                }
            }
            else if (rbQuestionList.Visible == true)
            {
                if (rbQuestionList.SelectedIndex > -1)
                {
                    sele = rbQuestionList.SelectedValue.ToString();
                    selDat.Add(rbQuestionList.SelectedValue.ToString());
                    Debug.WriteLine(sele);
                }
            }
            //Label1.Text = sele;
            Debug.WriteLine(sele + " SELE ");
            cls.valudateXML(testID, cmbChooseQue.SelectedValue.ToString(), selDat);

            return true;
        }
        protected void rbQuestionList_Unload(object sender, EventArgs e)
        {

        }
        protected void btnNext_Click(object sender, EventArgs e) //Näst fråga kommer man till, samma som på den tidigare
        {
            checkAnswers();
            
            ViewState["alfred"] = cmbChooseQue.SelectedValue.ToString();
            if (cmbChooseQue.Items.Count > cmbChooseQue.SelectedIndex + 1)
            {
                cmbChooseQue.SelectedIndex = cmbChooseQue.SelectedIndex + 1;
            }
            fillquestion();
            if (btnNext.Text == "Översikt")
            {
                btnNext.OnClientClick = "return wantToCont();";
            }
            if (cmbChooseQue.SelectedIndex >= cmbChooseQue.Items.Count - 1)
            {
                btnNext.Text = "Översikt";
                btnNext.OnClientClick = "return wantToCont();";
            }
            else
            {
                btnNext.OnClientClick = null;
                btnNext.Text = "Nästa";
            }
        }
        private bool ReturnValue()
        {

            return false;
        }
        protected void btnPrevious_Click(object sender, EventArgs e) //Föregående fråga kommer man till 
        {
            checkAnswers();
            ViewState["alfred"] = cmbChooseQue.SelectedItem.ToString();
            if (cmbChooseQue.Items.Count > cmbChooseQue.SelectedIndex - 1)
            {
                cmbChooseQue.SelectedIndex = cmbChooseQue.SelectedIndex - 1;
            }
            fillquestion();
            btnNext.Text = "nästa";

        }
        protected void cmbChooseQue_SelectedIndexChanged(object sender, EventArgs e)
        {
            //checkAnswers(); //Sparar svarerne
            ViewState["alfred"] = cmbChooseQue.SelectedItem.ToString();
            //fillquestion();
        }
    }
}
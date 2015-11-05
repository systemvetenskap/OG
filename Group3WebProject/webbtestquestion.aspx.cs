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
                  
                    Classes.clsLogin clsLog = new Classes.clsLogin();
                    if (clsLog.getLevel(HttpContext.Current.Session["userid"].ToString()) == "deltagare") //Inloggad
                    {
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
                }
                else
                {
                    int tstID;
                    clsStartingTest clsTestID = new clsStartingTest();
                    testID = clsTestID.getTestid(HttpContext.Current.Session["userid"].ToString());
                    if (int.TryParse(testID, out tstID))
                    {
                        ViewState["testID"] = testID;
                    }
                }
                // ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:timeToEnd(); ", true); //Skapar en timer för nedräkning javascript
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
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                    rbQuestionList.Enabled = false;
                    cmbChooseQue.Enabled = false;
                }
            }
            else
            {
                testID = ViewState["testID"].ToString();
            }
            if (btnNext.Text == "Lämna in")
            {
                checkAnswers();
                clsMethods clMeth = new clsMethods();
                clsFillQuestion clQue = new clsFillQuestion();
                clsRightOrNot clRi = new clsRightOrNot();
                
                string handle = clRi.canHandIn(ViewState["testID"].ToString());
                if (handle == "TIDEN DROG ÖVER")
                {
                    clRi.setFail(ViewState["testID"].ToString()); //Om tiden har gått över 30min så har man failat
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "alert('Du lämnade in testet för sent du blir underkänd');", true);
                    Response.Redirect("default.aspx");
                    return;
                }
                else if(handle != "OK")
                {
                    return;
                }
                string xml = clQue.getXml(ViewState["testID"].ToString());                
                Tuple<bool, List<int>, List<int>, int, int> aa = clMeth.PartAndTotalResult(clMeth.XmlToClasses(xml));//list1, 
                bool resultP = aa.Item1;
                //Debug.WriteLine("TESTID " + testID + "  " + resultP.ToString() + " res och " + aa.Item4.ToString() + " till sist " + aa.Item5);
                clRi.updateResult(ViewState["testID"].ToString(), resultP);
                Response.Redirect("webbtestresult.aspx");
            }
            string start;
            if (ViewState["startime"] == null)
            {
                clsSetGetStarttime clSta = new clsSetGetStarttime();
                start = clSta.getStarttime(ViewState["testID"].ToString()).ToString();
                ViewState.Add("startime", start.ToString());
            }
            else
            {
                start = ViewState["startime"].ToString();
            }
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "CallHandler('" + start + "'); ", true);
            
           
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
           
        }
        /// <summary>
        /// Laddar in frågorna i antingen checboxlist eller radiobuttonlist
        /// </summary>
        /// <returns></returns>
        private bool fillquestion()//Hämtar frågorna 
        {
            Label1.Text = "Du är på fråga: " + cmbChooseQue.SelectedItem.Text + " av " + cmbChooseQue.Items.Count.ToString() + " frågor";
            Classes.clsFillQuestion clFill = new Classes.clsFillQuestion();
            Tuple<DataTable, string, int, string, string> getData = clFill.readXML(cmbChooseQue.SelectedValue.ToString(), testID);
            DataTable dt = getData.Item1;
            int antVal = getData.Item3;
            bool lookAgain = false;
            string part = getData.Item4.ToUpper();
            if (HttpContext.Current.Session["seeTest"] != null)
            {
                lookAgain = bool.Parse(HttpContext.Current.Session["seeTest"].ToString());
                if (lookAgain) //Om man vill titta igen får man inte kryssa i något
                {
                    rbQuestionList.Enabled = false;
                    chkQuestionList.Enabled = false;
                }
            }
            
            if (part == "ETIK")
            {
                part = " Etik och regelverk";
            }
            else if(part == "EKONOMI")
            {
                part = " Ekonomi – nationalekonomi, finansiell ekonomi och privatekonomi.";
            }
            else if (part == "PRODUKTER")
            {
                part = " Produkter och hantering av kundens affärer ";
            }
            Label3.Text = "Frågan är inom området:" + part + " <br />" + "<h4>" + getData.Item2 + "</h4>";
            ViewState.Add("antQue", antVal.ToString());
            lblChoose.Text = " Du ska välja: <b>" + antVal.ToString() + "</b> svar";
            if (getData.Item5 != "")
            {
                Label2.Text = "<img src='pictures/" + getData.Item5 + "' style='height: 250px; width: 250px;'alt='bilden' />";
            }
            else
            {
                Label2.Text = "";
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
                        if (dt.Rows[i]["answ"].ToString().ToUpper() == "TRUE" )
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
                        if (dt.Rows[i]["answ"].ToString().ToUpper() == "TRUE" ) //Om man vill kolla på frågorna igen så markeras den grön om det är okej 
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
                }
            }
            //Label1.Text = sele;
            cls.valudateXML(testID, cmbChooseQue.SelectedValue.ToString(), selDat); //SKickar en list så att flera val kan väljas

            return true;
        }
        protected void rbQuestionList_Unload(object sender, EventArgs e)
        {

        }
        protected void btnNext_Click(object sender, EventArgs e) //Näst fråga kommer man till, samma som på den tidigare
        {
            //btnNext.OnClientClick = "return userValid('rbQuestionList', '" + antVal + "');";
           
            checkAnswers();
           
            string handIN = "";
            if (rbQuestionList.Visible == true)
            {
                handIN = "return wantToCont('rbQuestionList', '" + ViewState["antQue"].ToString() + "')";
            }
            else
            {
                handIN = "return wantToCont('chkQuestionList', '" + ViewState["antQue"].ToString() + "')";
            }
            ViewState["alfred"] = cmbChooseQue.SelectedValue.ToString();
            if (cmbChooseQue.Items.Count > cmbChooseQue.SelectedIndex + 1)
            {
                cmbChooseQue.SelectedIndex = cmbChooseQue.SelectedIndex + 1;
            }
            fillquestion();
            
            if (btnNext.Text == "Lämna in")
            {
                btnNext.OnClientClick = handIN; 
            }
            if (cmbChooseQue.SelectedIndex >= cmbChooseQue.Items.Count - 1)
            {
                btnNext.Text = "Lämna in";
                btnNext.CssClass = "btn btn-succes";
                btnNext.OnClientClick = handIN; //Frågar om man vill gå vidare och lämna in det 
            }
            else
            {
                
                btnNext.CssClass = "btn";
              //  btnNext.OnClientClick = null;
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
            //cmbChooseQue.Items.Count
            btnNext.CssClass = "btn";
            btnNext.Text = "Nästa"; //Sätter nästa
        }
        protected void cmbChooseQue_SelectedIndexChanged(object sender, EventArgs e)
        {
            //checkAnswers(); //Sparar svarerne
           // ViewState["alfred"] = cmbChooseQue.SelectedItem.ToString();
            //fillquestion();
            
        }

        


        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Diagnostics;
namespace Group3WebProject
{
    public partial class webbtestquestion : System.Web.UI.Page
    {
        string testID = "2";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Debug.WriteLine("Startat");
                // Classes.clsSetGetStarttime clSetStart = new Classes.clsSetGetStarttime();
                // HttpCookie myCookie = clSetStart.getStart();
                //if (myCookie == null)
                //{
                //    Response.Cookies.Add(myCookie);
                //}


                //  Classes.clsFillMenu aa = new Classes.clsFillMenu();
                Classes.clsTestMenuFill clMenFill = new Classes.clsTestMenuFill();
                cmbChooseQue.DataValueField = "id";
                cmbChooseQue.DataTextField = "name";
                cmbChooseQue.DataSource = clMenFill.read(testID);

                cmbChooseQue.DataBind();
                if (cmbChooseQue.Items.Count > 0)
                {
                    ViewState["alfred"] = cmbChooseQue.SelectedItem.ToString();
                    fillquestion();
                    //checkedRadion();
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

            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            fillquestion();
            ViewState["alfred"] = cmbChooseQue.SelectedValue.ToString();
            Classes.clsRightOrNot cls = new Classes.clsRightOrNot();
           // Label2.Text = cls.allReadyCheckd(cmbChooseQue.SelectedValue.ToString(), testID);

        }
        private bool fillquestion()//Hämtar frågorna 
        {
            Classes.clsFillQuestion clFill = new Classes.clsFillQuestion();
            DataTable dt = clFill.readXML(cmbChooseQue.SelectedValue.ToString(), testID);
            // Label1.Text = //DataTable dt = clFill.readXML(cmbChooseQue.SelectedValue.ToString(), "1");
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
                if (sumCheck > 1)
                {
                    chkQuestionList.DataTextField = "name";
                    chkQuestionList.DataValueField = "id";
                    chkQuestionList.DataSource = dt;
                    chkQuestionList.DataBind();
                    for (int i = 0; i < dt.Rows.Count; i++)  //KRyssar i de som redan användaren har kryssat i
                    {
                        int val = 0;
                        if (int.TryParse(dt.Rows[i]["sel"].ToString(), out val))
                        {
                            chkQuestionList.SelectedValue = val.ToString();
                        }
                    }
                }
                else //Om det är ett val så kommer man till en radiobuttonlist
                {
                    rbQuestionList.DataTextField = "name";
                    rbQuestionList.DataValueField = "id";
                    rbQuestionList.DataSource = dt;
                    rbQuestionList.DataBind();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int val = 0;
                        if (int.TryParse(dt.Rows[i]["sel"].ToString(), out val))
                        {
                            rbQuestionList.SelectedValue = val.ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                // Label1.Text = ex.ToString();
            }

            return true;
        }
       
        private bool checkAnswers(RadioButtonList aa)//Sparar svars alternativen
        {
            return true;
            try
            {
                Classes.clsRightOrNot cls = new Classes.clsRightOrNot();
                if (aa.SelectedIndex > 0)
                {
                    Label3.Text = cls.saveAnswers(cmbChooseQue.SelectedValue.ToString(), aa.SelectedValue.ToString(), testID);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }
        protected void rbQuestionList_Unload(object sender, EventArgs e)
        {

        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            checkAnswers(rbQuestionList);
            ViewState["alfred"] = cmbChooseQue.SelectedValue.ToString();
            if (cmbChooseQue.Items.Count > cmbChooseQue.SelectedIndex + 1)
            {
                cmbChooseQue.SelectedIndex = cmbChooseQue.SelectedIndex + 1;
            }
            fillquestion();
        }
        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            checkAnswers(rbQuestionList);
            ViewState["alfred"] = cmbChooseQue.SelectedItem.ToString();
            if (cmbChooseQue.Items.Count > cmbChooseQue.SelectedIndex - 1)
            {
                cmbChooseQue.SelectedIndex = cmbChooseQue.SelectedIndex - 1;
            }
            fillquestion();
        }
        protected void cmbChooseQue_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkAnswers(rbQuestionList);
            ViewState["alfred"] = cmbChooseQue.SelectedItem.ToString();
            fillquestion();
        }
    }
}
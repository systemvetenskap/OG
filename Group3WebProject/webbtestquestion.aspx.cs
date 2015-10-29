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
    public partial class webbtestquestion : System.Web.UI.Page
    {
        string testID = "2";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpSessionState ss = HttpContext.Current.Session;
                //HttpContext.Current.Session["test"] = "test";
                //HttpContext.Current.Response.Write(ss.StaticObjects["username"]);
                System.Diagnostics.Debug.WriteLine(HttpContext.Current.Session["username"].ToString() + " 32 ");
                if (HttpContext.Current.Session["level"] != null)
                {
                    if (HttpContext.Current.Session["level"].ToString() == "2")
                    {
                        Debug.WriteLine(" DU KOM IN ");
                    }
                }
                //string tre = Session.StaticObjects["level"].ToString();
                //Debug.WriteLine(tre);
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
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
           // fillquestion();
           // ViewState["alfred"] = cmbChooseQue.SelectedValue.ToString();
            Classes.clsRightOrNot cls = new Classes.clsRightOrNot();
            List<string> liAnsw = new List<string>();
            liAnsw.Add("1");
            cls.valudateXML(testID, "1", "1");
            Label3.Text = cls.getXml(testID);
        }
        private bool fillquestion()//Hämtar frågorna 
        {
            Classes.clsFillQuestion clFill = new Classes.clsFillQuestion();
            DataTable dt = clFill.readXML(cmbChooseQue.SelectedValue.ToString(), testID);
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
                        }                        
                    }
                }
                else //Om det är ett val så kommer man till en radiobuttonlist
                {
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
                    Debug.WriteLine(sele);
                }
            }
            //Label1.Text = sele;
            Debug.WriteLine(sele + " SELE ");
            cls.valudateXML(testID, cmbChooseQue.SelectedValue.ToString(), sele);

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
            if (cmbChooseQue.SelectedIndex >= cmbChooseQue.Items.Count - 1)
            {
                btnNext.Text = "Översikt";
            }
            else
            {
                btnNext.Text = "nästa";
            }
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
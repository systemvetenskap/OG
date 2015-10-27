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
                cmbChooseQue.DataSource = clMenFill.read("1");
                //Debug.WriteLine(aa.read(Server.MapPath("~/questions.xml")).Rows.Count.ToString());
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
            Label2.Text = cls.allReadyCheckd(cmbChooseQue.SelectedValue.ToString(), testID);

        }
        private bool fillquestion()//Hämtar frågorna 
        {
            Classes.clsFillQuestion clFill = new Classes.clsFillQuestion();
            DataTable dt = clFill.readXML(cmbChooseQue.SelectedValue.ToString(), "1");
            Label1.Text = dt.Rows.Count.ToString();
            try
            {
                rbQuestionList.DataTextField = "name";
                rbQuestionList.DataValueField = "id";
                rbQuestionList.DataSource = dt;
                rbQuestionList.DataBind();


            }
            catch (Exception ex)
            {
               // Label1.Text = ex.ToString();
            }
           
            return true;
        }
        private void checkedRadion()//Vilken som redan är vald och då checkar den i uppstarten 
        {
            Classes.clsRightOrNot cls = new Classes.clsRightOrNot();
            string strVal = cls.allReadyCheckd(cmbChooseQue.SelectedValue.ToString(), testID);
            Label2.Text = strVal;
            int val = 0;
            if (int.TryParse(strVal, out val))
            {
                rbQuestionList.SelectedValue = val.ToString();
            }
            else
            {

            }
        }

        private bool checkAnswers(RadioButtonList aa)//Sparar svars alternativen
        {
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
            // Label1.Text = ViewState["alfred"].ToString();
            checkAnswers(rbQuestionList);         

            ViewState["alfred"] = cmbChooseQue.SelectedValue.ToString();
            if (cmbChooseQue.Items.Count > cmbChooseQue.SelectedIndex + 1)
            {
                cmbChooseQue.SelectedIndex = cmbChooseQue.SelectedIndex + 1;
            }
            fillquestion();

            checkedRadion();
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

            checkedRadion();
        }
        protected void cmbChooseQue_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkAnswers(rbQuestionList);         

            ViewState["alfred"] = cmbChooseQue.SelectedItem.ToString();
            fillquestion();

            checkedRadion();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.SessionState;
namespace Group3WebProject
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
            DataTable dt = new DataTable();
            dt.Columns.Add("name");
            dt.Columns.Add("id");
            dt.Rows.Add("Alfred", "1");
            dt.Rows.Add("Anna", "2");

            DropDownList1.DataValueField = "id";
            DropDownList1.DataTextField = "name";
            DropDownList1.DataSource = dt;
            DropDownList1.DataBind();
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string userid = DropDownList1.SelectedValue;
            Label1.Text = userid;
            HttpSessionState ss = HttpContext.Current.Session; //Går till sessionen som finns
            HttpContext.Current.Session["username"] = "test";
            HttpContext.Current.Session["userid"] = "321";
            HttpContext.Current.Session["level"] = "2"; 
        }
    }
}
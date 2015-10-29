﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.SessionState;
using Group3WebProject;
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
            Classes.clsLogin clsUs = new Classes.clsLogin();
            ddlAllUser.DataValueField = "id";
            ddlAllUser.DataTextField = "name";
            ddlAllUser.DataSource = clsUs.getUsers();
            ddlAllUser.DataBind();
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlAllUser
            string userid = ddlAllUser.SelectedValue;
            Label1.Text = userid;
           
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            HttpSessionState ss = HttpContext.Current.Session; //Går till sessionen som finns
            HttpContext.Current.Session["username"] = ddlAllUser.Text;
            HttpContext.Current.Session["userid"] = ddlAllUser.SelectedValue;
            //HttpContext.Current.Session["level"] = "2"; 
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web.Configuration;
using Npgsql;
using System.Configuration;
using Group3WebProject.Classes;
using System.Web.Caching;
using System.Web.SessionState;

namespace Group3WebProject
{
    public partial class headsite : System.Web.UI.MasterPage
    {

    //    clsUsers user;
    //    private static List<clsUsers> userList = new List<clsUsers>();
    //    clsUsers sessionUser;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["usrLevel"] != null)
            {
                if (HttpContext.Current.Session["usrLevel"].ToString() == "provledare")
                {

                }
                else if (HttpContext.Current.Session["usrLevel"].ToString() == "provledare")
                {

                }
                else
                {

                }
            }
            else
            {

            }
        }

    //    protected void ddl_users_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        sessionUser = new clsUsers();
    //        sessionUser = userList[ddl_users.SelectedIndex];
            
    //    }

    }
}
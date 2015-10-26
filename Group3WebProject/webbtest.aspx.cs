using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Group3WebProject
{
    public partial class webbtest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                Classes.clsgetTests cls = new Classes.clsgetTests();
                gridTest.DataSource = cls.getTest();
                gridTest.DataBind();
            }
        }
        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("webbtestquestion.aspx?testID=12");
        }

    }
}
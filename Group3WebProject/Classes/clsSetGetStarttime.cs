using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
namespace Group3WebProject.Classes
{
    public class clsSetGetStarttime
    {
        DateTime startTime;
        public void starting()
        {
            startTime = DateTime.Now;
            
        }
        public HttpCookie getStart()
        {

            HttpCookie myCookie = new HttpCookie("UserSettings");
            myCookie["Font"] = "Arial";
            myCookie["Color"] = "Blue";
            myCookie.Expires = DateTime.Now.AddDays(1d);
           
            return myCookie;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Data;
using System.Diagnostics;
namespace Group3WebProject
{
    /// <summary>
    /// Summary description for webbtesttime
    /// </summary>
    public class webbtesttime : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["type"] == "tim")
            {
                string startTime = context.Request["time"];
                context.Response.ContentType = "text/html";
                JavaScriptSerializer java = new JavaScriptSerializer();
                string time = java.Serialize(aa(startTime));
                context.Response.Write(time);
            }
            //else
            //{
            //    string testID = context.Request["test"];
            //    Classes.clsGetHtmlElement clGetEl = new Classes.clsGetHtmlElement();
            //    admin vv = new admin();
            //    //int par = int.Parse(HttpContext.Current.Session["userid"].ToString());
            //    string Name = "1";
            //    //if (HttpContext.Current.Session["userid"] != null)
            //    //    Name = HttpContext.Current.Session["userid"].ToString();
            //    int par = int.Parse(Name);

            //    string res = clGetEl.getTableFixed(vv.testStats(par, int.Parse(testID)), 1);
            //    context.Response.Write(res);
            //}



        }
        private getTime aa(string startTime)
        {
            getTime tim = new getTime();
            DateTime start;
            start = DateTime.Parse(startTime);
            int h = start.Hour;
            int m = start.Minute;
            int s = start.Second;
            int totSec = (h * 3600) + (m * 60) + (s) + (30 * 60); //Sluttiden är 30min efter starttiden   21:30
            DateTime timNo = DateTime.Now;
            int noSec = timNo.Hour * 3600 + timNo.Minute * 60 + timNo.Second;   //21:30-21:10 
            int timeLeft = totSec - noSec;
            string handBack = "";
            if (timeLeft < 60) //Om tiden är mindre än 60sekunder 
            {
                tim.status = "Bad";
            }
            else
            {
                tim.status = "Good";
            }
            if (timeLeft < 0)
            {
                tim.status = "false";
                handBack = "Tiden gick ut";
            }
            else
            {
                h = (timeLeft / 3600);
                m = (timeLeft / 60) - (h * 3600);
                s = timeLeft - (m * 60) - (h * 3600);
                if (fixFormat(h, true).Length > 0)
                {
                    handBack += fixFormat(h, true) + ":";
                }
                handBack +=   fixFormat(m, false) + ":" + fixFormat(s, false);
            }
            
            tim.timeLeft = handBack;
            tim.timeNow = DateTime.Now.ToString();
            return tim;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private string fixFormat(int inp, bool h)
        {
            if (h == true && inp < 1)
            {
                return "";
            }
            if (inp < 1)
            {
                return "00";
            }
            else if(inp < 10)
            {
                return "0" + inp.ToString();
            }
            else
            {
                return inp.ToString();
            }
        }

    }
    public class getTime
    {
        public string timeLeft { set; get; }
        public string timeNow { set; get; }
        public string status { set; get; }
    }
}
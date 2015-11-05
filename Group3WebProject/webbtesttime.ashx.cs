using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Group3WebProject
{
    /// <summary>
    /// Summary description for webbtesttime
    /// </summary>
    public class webbtesttime : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string startTime = context.Request["time"];
            context.Response.ContentType = "text/html";
            JavaScriptSerializer java = new JavaScriptSerializer();
            string time = java.Serialize(aa(startTime));
            context.Response.Write(time);
        }
        private getTime aa(string startTime)
        {
            DateTime start;
            
            start = DateTime.Parse(startTime);

            int h = start.Hour;
            int m = start.Minute;
            int s = start.Second;
            int totSec = (h * 3600) + (m * 60) + (s) + (30 * 60); //Sluttiden är 30min efter starttiden   21:30
            DateTime timNo = DateTime.Now;
            int noSec = timNo.Hour * 3600 + timNo.Minute * 60 + timNo.Second;   //21:30-21:10 
            int timeLeft = totSec - noSec;
            string handBack;
            if (timeLeft < 0)
            {
                handBack = "Tiden gick ut";
            }
            else
            {
                h = (timeLeft / 3600);
                m = (timeLeft / 60) - (h * 3600);
                s = timeLeft - (m * 60) - (h * 3600);
                handBack = h + ":" + m + ":" + s;
            }
            getTime tim = new getTime();
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
    }
    public class getTime
    {
        public string timeLeft { set; get; }
        public string timeNow { set; get; }
    }
}
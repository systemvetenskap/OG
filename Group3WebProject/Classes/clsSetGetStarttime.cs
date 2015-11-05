using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Data;
using Npgsql;
using System.Diagnostics;
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
        public DateTime getStarttime(string testID)
        {
            DateTime result = DateTime.Parse("1970-01-01 12:12:12");
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JE"].ConnectionString);
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT  id, start_time as sta FROM completed_test  where id= @testID", conn);
                cmd.Parameters.AddWithValue("testID", int.Parse(testID));
              
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    result = DateTime.Parse(dr["sta"].ToString());
                }
                dr.Close();
                conn.Close();
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return result;

        }
    }
}
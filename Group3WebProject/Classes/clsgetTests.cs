using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Group3WebProject.Classes
{
    public class clsgetTests
    {
        public DataTable getTest()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Name");
            dt.Columns.Add("Level");
            dt.Rows.Add("1", "Vidareutbildning", "Åku");
            dt.Rows.Add("2", "Sommarutibldning", "Sku");
            dt.Rows.Add("3", "Grunläggande kurs", "Åku");

            return dt;

        }
    }
}
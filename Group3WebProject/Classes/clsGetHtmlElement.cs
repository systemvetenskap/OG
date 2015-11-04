using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Diagnostics;
namespace Group3WebProject.Classes
{
    public class clsGetHtmlElement
    {
        public string getTableFixed(DataTable dt, int antCols)
        {
            string ht = "<table class='tabFixRel'>";
            ht += "<tr>";
            for (int z = 0; z < dt.Columns.Count; z++ )
            {
                if (z < antCols)
                {
                    ht += "<th class='fixColumns' style='margin-left:" + ((z) * 100).ToString() + "px;'>" + dt.Columns[z].ColumnName.ToString() + "</th>";
                    Debug.WriteLine(ht);
                }
                else
                {
                    ht += "<th class='relColumns'>" + dt.Columns[z].ColumnName.ToString()  + "</th>";
                }
            }
            ht += "</tr>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ht += "<tr>";
                    for (int z = 0; z < dt.Columns.Count; z++)
                    {
                        string value = dt.Rows[i][z].ToString();
                        if ((value.ToUpper() == "FEL") || (value.ToUpper() == "FALSE"))
                        {
                            value = "<img src='pictures/wrong.jpg' style='height:23px; width:auto'></img>";
                        }
                        if ((value.ToUpper() == "RÄTT") || (value.ToUpper() == "TRUE"))
                        {
                            value = "<img src='pictures/right.jpg' style='height:23px; width:auto'></img>";
                        }
                        if (z < antCols)
                        {
                            ht += "<td class='fixColumns' style='margin-left:" + ((z) * 100).ToString() + "px;'>" + value + "</td>";
                            Debug.WriteLine(ht);
                        }
                        else
                        {
                            ht += "<td class='relColumns'>" + value + "</td>";
                        }
                    }
                    ht += "</tr>";
                }
            ht += "</table>";
            return ht;
        }
        public string getCheckList(DataTable dt)
        {


            return "";
        }
    }

}
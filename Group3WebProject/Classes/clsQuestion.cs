using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Group3WebProject.Classes;
using System.Xml.Serialization;

namespace Group3WebProject.Classes
{   
    [Serializable]
    public class clsQuestion 
    {
        public int value { get; set; }
        public string txt { get; set; }
        public string part { get; set; }

        public List<clsAnswer> answers { get; set; }

        public bool right()
        {
            bool right = true;
            foreach (clsAnswer ca in answers)
            {
                if( ca.answ != ca.selected)
                {
                    right = false;
                    break;
                }
            }
            return right;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
namespace Group3WebProject.Classes 
{
    [Serializable]

    public class clsAnswer 
    {
        public int id { get; set; }
        public bool answ { get; set; }
        public bool selected { get; set; }
        public string text { get; set; }

    }
}
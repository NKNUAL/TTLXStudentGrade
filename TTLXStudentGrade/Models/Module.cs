using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTLXStudentGrade.Models
{
    public class Module
    {
        public int key { get; set; }
        public string name { get; set; }
        public string mimg { get; set; }
        public List<ChildModule> childs { get; set; }
    }

    public class ChildModule
    {
        public int key { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string mimg { get; set; }
    }
}
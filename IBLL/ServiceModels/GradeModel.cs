using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBLL.ServiceModels
{
    public class GradeModel
    {

        public string Lexueid { get; set; }
        public string Kaohao { get; set; }
        public string UserName { get; set; }
        public string ClassName { get; set; }
        public int WorkCount { get; set; }
        public double MaxScore { get; set; }
        public double MinScore { get; set; }
        public double AvgScore { get; set; }
    }
}
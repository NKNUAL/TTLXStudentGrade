using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBLL.ServiceModels
{
    public class SchoolCompareModel
    {

        public string SchoolName { get; set; }
        public double JoinExamCount { get; set; }
        public double PassCount { get; set; }
        public double PassingRate { get; set; }
        public string PassingRate_P { get; set; }//百分比
        public double AvgScore { get; set; }
        public double MaxScore { get; set; }
        public double MinScore { get; set; }
        public double StandardDeviation { get; set; }
        public double? ZScore { get; set; }
        public int A { get; set; }//优
        public int B { get; set; }//良
        public int C { get; set; }//中
        public int D { get; set; }//差

    }
}
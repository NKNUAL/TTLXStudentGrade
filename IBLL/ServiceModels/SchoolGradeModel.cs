using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.ServiceModels
{
    public class SchoolGradeModel
    {
        public int JoinSpecialtyCount { get; set; }
        public int JoinCultureCount { get; set; }
        public int JoinBothCount { get; set; }
        public double SpecialtyMaxScore { get; set; }
        public double SpecialtyMinScore { get; set; }
        public double SpecialtyAvgScore { get; set; }
        public double CultureMaxScore { get; set; }
        public double CultureMinScore { get; set; }
        public double CultureAvgScore { get; set; }
    }
}

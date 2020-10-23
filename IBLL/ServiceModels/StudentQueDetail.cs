using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.ServiceModels
{
    public class StudentQueDetail
    {

        public double StudentSpecialtyScore { get; set; }

        public double ProvinceAvgScore { get; set; }

        public double ProvinceMaxScore { get; set; }

        public double ProvinceMinScore { get; set; }

        public double ChineseScore { get; set; }
        public double MathScore { get; set; }
        public double EnglishScore { get; set; }
        public double StudentCultureScore { get; set; }
        public double TotalScore
        {
            get
            {
                return StudentSpecialtyScore + StudentCultureScore;
            }
            set
            {
            }
        }
        public List<StudentQueDetailModel> QueDetail { get; set; }
    }
}

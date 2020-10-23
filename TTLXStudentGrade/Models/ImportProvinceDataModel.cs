using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTLXStudentGrade.Models
{
    public class ImportProvinceDataModel
    {

        public PlanModel PlanModel { get; set; }

        public ProvinceScoreModel ProvinceScore { get; set; }

        public List<StudentScoreModel2> StudentModels { get; set; }
    }

    public class PlanModel2
    {
        public string PlanName { get; set; }

        public string ProName { get; set; }

        public string SpecialtyName { get; set; }
    }

    public class ProvinceScoreModel
    {
        public double MaxScore { get; set; }
        public double MinScore { get; set; }
        public double AvgScore { get; set; }
        public int ExamCount { get; set; }
        public double? EffectiveScore { get; set; }
    }
}
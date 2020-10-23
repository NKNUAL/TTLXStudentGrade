using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTLXStudentGrade.Models
{
    public class ImportExcelGradeModel
    {
        public PlanModel PlanModel { get; set; }

        public List<SchoolScoreModel2> SchoolScoreModels { get; set; }

        public List<StudentScoreModel2> StudentModels { get; set; }
    }

    public class PlanModel
    {
        public string PlanName { get; set; }

        public string SchoolName { get; set; }

        public string SpecialtyName { get; set; }
    }

    public class StudentScoreModel2
    {
        public string Kaohao { get; set; }
        public string UserName { get; set; }
        public double StudentScore { get; set; }
        public int SchoolRank { get; set; }
        public int ProvinceRank { get; set; }
        public string SchoolName { get; set; }
        public int? AnswerTime { get; set; }
    }

    public class SchoolScoreModel2
    {
        public string RegionName { get; set; }
        public double MaxScore { get; set; }
        public double MinScore { get; set; }
        public double AvgScore { get; set; }
        public int ExamCount { get; set; }
        public double? EffectiveScore { get; set; }

    }


}
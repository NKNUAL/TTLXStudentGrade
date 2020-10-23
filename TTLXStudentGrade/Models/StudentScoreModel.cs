using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTLXStudentGrade.Models
{
    public class StudentScoreModel
    {
        public string Kaohao { get; set; }
        public string UserName { get; set; }
        public double StudentScore { get; set; }
        public int Rank { get; set; }
        public string SchoolCode { get; set; }
        public string SchoolName { get; set; }
        public string SpecialtyName { get; set; }
        public int? AnswerTime { get; set; }
    }

    public class SchoolScoreModel
    {
        public string SpecialtyName { get; set; }
        public double MaxScore { get; set; }
        public double MinScore { get; set; }
        public double AvgScore { get; set; }
        public int ProvinceExamCount { get; set; }
        public int SchoolExamCount { get; set; }
        public double? EffectiveScore { get; set; }

    }

    public class SchoolResultModel
    {
        public SchoolScoreModel schoolScore { get; set; }
        public List<StudentScoreModel> studentScores { get; set; }
    }
}
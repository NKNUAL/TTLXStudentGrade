using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBLL.ServiceModels
{
    public class ImportExcelGradeModel
    {
        public PlanModel PlanModel { get; set; }
        public List<SchoolScoreModel2> SpecialtySchoolScore { get; set; }
        public List<SchoolScoreModel2> CultureSchoolScore { get; set; }
        public List<StudentScoreModel> StudentModels { get; set; }
    }

    public class PlanModel
    {
        public string PlanName { get; set; }

        public string SchoolName { get; set; }

        public string SpecialtyName { get; set; }
    }

    public class StudentScoreModel2
    {
        public string Lexueid { get; set; }
        public string UserName { get; set; }
        public double StudentScore { get; set; }
        public int SchoolRank { get; set; }
        public int ProvinceRank { get; set; }
        public string SchoolName { get; set; }
        private int? _answerTime;
        public int? AnswerTime
        {
            get
            {
                if (_answerTime > 500)
                    return 0;
                return _answerTime;
            }
            set
            {
                _answerTime = value;
            }
        }
        public double SelectScore { get; set; }//选择题分数
        public double WinScore { get; set; }//windows题分数
        public double NetScore { get; set; }//网络题分数
        public double WordScore { get; set; }//word题分数
        public double PptScore { get; set; }//ppt题分数
        public double ExcelScore { get; set; }//excel题分数
        public double ProgramScore { get; set; }//编程题分数
        public double AccessScore { get; set; }//access分数
        public double CultureStudentScore { get; set; }//文化课总分
        public double ChineseScore { get; set; }//语文分数
        public double MathScore { get; set; }//数学分数
        public double EnglishScore { get; set; }//英语分数
        public double StudentTotalScore { get { return CultureStudentScore + StudentScore; } set { } }//总分
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
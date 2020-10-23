using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBLL.ServiceModels
{
    public class StudentScoreModel
    {
        public string Lexueid { get; set; }
        public string UserName { get; set; }
        public double StudentScore { get; set; }//专业课总分
        public int Rank { get; set; }
        public string SchoolCode { get; set; }
        public string SchoolName { get; set; }
        public string SpecialtyName { get; set; }
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
        public string AnswerDetail { get; set; }
        public double CultureStudentScore { get; set; }//文化课总分
        public double ChineseScore { get; set; }//语文分数
        public double MathScore { get; set; }//数学分数
        public double EnglishScore { get; set; }//英语分数
        public double StudentTotalScore { get { return CultureStudentScore + StudentScore; } set { } }

        public StudentScoreModel Clone()
        {
            return (StudentScoreModel)MemberwiseClone();
        }
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
        public List<SchoolScoreModel> schoolScore { get; set; }
        public List<StudentScoreModel> studentScores { get; set; }
        public bool IsComputerSpecialty { get; set; }
    }
}
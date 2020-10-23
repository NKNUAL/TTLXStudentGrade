using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.ServiceModels
{
    public class StudentPaperData
    {
        public string ExamDate { get; set; }//考试时间
        public int PlanID { get; set; }//考试ID
        public string PlanName { get; set; }//考试ID
        public int ExamPaperID { get; set; }//试卷ID
        public string ExamPaperName { get; set; }//试卷名称
        public double StudentScore { get; set; }
    }


    public class StudentPaperDetail
    {
        public string ExamDate { get; set; }//考试时间

        public string ExamPaperID { get; set; }//试卷ID

        public string ExamPaperName { get; set; }//试卷名称

        public int SchoolRank { get; set; }//学校排名

        public int SchoolCount { get; set; }//本场考试学校参考人数

        public int ProvinceRank { get; set; }//省排名

        public int ProvinceCount { get; set; }//本场考试省份参考人数

        public double StudentScore { get; set; }

        public List<StudentQueDetailModel> AnswerDetail { get; set; }
    }


}

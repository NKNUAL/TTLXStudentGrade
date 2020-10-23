using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTLXStudentGrade.Models
{
    public class QuestionDetailModel
    {

        public string QueName { get; set; }
        public string CorrectAnswer { get; set; }
        public string FirstAnswer { get; set; }
        public string SecondAnswer { get; set; }
        public string ThirdAnswer { get; set; }
        public string OtherAnswer { get; set; }
    }

    public class StudentQueDetailModel
    {
        public string QueName { get; set; }
        public string CorrectAnswer { get; set; }
        public string StudentAnswer { get; set; }
    }

    public class QuestionQueryModel
    {
        public string QueNo { get; set; }
        public string QueName { get; set; }
        public string CorrectAnswer { get; set; }
    }

    public class StuAnswerModel
    {
        public string StuAnswer { get; set; }
        public int AnswerCount { get; set; }
    }
}
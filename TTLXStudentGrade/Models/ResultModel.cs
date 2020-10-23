using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTLXStudentGrade.Models
{
    public class ResultModel
    {
        public int code { get; set; }

        public string message { get; set; }

        public dynamic data { get; set; }
    }

    public class ResultModel2
    {
        public bool success { get; set; }

        public dynamic data { get; set; }
    }
}
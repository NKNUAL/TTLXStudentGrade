using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTLXStudentGrade.Models
{
    public class HttpResultModel
    {

        public bool success { get; set; }

        public string message { get; set; }

        public dynamic data { get; set; }
    }
}
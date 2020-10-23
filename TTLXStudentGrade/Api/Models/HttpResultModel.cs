using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace TTLXStudentGrade.Api.Models
{
    public class HttpResultModel
    {
        public bool success { get; set; }

        public string message { get; set; }

        public dynamic data { get; set; }

    }
}
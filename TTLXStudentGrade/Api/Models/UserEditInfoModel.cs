using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTLXStudentGrade.Api.Models
{
    public class UserEditInfoModel
    {
        public string Lexueid { get; set; }
        public string NewPassword { get; set; }
        public string UserName { get; set; }
    }
}
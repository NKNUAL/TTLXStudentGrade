using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTLXStudentGrade.Models
{
    public class RegisterModel
    {
        public string UserName { get; set; }
        public string SchoolNo { get; set; }
        public string IDCard { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string SpecialtyId { get; set; }
        public string QQ { get; set; }
    }
}
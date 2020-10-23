using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTLXStudentGrade.Models
{
    public class LoginModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string pwd { get; set; }

    }
}
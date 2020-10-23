using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTLXStudentGrade.Models.Enum
{
    public enum UserRole
    {
        None = -1,
        Admin = 0,
        Student = 1,
        SchoolAdmin = 3,
        ReviewTeacher = 4,
    }
}
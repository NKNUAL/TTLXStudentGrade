using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTLXStudentGrade.Models
{
    public class GradeQueryModel
    {
        public string SchoolId { get; set; }
        public int SpecialtyId { get; set; }
        /// <summary>
        /// 由于数据中并没有维护ClassCode字段，所以这里的ClassCode也就是ClassName
        /// </summary>
        public string ClassCode { get; set; }
        public string PaperId { get; set; }
    }
}
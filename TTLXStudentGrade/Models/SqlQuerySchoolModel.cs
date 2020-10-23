using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTLXStudentGrade.Models
{
    public class SqlQuerySchoolModel
    {
        public string SchoolNo { get; set; }
        public string SchoolName { get; set; }
        public string AreaNo { get; set; }
        public string AreaName { get; set; }
    }


    public class QuerySpecialtyModel
    {
        public int? SpecialtyType { get; set; }
        public string SpecialtyName { get; set; }
    }

    public class QueryPlanModel
    {
        public int? PlanId { get; set; }
        public string PlanName { get; set; }
    }

    public class QueryModel
    {
        public string KsjhName { get; set; }
        public double PaperQueTotalScore { get; set; }
        public double StudentScore { get; set; }
        public string FK_School { get; set; }
        public string FK_SchoolID { get; set; }
    }

    public class QueryModel2
    {
        public long rownum { get; set; }
        public double StudentScore { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.ServiceModels
{
    public class StudentData
    {
        public string Kaohao { get; set; }
        public string UserName { get; set; }
        public string IDCard { get; set; }
        public string Pwd { get; set; }

    }

    public class StudentDataImport
    {
        public string Lexueid { get; set; }
        public string Kaohao { get; set; }
        public string UserName { get; set; }
        public string IDCard { get; set; }
        public string Pwd { get; set; }
        public string SpecailtyId { get; set; }
        public string SpecialtyName { get; set; }
        public string SchoolNo { get; set; }
        public string SchoolName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsHadUpload { get; set; }
        public string QQ { get; set; }

        public string RegisterDate { get; set; }
    }


    public class StudentRegisterResult
    {
        public int code { get; set; }

        public string message { get; set; }

        public StudentDataImport student { get; set; }
    }
}

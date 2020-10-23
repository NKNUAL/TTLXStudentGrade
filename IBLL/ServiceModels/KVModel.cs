using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.ServiceModels
{
    public class SchoolKVModel
    {
        public string SchoolNo { get; set; }

        public string SchoolName { get; set; }
    }


    public class SpecialtyKVModel
    {
        public string SpecialtyId { get; set; }

        public string SpecialtyName { get; set; }
    }

    public class AreaVKModel
    {
        public string AreaNo { get; set; }
        public string AreaName { get; set; }
    }

    public class AreaSchoolModel
    {
        public string AreaNo { get; set; }
        public List<SchoolKVModel> Schools { get; set; }
    }

}

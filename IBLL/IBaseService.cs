using IBLL.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    public interface IBaseService : IDependency
    {
        /// <summary>
        /// 获取省份的行政区
        /// </summary>
        /// <returns></returns>
        List<AreaVKModel> GetAreas(string provinceNo);

        /// <summary>
        /// 通过行政区的所有学校
        /// </summary>
        /// <param name="areaNo"></param>
        /// <returns></returns>
        List<SchoolKVModel> GetSchoolByArea(string areaNo);

        List<AreaSchoolModel> GetAreaSchools();

        List<SchoolKVModel> GetSchools(string schoolNo = null);

        List<SpecialtyKVModel> GetSpecialty();
    }
}

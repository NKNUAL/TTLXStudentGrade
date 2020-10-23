using IBLL.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    public interface IPhoneService : IDependency
    {

        List<PhoneLimitModel> GetSchoolLimitCount(string schoolNo);


        bool UpdateSchoolLimit(string schoolNo, string specialtyId, int limitCount);


        int GetSchoolLimit(string schoolNo, string specialtyId);

        int GetSchoolHasUploadCount(string schoolNo, int specialtyId);

        bool AuthVerify(string schoolNo, string gpCode);


        ResultModel UploadStudent(UploadUserServiceModel uploadUser);


        List<PhoneUserModel> GetPhoneUser(string schoolNo, string specialtyCode, string studentName);


        ResultModel UserEditInfo(string lexueid, string password, string userName);
    }
}

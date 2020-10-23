using IBLL.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBLL
{
    public interface IGradeService : IDependency
    {


        IDictionary<string, string> GetClassBySpecialty(string schoolId, int specialtyId);

        List<GradeModel> GetUserGrade(GradeQueryModel query);

        IDictionary<int, string> GetPapers(string schoolId, int specialtyId);

        bool OutputExcel(List<GradeModel> grades, string filepath);

        string GetPaperNameById(int paperId, int specialtyId);

        List<StudentGradeModel> GetStudentGradeDetails(string lexueid, string paperId);
    }
}
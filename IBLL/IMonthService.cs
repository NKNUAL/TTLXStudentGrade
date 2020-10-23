using IBLL.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    public interface IMonthService : IDependency
    {

        /// <summary>
        /// 查询参加考试的学校
        /// </summary>
        /// <param name="schoolName">查询参加考试的学校中名字包含schoolName的</param>
        /// <returns></returns>
        TreeModel GetSchoolTree(int planId);

        /// <summary>
        /// 获取调考的所有专业
        /// </summary>
        /// <returns></returns>
        Dictionary<int, string> GetMonthSpecialty();

        /// <summary>
        /// 获取某专业下所有考试计划
        /// </summary>
        /// <param name="specialtyType"></param>
        /// <returns></returns>
        Dictionary<int, string> GetPlanBySpecialty(int specialtyType);

        /// <summary>
        /// 获取全部参加考试的学校
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        Dictionary<string, string> GetTotalExamSchool(int planId);

        /// <summary>
        /// 获取学校考试概况
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="schoolCode"></param>
        /// <returns></returns>
        List<StudentScoreModel> GetStudentScore(int planId, string schoolCode, ref SchoolGradeModel model);
        /// <summary>
        /// 获取全省学生考试分数
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="planId"></param>
        /// <returns></returns>
        List<StudentScoreModel> GetStudentScore(int? page, int? rows, int planId, out int total);

        /// <summary>
        /// 获取学校考试的总体信息
        /// </summary>
        /// <returns></returns>
        SchoolResultModel GetSchoolBaseMessage(int planId, string schoolCode);

        List<SchoolScoreModel> GetProvinceBaseMessage(int planId);

        /// <summary>
        /// 获取学校比较数据
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="schoolCodes"></param>
        /// <returns></returns>
        List<SchoolCompareModel> GetSchoolCompareData(int planId, List<string> schoolCodes);

        /// <summary>
        /// 获取试卷答题情况
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        List<QuestionDetailModel> GetPaperQuestionDetail(int planId);


        /// <summary>
        /// 获取学生答题情况
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="studentId"></param>
        /// <param name="specialtyId"></param>
        /// <returns></returns>
        List<StudentQueDetailModel> GetStuQueDetail(int planId, string studentId, int specialtyId);

        int GetSpecialtyIdByPlanId(int planId);
        string GetSpecialtyNameByPlanId(int planId);

        string GetRank(int planId, string studentId, string schoolCode);

        double GetTotalScore(int planId, string studentId);

        ImportExcelGradeModel GetSchoolExcel(int planId, string studentId);

        string GetPlanNameById(int planId);

        string GetSchoolName(string studentId);

        ImportProvinceDataModel GetProvinceExcel(int planId);

        List<StudentPaperData> GetStudentMonthExamData(int year);

        /// <summary>
        /// 获取当前学生所有考试的时间
        /// </summary>
        /// <returns></returns>
        List<string> GetStudentExamDate(string lexueid);


        StudentQueDetail GetStudentPaperDetail(int planId);

    }
}

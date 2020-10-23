using Application.Common;
using Application.Logger;
using IBLL;
using IBLL.Helper;
using IBLL.ServiceModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TTLXStudentGrade.Models;

namespace TTLXStudentGrade.Controllers
{
    [SysAuth(Roles = "0,1,3")]
    public class MonthController : Controller
    {
        public IMonthService _monthService { get; set; }
        public IUserService _userService { get; set; }
        public MonthController() { }
        public MonthController(IMonthService monthService, IUserService userService)
        {
            this._monthService = monthService;
            this._userService = userService;
        }

        // GET: Month
        [SysAuth(Roles = "0,3")]
        public ActionResult Index()
        {
            return View();
        }
        [SysAuth(Roles = "1")]
        public ActionResult Student()
        {
            ViewData["schoolName"] = CookieHelper.GetSchoolName() + "    " + CookieHelper.GetUserName();
            return View();
        }
        [SysAuth(Roles = "0,3")]
        public JsonResult GetSchoolTree(QueryTreeModel query)
        {
            try
            {
                TreeModel tree = _monthService.GetSchoolTree(query.PlanId);
                return Json(new List<TreeModel> { tree });
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[MonthController:GetMonthSpecialty]"
                }, Log4NetLevel.Error);
                return Json(null);
            }
        }
        [SysAuth(Roles = "0,3")]
        public JsonResult GetMonthSpecialty()
        {
            try
            {
                var dic = _monthService.GetMonthSpecialty();
                List<dynamic> data = new List<dynamic>();
                foreach (var item in dic)
                {
                    data.Add(new { SpecialtyType = item.Key, SpecialtyName = item.Value });
                }
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[MonthController:GetMonthSpecialty]"
                }, Log4NetLevel.Error);
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// 通过专业查询当前用户可以查看的考试计划
        /// </summary>
        /// <param name="specialtyType"></param>
        /// <returns></returns>
        [SysAuth(Roles = "0,1,3")]
        public JsonResult GetMonthPlan(int specialtyType)
        {
            try
            {
                var dic = _monthService.GetPlanBySpecialty(specialtyType);
                List<dynamic> data = new List<dynamic>();
                foreach (var item in dic)
                {
                    data.Add(new { PlanId = item.Key, PlanName = item.Value });
                }
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[MonthController:GetMonthPlan]"
                }, Log4NetLevel.Error);
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// 获取参加考试的学校
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        [SysAuth(Roles = "0,3")]
        public JsonResult GetJoinSchool(int planId)
        {
            try
            {
                var dic = _monthService.GetTotalExamSchool(planId);
                List<dynamic> data = new List<dynamic>();
                foreach (var item in dic)
                {
                    data.Add(new { SchoolCode = item.Key, SchoolName = item.Value });
                }
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[MonthController:GetJoinSchool]"
                }, Log4NetLevel.Error);
                return Json(new { success = false });
            }
        }

        [SysAuth(Roles = "0,3")]
        public JsonResult GetSchoolScore(int planId, string schoolCode)
        {
            try
            {
                int hashcode = _monthService.GetHashCode();
                var data = _monthService.GetSchoolBaseMessage(planId, schoolCode);
                return Json(new
                {
                    success = true, data = new
                    {
                        isComputerSpecialty = data.IsComputerSpecialty,
                        school = new
                        {
                            rows = data.schoolScore, total = 2
                        },
                        student = new
                        {
                            rows = data.studentScores, total = data.studentScores.Count
                        }
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[MonthController:GetSchoolScore]"
                }, Log4NetLevel.Error);

            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [SysAuth(Roles = "0,3")]
        public ActionResult GetProvinceScore(int? page, int? rows, int planId)
        {
            try
            {
                var data = _monthService.GetStudentScore(page, rows, planId, out int total);
                var serializer = new JavaScriptSerializer
                {
                    MaxJsonLength = int.MaxValue
                };//使用原生Json转换类
                var result = new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new { rows = data, total = total }),// serializer.Serialize(list),      //data为要序列化的LINQ对象
                    ContentType = "application/json"
                };

                return result;

            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[MonthController:GetProvinceScore]"
                }, Log4NetLevel.Error);

            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProvinceBaseScore(int planId)
        {
            try
            {
                var data = _monthService.GetProvinceBaseMessage(planId);
                return Json(new { rows = data, total = data.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[MonthController:GetProvinceBaseScore]"
                }, Log4NetLevel.Error);

            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);

        }

        [SysAuth(Roles = "0,3")]
        public JsonResult DataCompare(int planId, List<TreeArgs> args)
        {
            List<string> schoolCodes = args.Where(a => a.Level == 2).Select(a => a.id).ToList();
            try
            {
                var data = _monthService.GetSchoolCompareData(planId, schoolCodes);
                return Json(new { success = true, data = new { rows = data, total = data.Count } });
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[MonthController:DataCompare]"
                }, Log4NetLevel.Error);

            }
            return Json(new { success = false });
        }

        [SysAuth(Roles = "0,3")]
        public ActionResult OutputDataCompare(int planId, string argsJson)
        {
            var args = JsonConvert.DeserializeObject<List<TreeArgs>>(argsJson);
            List<string> schoolCodes = args.Where(a => a.Level == 2).Select(a => a.id).ToList();
            try
            {
                var user = CookieHelper.GetUserData();
                var data = _monthService.GetSchoolCompareData(planId, schoolCodes);
                string strTemp;
                if (user.UserRole == Application.Enum.UserRole.SchoolAdmin)
                {
                    strTemp = Server.MapPath($"/同级单位比较/{planId}/{user.SchoolCode}");
                }
                else
                {
                    strTemp = Server.MapPath($"/同级单位比较/{planId}/admin");
                }

                if (!Directory.Exists(strTemp))
                {
                    Directory.CreateDirectory(strTemp);
                }
                strTemp = Path.Combine(strTemp, string.Format("{0}_同级单位比较.xlsx", _monthService.GetPlanNameById(planId)));
                if (System.IO.File.Exists(strTemp))
                {
                    System.IO.File.Delete(strTemp);
                }
                if (ExcelHelper.Instance.DataCompareConvertToFile(data, strTemp))
                {
                    return File(new FileStream(strTemp, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), "application/vnd.ms-excel", Path.GetFileName(strTemp));
                }
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[MonthController:OutputDataCompare]"
                }, Log4NetLevel.Error);
            }
            return Json(false);
        }

        [SysAuth(Roles = "0,3")]
        public JsonResult GetQuestionDetail(int planId)
        {
            try
            {

                var data = _monthService.GetPaperQuestionDetail(planId);
                return Json(new { rows = data, total = data.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[MonthController:GetQuestionDetail]"
                }, Log4NetLevel.Error);

            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        [SysAuth(Roles = "0,1,3")]
        public JsonResult GetStudentQueDetail(int planId, string studentId)
        {
            try
            {
                if (string.IsNullOrEmpty(studentId))
                    studentId = CookieHelper.GetUserId();
                var data = _monthService.GetStuQueDetail(planId, studentId, _monthService.GetSpecialtyIdByPlanId(planId));
                return Json(new { rows = data, total = data.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[MonthController:GetStudentQueDetail]"
                }, Log4NetLevel.Error);

            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        [SysAuth(Roles = "1")]
        public JsonResult GetStudentSchoolRank(int planId, string studentId)
        {
            try
            {
                if (string.IsNullOrEmpty(studentId))
                    studentId = CookieHelper.GetUserId();
                string rank = _monthService.GetRank(planId, studentId, CookieHelper.GetSchoolCode());
                return Json(rank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[MonthController:GetStudentSchoolRank]"
                }, Log4NetLevel.Error);

            }
            return Json(null);
        }
        [SysAuth(Roles = "1")]
        public JsonResult GetStudentProvinceRank(int planId, string studentId)
        {
            try
            {
                if (string.IsNullOrEmpty(studentId))
                    studentId = CookieHelper.GetUserId();
                string rank = _monthService.GetRank(planId, studentId, "");
                return Json(rank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[MonthController:GetStudentProvinceRank]"
                }, Log4NetLevel.Error);

            }
            return Json(null);
        }
        [SysAuth(Roles = "1")]
        public double GetStudentTotalScore(int planId, string studentId)
        {
            try
            {
                if (string.IsNullOrEmpty(studentId))
                    studentId = CookieHelper.GetUserId();
                double rank = _monthService.GetTotalScore(planId, studentId);
                return rank;
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[MonthController:GetStudentTotalScore]"
                }, Log4NetLevel.Error);

            }
            return 0;
        }
        [SysAuth(Roles = "0,3")]
        public ActionResult OutputStudentGrade(int planId, string schoolCode)
        {

            string specialttyName = _monthService.GetSpecialtyNameByPlanId(planId);
            string strTemp = Server.MapPath($"/学生成绩/{planId}/{schoolCode}/{specialttyName}");
            if (!Directory.Exists(strTemp))
            {
                Directory.CreateDirectory(strTemp);
            }
            strTemp = Path.Combine(strTemp, string.Format("{0}_{1}_学生成绩.xlsx", _monthService.GetPlanNameById(planId), specialttyName));
            if (System.IO.File.Exists(strTemp))
            {
                return File(new FileStream(strTemp, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), "application/vnd.ms-excel", Path.GetFileName(strTemp));
            }

            var excel = _monthService.GetSchoolExcel(planId, schoolCode);

            if (MonthHelper.Instance.UserConvertToExcel(excel, strTemp))
            {
                return File(new FileStream(strTemp, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), "application/vnd.ms-excel", Path.GetFileName(strTemp));
            }
            return Json(false);
        }

        [SysAuth(Roles = "0,3")]
        public ActionResult OutputProvinceStudentGrade(int planId)
        {

            string strTemp = Server.MapPath($"/学生成绩/{planId}/湖北省");
            if (!Directory.Exists(strTemp))
            {
                Directory.CreateDirectory(strTemp);
            }
            strTemp = Path.Combine(strTemp, string.Format("{0}_学生成绩.xlsx", _monthService.GetPlanNameById(planId)));
            if (System.IO.File.Exists(strTemp))
            {
                return File(new FileStream(strTemp, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), "application/vnd.ms-excel", Path.GetFileName(strTemp));
            }

            var excel = _monthService.GetProvinceExcel(planId);

            if (MonthHelper.Instance.UserConvertToExcel(excel, strTemp))
            {
                return File(new FileStream(strTemp, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), "application/vnd.ms-excel", Path.GetFileName(strTemp));
            }
            return Json(false);
        }

        [SysAuth(Roles = "0,3")]
        public ActionResult DownloadStudent(QueryStudentModel model)
        {
            if (model == null)
            {
                return Json("下载失败");
            }

            var userdata = CookieHelper.GetUserData();
            if (userdata.UserRole == Application.Enum.UserRole.SchoolAdmin)
            {
                model.SchoolNo = userdata.SchoolCode;
            }
            string strTemp = System.Web.Hosting.HostingEnvironment.MapPath($"/{model.SchoolNo}/学生信息");
            if (!Directory.Exists(strTemp))
            {
                Directory.CreateDirectory(strTemp);
            }
            strTemp = Path.Combine(strTemp, "学生信息.xlsx");
            var result = _userService.DownloadStudent(model.SchoolNo, model.SpecialtyId, strTemp);

            if (result.code == 1)
                return File(new FileStream(strTemp, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), "application/vnd.ms-excel", Path.GetFileName(strTemp));

            return Json("下载失败");
        }
        [SysAuth(Roles = "0,3")]
        public JsonResult GetStudentList(int? page, int? rows, QueryStudentModel model)
        {
            if (model == null)
            {
                return Json(new { success = false, message = "参数不合法" }, JsonRequestBehavior.AllowGet);
            }

            var userdata = CookieHelper.GetUserData();
            if (userdata.UserRole == Application.Enum.UserRole.SchoolAdmin)
            {
                model.SchoolNo = userdata.SchoolCode;
            }

            var result = _userService.GetStudentList(page, rows, model.SchoolNo, model.SpecialtyId, model.StudentName);
            if (result.code == 1)
            {
                return Json(result.data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = result.message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
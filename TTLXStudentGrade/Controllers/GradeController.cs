using Application.Common;
using Application.Logger;
using IBLL;
using IBLL.ServiceModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTLXStudentGrade.Controllers
{
    [SysAuth(Roles = "3")]
    public class GradeController : Controller
    {


        public IGradeService _gradeService { get; set; }
        public GradeController() { }
        public GradeController(IGradeService gradeService)
        {
            this._gradeService = gradeService;
        }

        // GET: Grade
        public ActionResult Index(int specialtyId)
        {
            ViewData["specialty_id"] = specialtyId;
            return View();
        }

        public JsonResult GetClassBySpecialty(int specialtyId)
        {
            try
            {

                var result = _gradeService.GetClassBySpecialty(CookieHelper.GetSchoolCode(), specialtyId);
                List<dynamic> classMessage = new List<dynamic>();
                classMessage.Add(new { classCode = "total", className = "全部班级" });
                foreach (var item in result)
                {
                    classMessage.Add(new { classCode = item.Key, className = item.Value });
                }
                return Json(new { success = true, data = classMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[Grade:GetClassBySpecialty]"
                }, Log4NetLevel.Error);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetPapers(int specialtyId)
        {

            try
            {
                var result = _gradeService.GetPapers(CookieHelper.GetSchoolCode(), specialtyId);
                List<dynamic> paperMessage = new List<dynamic>();
                foreach (var item in result)
                {
                    paperMessage.Add(new { paperId = item.Key, paperName = item.Value });
                }
                return Json(new { success = true, data = paperMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[Grade:GetPapers]"
                }, Log4NetLevel.Error);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetGrade(GradeQueryModel query)
        {
            try
            {
                query.SchoolId = CookieHelper.GetSchoolCode();
                var result = _gradeService.GetUserGrade(query);
                return Json(new { success = true, data = new { rows = result, total = result.Count } });
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[Grade:GetGrade]"
                }, Log4NetLevel.Error);
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// 学生信息导出
        /// </summary>
        /// <param name="specialtyCode"></param>
        /// <returns></returns>
        public ActionResult SaveExcel(GradeQueryModel query)
        {
            query.SchoolId = CookieHelper.GetSchoolCode();
            var result = _gradeService.GetUserGrade(query);
            string strTemp = Server.MapPath($"/学生成绩/{query.SchoolId}/{query.SpecialtyId}");
            if (!Directory.Exists(strTemp))
            {
                Directory.CreateDirectory(strTemp);
            }
            strTemp = Path.Combine(strTemp, string.Format("{0}_学生成绩.xlsx", _gradeService.GetPaperNameById(Convert.ToInt32(query.PaperId), query.SpecialtyId)));
            if (_gradeService.OutputExcel(result, strTemp))
            {
                return File(new FileStream(strTemp, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), "application/vnd.ms-excel", Path.GetFileName(strTemp));
            }
            return Json(false);
        }

        public JsonResult GetStudentGradeDetails(string paperId, string lexueid)
        {
            try
            {
                var result = _gradeService.GetStudentGradeDetails(lexueid, paperId);
                return Json(new { success = true, data = new { rows = result, total = result.Count } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = "[Grade:GetStudentGradeDetails]"
                }, Log4NetLevel.Error);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
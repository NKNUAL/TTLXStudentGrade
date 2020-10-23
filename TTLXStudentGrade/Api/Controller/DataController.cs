using Application.Common;
using Application.Logger;
using IBLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Results;
using TTLXStudentGrade.Api.Models;

namespace TTLXStudentGrade.Api.Controller
{


    [RoutePrefix("api/data")]
    public class DataController : BaseApiControler
    {

        public DataController(IUserService userService, IMonthService monthService, IBaseService baseService)
            : base(userService, monthService, baseService)
        {

        }


        [Route("basereg")]
        [HttpGet]
        public HttpResultModel GetBaseRegister()
        {
            try
            {
                var areas = _baseService.GetAreas("17");
                var specialties = _baseService.GetSpecialty();
                var schools = _baseService.GetAreaSchools();

                if (areas == null || specialties == null || schools == null)
                {
                    return new HttpResultModel
                    {
                        success = false,
                        message = "获取信息数据出错",
                    };
                }
                else
                {
                    return new HttpResultModel
                    {
                        success = true,
                        data = new { areas, specialties, schools }
                    };
                }
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = $"[{MethodBase.GetCurrentMethod().DeclaringType.Name}:{MethodBase.GetCurrentMethod().Name}]"
                }, Log4NetLevel.Error);
                return new HttpResultModel
                {
                    success = false,
                    message = "获取信息数据出错",
                };
            }

        }

        [ApiAuth(Roles = "1")]
        [Route("{year:int}")]
        public HttpResultModel GetStudentExamPaper(int year)
        {
            var data = _monthService.GetStudentMonthExamData(year);
            if (data == null)
            {
                return new HttpResultModel
                {
                    success = false,
                    message = "获取信息数据出错",
                };
            }
            else
            {
                return new HttpResultModel
                {
                    success = true,
                    message = "成功",
                    data = data
                };
            }
        }

        [ApiAuth(Roles = "1")]
        [Route("detail/{paperId}")]
        public HttpResultModel GetStudentQueDetail(int paperId)
        {
            var data = _monthService.GetStudentPaperDetail(paperId);
            if (data == null)
            {
                return new HttpResultModel
                {
                    success = false,
                    message = "获取信息数据出错",
                };
            }
            else
            {
                return new HttpResultModel
                {
                    success = true,
                    message = "成功",
                    data = data
                };
            }
        }

        [ApiAuth(Roles = "0,3")]
        [Route("studentlist")]
        [HttpPost]
        public HttpResultModel GetStudentList(int? page, int? rows, QueryStudentModel model)
        {
            if (model == null)
            {
                return new HttpResultModel { success = false, message = "参数不合法" };
            }

            var userdata = CookieHelper.GetUserData();
            if (userdata.UserRole == Application.Enum.UserRole.SchoolAdmin)
            {
                model.SchoolNo = userdata.SchoolCode;
            }

            var result = _userService.GetStudentList(page, rows, model.SchoolNo, model.SpecialtyId, model.StudentName);
            if (result.code == 1)
            {
                return new HttpResultModel { success = true, data = result.data };
            }
            else
            {
                return new HttpResultModel { success = false, message = result.message };
            }
        }

        //[ApiAuth(Roles = "0,3")]
        //[Route("schools")]
        //[HttpGet]
        //public JsonResult<HttpResultModel> GetSchools()
        //{
        //    try
        //    {
        //        var userdata = CookieHelper.GetUserData();
        //        if (userdata.UserRole == Application.Enum.UserRole.SchoolAdmin)
        //            return Json(new HttpResultModel { success = true, data = _baseService.GetSchools(userdata.SchoolCode) });
        //        return Json(new HttpResultModel { success = true, data = _baseService.GetSchools() });
        //    }
        //    catch (Exception ex)
        //    {
        //        LogContent.Instance.WriteLog(new AppOpLog()
        //        {
        //            LogMessage = ex.Message,
        //            MemberID = CookieHelper.GetUserId(),
        //            MethodName = $"[{MethodBase.GetCurrentMethod().DeclaringType.Name}:{MethodBase.GetCurrentMethod().Name}]"
        //        }, Log4NetLevel.Error);
        //        return Json(new HttpResultModel { success = false, message = "获取学校失败" });
        //    }

        //}
        [ApiAuth(Roles = "0,3")]
        [Route("schools")]
        [HttpGet]
        public HttpResultModel GetSchools()
        {
            try
            {
                var userdata = CookieHelper.GetUserData();
                if (userdata.UserRole == Application.Enum.UserRole.SchoolAdmin)
                    return new HttpResultModel { success = true, data = _baseService.GetSchools(userdata.SchoolCode) };
                return new HttpResultModel { success = true, data = _baseService.GetSchools() };
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = $"[{MethodBase.GetCurrentMethod().DeclaringType.Name}:{MethodBase.GetCurrentMethod().Name}]"
                }, Log4NetLevel.Error);
                return new HttpResultModel { success = false, message = "获取学校失败" };
            }

        }
        [ApiAuth(Roles = "0,3")]
        [Route("specialty")]
        [HttpGet]
        public HttpResultModel GetSpecialty()
        {
            try
            {
                var sp = _baseService.GetSpecialty();
                sp.Add(new IBLL.ServiceModels.SpecialtyKVModel
                {
                    SpecialtyId = "",
                    SpecialtyName = "全部"
                });
                return new HttpResultModel { success = true, data = sp };
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = $"[{MethodBase.GetCurrentMethod().DeclaringType.Name}:{MethodBase.GetCurrentMethod().Name}]"
                }, Log4NetLevel.Error);
                return new HttpResultModel { success = false, message = "获取专业失败" };
            }
        }

        [ApiAuth(Roles = "0,3")]
        [Route("download/student")]
        [HttpPost]
        public IHttpActionResult DownloadStudent(QueryStudentModel model)
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
            string strTemp = System.Web.Hosting.HostingEnvironment.MapPath($"学生信息/{model.SchoolNo}");
            if (!Directory.Exists(strTemp))
            {
                Directory.CreateDirectory(strTemp);
            }
            strTemp = Path.Combine(strTemp, "学生信息.xlsx");
            var result = _userService.DownloadStudent(model.SchoolNo, model.SpecialtyId, strTemp);

            if (result.code == 1)
                return new FileStreamResult(new FileStream(strTemp, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), "application/vnd.ms-excel", Path.GetFileName(strTemp));

            return Json("下载失败");
        }
    }
}

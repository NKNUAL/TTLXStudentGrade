using Application.Common;
using Application.Logger;
using IBLL;
using IBLL.ServiceModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Results;
using TTLXStudentGrade.Api.Models;

namespace TTLXStudentGrade.Api.Controller
{

    [RoutePrefix("api/user")]
    public class UserController : BaseApiControler
    {

        public UserController(IUserService userService, IMonthService monthService)
            : base(userService, monthService)
        {
        }
        public UserController()
            : base()
        {
        }

        public string Get()
        {
            return "success";
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        public JsonResult<HttpResultModel> Login(LoginModel login)
        {

            if (string.IsNullOrEmpty(login.UserId) || string.IsNullOrEmpty(login.pwd))
            {
                return Json(new HttpResultModel
                {
                    success = false,
                    message = "用户名和密码不能为空"
                });
            }
            else
            {
                LoginResult result = _userService.Login(login.UserId, login.pwd);
                if (result.code == 1)
                {
                    result.data.Guid = Guid.NewGuid().ToString().Replace("-", "");
                    CookieHelper.SetUserDataCookie(result.data);
                    string token = TokenHelper.GenerateToken(result.data.Lexueid, DateTime.Now, result.data.Guid);
                    CookieHelper.SetCookie("x-ttlx-token", token);
                    if (result.data.UserRole == Application.Enum.UserRole.Student)
                    {
                        return Json(new HttpResultModel
                        {
                            success = true,
                            message = "成功",
                            data = new
                            {
                                url = "http://" + Url.Request.Headers.Host + "/Static/view/home/student.html",
                                userData = new
                                {
                                    years = _monthService.GetStudentExamDate(result.data.Lexueid),
                                    userName = result.data.UserName,
                                    userSpecialtyName = result.data.SpecialtyName
                                }
                            }
                        });
                    }
                    else
                    {
                        return Json(new HttpResultModel
                        {
                            success = true,
                            message = "成功",
                            data = new { url = "http://" + Url.Request.Headers.Host + "/Home/Index" }
                        });
                    }
                }
                else
                {
                    return Json(new HttpResultModel
                    {
                        success = false,
                        message = result.message
                    });
                }
            }
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("reg")]
        [HttpPost]
        public JsonResult<HttpResultModel> Register(RegisterModel model)
        {

            string endTime = ConfigurationManager.AppSettings["endDate"];

            if (!string.IsNullOrEmpty(endTime))
            {
                DateTime endDate = DateTime.Parse(endTime);
                if (DateTime.Now > endDate)
                {
                    return Json(new HttpResultModel
                    {
                        success = false,
                        message = "已经过了报名截止时间！",
                    });
                }
            }

            try
            {
                var result = _userService.StudentRegister(model.SchoolNo, model.UserName, model.IDCard, model.PhoneNumber, model.Password, model.SpecialtyId, model.QQ);

                if (result.code == 1)
                {
                    return Json(new HttpResultModel
                    {
                        success = true,
                        message = result.message,
                        data = result.student
                    });
                }
                else
                {
                    return Json(new HttpResultModel
                    {
                        success = false,
                        message = result.message
                    });
                }
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = "用户注册",
                    MethodName = $"[{MethodBase.GetCurrentMethod().DeclaringType.Name}:{MethodBase.GetCurrentMethod().Name}]"
                }, Log4NetLevel.Error);

                return Json(new HttpResultModel
                {
                    success = false,
                    message = "服务器暂忙，请稍后再试"
                });
            }
        }

        /// <summary>
        /// 用户绑定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("bind")]
        [HttpPost]
        public JsonResult<HttpResultModel> Binding(BindModel model)
        {

            string endTime = ConfigurationManager.AppSettings["endDate"];

            if (!string.IsNullOrEmpty(endTime))
            {
                DateTime endDate = DateTime.Parse(endTime);
                if (DateTime.Now > endDate)
                {
                    return Json(new HttpResultModel
                    {
                        success = false,
                        message = "已经过了信息绑定截止时间！",
                    });
                }
            }

            try
            {
                var result = _userService.UserBind(model.Kaohao, model.UserName, model.Pwd, model.IDCard, model.PhoneNumber, model.QQ);
                return Json(new HttpResultModel
                {
                    success = result.code == 1,
                    message = result.message
                });

            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = "用户绑定",
                    MethodName = $"[{MethodBase.GetCurrentMethod().DeclaringType.Name}:{MethodBase.GetCurrentMethod().Name}]"
                }, Log4NetLevel.Error);

                return Json(new HttpResultModel
                {
                    success = false,
                    message = "服务器暂忙，请稍后再试"
                });
            }
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("pwd")]
        [HttpPost]
        public JsonResult<HttpResultModel> GetPwd(FindPwdModel model)
        {
            var result = _userService.FindPwd(model.IDCard, model.UserName);

            if (result.code == 0)
            {
                return Json(new HttpResultModel
                {
                    success = false,
                    message = result.message,
                });
            }
            else
            {
                return Json(new HttpResultModel
                {
                    success = true,
                    message = "密码找回成功",
                    data = result.student
                });
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [Route("out")]
        [HttpGet]
        public JsonResult<HttpResultModel> Loginout()
        {
            CookieHelper.RemoveCookie();
            return Json(new HttpResultModel
            {
                success = true,
                data = "http://" + Url.Request.Headers.Host + "/Static/view/login/login.html",
            });
        }

    }
}

using Application.Common;
using IBLL;
using IBLL.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TTLXStudentGrade.Models;

namespace TTLXStudentGrade.Controllers
{
    public class LoginController : Controller
    {

        public IUserService _userService { get; set; }
        public LoginController() { }
        public LoginController(IUserService userService)
        {
            this._userService = userService;
        }
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginModel login, string ReturnUrl)
        {

            if (string.IsNullOrEmpty(login.UserId) || string.IsNullOrEmpty(login.pwd))
            {
                ModelState.AddModelError("", "用户名和密码不能为空");

            }
            else
            {

                ResultModel result = _userService.Login(u => (u.lexueid == login.UserId || u.KaoHao == login.UserId || u.IDcard == login.UserId) && u.UserPassword == login.pwd);
                if (result.code == 1)
                {
                    CookieHelper.SetUserDataCookie((UserData)result.data);
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", result.message);
                }
            }
            return View();
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOff()
        {
            FormsAuthentication.SignOut();
            return Redirect(Url.Content("~/Static/view/login/login.html"));
            //return RedirectToAction("Login", "Login");
        }
    }
}
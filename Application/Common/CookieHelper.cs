using Application.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Application.Common
{
    public class CookieHelper
    {
        public static void SetUserDataCookie(UserData data)
        {
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            //数据放入ticket
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, data.UserName, DateTime.Now, DateTime.Now.AddMinutes(60), false, jsonData);
            //数据加密
            string enyTicket = FormsAuthentication.Encrypt(ticket);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, enyTicket);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }


        public static void SetCookie(string key, string value)
        {
            HttpCookie cookie = new HttpCookie(key, value);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }


        public static void RemoveCookie()
        {
            FormsAuthentication.SignOut();
        }


        public static string GetUserName()
        {
            if (IsLogin())
            {
                string strUserData = ((FormsIdentity)(HttpContext.Current.User.Identity)).Ticket.UserData;
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<UserData>(strUserData);
                return data.UserName;
            }
            return "";
        }
        public static string GetUserId()
        {
            if (IsLogin())
            {
                string strUserData = ((FormsIdentity)(HttpContext.Current.User.Identity)).Ticket.UserData;
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<UserData>(strUserData);
                return data.Lexueid;
            }
            return "";
        }
        public static UserRole GetRole()
        {
            if (IsLogin())
            {
                string strUserData = ((FormsIdentity)(HttpContext.Current.User.Identity)).Ticket.UserData;
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<UserData>(strUserData);
                return data.UserRole;
            }
            return UserRole.None;
        }


        public static string GetProvinceCode()
        {
            if (IsLogin())
            {
                string strUserData = ((FormsIdentity)(HttpContext.Current.User.Identity)).Ticket.UserData;
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<UserData>(strUserData);
                return data.ProvinceCode;
            }
            return "";
        }

        public static string GetSchoolCode()
        {
            if (IsLogin())
            {
                string strUserData = ((FormsIdentity)(HttpContext.Current.User.Identity)).Ticket.UserData;
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<UserData>(strUserData);
                return data.SchoolCode;
            }
            return "";
        }

        public static string GetSchoolName()
        {
            if (IsLogin())
            {
                string strUserData = ((FormsIdentity)(HttpContext.Current.User.Identity)).Ticket.UserData;
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<UserData>(strUserData);
                return data.SchoolName;
            }
            return "";
        }

        public static UserData GetUserData()
        {
            if (IsLogin())
            {
                string strUserData = ((FormsIdentity)(HttpContext.Current.User.Identity)).Ticket.UserData;
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<UserData>(strUserData);
                return data;
            }
            return null;
        }

        /// <summary>
        /// 判断用户是否登陆
        /// </summary>
        /// <returns>True,Fales</returns>
        public static bool IsLogin()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
    }

    public class UserData
    {
        public string Lexueid { get; set; }
        public string Kaohao { get; set; }
        public string UserName { get; set; }
        public int UserType { get; set; }
        public bool IsAdmin { get; set; }
        public UserRole UserRole { get; set; }
        public string ProvinceCode { get; set; }
        public string SchoolCode { get; set; }
        public string SchoolName { get; set; }
        public string SpecialtyId { get; set; }
        public string SpecialtyName { get; set; }
        public string Guid { get; set; }
        public string IDCard { get; set; }
    }
}

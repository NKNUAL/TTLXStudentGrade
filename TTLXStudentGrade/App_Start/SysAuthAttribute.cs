using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTLXStudentGrade
{
    public class SysAuthAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //如果没有设定角色，则只要登录就放行
            if (Roles == null)
            {
                if (!string.IsNullOrEmpty(CookieHelper.GetUserName()))
                    return true;
            }
            else
            {
                var role = CookieHelper.GetRole();
                foreach (var item in Roles.Split(','))
                {
                    int roleNum = (int)role;
                    if (item == roleNum.ToString())
                        return true;
                }
            }
            return base.AuthorizeCore(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}
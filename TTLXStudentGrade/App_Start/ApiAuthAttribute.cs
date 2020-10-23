using Application.Common;
using Application.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TTLXStudentGrade.Api.Models;

namespace TTLXStudentGrade
{
    public class ApiAuthAttribute : ActionFilterAttribute
    {
        public string Roles { get; set; }
        /// <summary>  
        /// 检查用户是否有该Action执行的操作权限  
        /// </summary>  
        /// <param name="actionContext"></param>  
        public override void OnActionExecuting(HttpActionContext actionContext)
        {


            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return;
            }
            bool bValid = actionContext.Request.Headers.TryGetValues("x-ttlx-token", out IEnumerable<string> token);
            if (bValid)
            {
                if (Application.Common.TokenHelper.ValidToken(token.FirstOrDefault()))
                {
                    if (!string.IsNullOrEmpty(Roles))
                    {
                        var role = CookieHelper.GetRole();

                        foreach (var item in Roles.Split(','))
                        {
                            int roleNum = (int)role;
                            if (item == roleNum.ToString())
                                return;
                        }
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new HttpResultModel
                        {
                            success = false,
                            message = "用户无权限"
                        });
                    }

                    return;
                }
            }
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new HttpResultModel
            {
                success = false,
                message = "用户未登陆"
            });
        }

    }
}
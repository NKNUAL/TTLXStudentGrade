using Application.Common;
using Application.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using TTLXStudentGrade.Api.Models;

namespace TTLXStudentGrade
{
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        //重写基类的异常处理方法
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {

            LogContent.Instance.WriteLog(new AppOpLog()
            {
                LogMessage = actionExecutedContext.Exception.Message + "——堆栈信息：" + actionExecutedContext.Exception.StackTrace,
                MemberID = CookieHelper.GetUserId(),
                MethodName = actionExecutedContext.Exception.GetType().ToString()
            }, Log4NetLevel.Error);


            //actionExecutedContext.Response = new HttpResultModel { success = false, message = "服务器错误" };

            base.OnException(actionExecutedContext);
        }
    }
}
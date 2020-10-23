using Application.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;


namespace TTLXStudentGrade.Api.Controller
{
    [RoutePrefix("api/test")]
    public class TestController : BaseApiControler
    {

        [Route("test")]
        [HttpGet]
        public string Get()
        {
            LogContent.Instance.WriteLog(new AppOpLog()
            {
                LogMessage = "测试",
                MemberID = "admin",
                MethodName = $"[test]"
            }, Log4NetLevel.Error);
            return "success";
        }

    }
}

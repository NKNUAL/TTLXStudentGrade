using Application.Logger;
using Autofac;
using Autofac.Integration.Mvc;
using FluentScheduler;
using IBLL.Helper;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TTLXStudentGrade.AutoMapping;

namespace TTLXStudentGrade
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = false;          
            
            AutofacConfig.Register(typeof(WebApiApplication));
            JobManager.Initialize(new QueueExeHelper());

            Log4NetConfig.ConfigureLog4Net();

            AutoMapperConfiguration.Configure();//注册映射
        }

    }
}

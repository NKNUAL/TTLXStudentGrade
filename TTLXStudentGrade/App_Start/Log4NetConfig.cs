using Application.Common;
using Application.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTLXStudentGrade
{
    public class Log4NetConfig
    {
        public static void ConfigureLog4Net()
        {
            log4net.Config.XmlConfigurator.Configure();
            log4net.Repository.Hierarchy.Hierarchy hierarchy = log4net.LogManager.GetRepository() as log4net.Repository.Hierarchy.Hierarchy;
            if (hierarchy != null && hierarchy.Configured)
            {
                foreach (log4net.Appender.IAppender appender in hierarchy.GetAppenders())
                {
                    if (appender is log4net.Appender.AdoNetAppender adoNetAppender)
                    {
                        adoNetAppender.ConnectionString = ConfigTools.GetDBConnString("dbUser");
                        adoNetAppender.ActivateOptions();
                    }
                }
            }
        }
    }
}
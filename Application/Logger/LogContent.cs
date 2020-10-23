using log4net;
using log4net.Layout;
using log4net.Layout.Pattern;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Application.Logger
{


    public class LogContent
    {
        private static ILog _log  = LogManager.GetLogger(typeof(LogContent));
        #region 单例
        
        private static readonly object padlock = new object();
        private static LogContent _logger;
        public static LogContent Instance
        {
            get
            {
                if (_logger == null)
                {
                    lock (padlock)
                    {
                        if (_logger == null)
                        {
                            _logger = new LogContent();
                        }
                    }
                }
                return _logger;
            }
        }

        private LogContent() { }
        #endregion

        /// <summary>
        /// 调用Log4net写日志
        /// </summary>
        /// <param name="logContent">日志内容</param>
        /// <param name="log4Level">记录日志等级，枚举</param>
        public void WriteLog(AppOpLog logContent, Log4NetLevel log4Level)
        {
            switch (log4Level)
            {
                case Log4NetLevel.Warn:
                    _log.Warn(logContent);
                    break;
                case Log4NetLevel.Debug:
                    _log.Debug(logContent);
                    break;
                case Log4NetLevel.Info:
                    _log.Info(logContent);
                    break;
                case Log4NetLevel.Fatal:
                    _log.Fatal(logContent);
                    break;
                case Log4NetLevel.Error:
                    _log.Error(logContent);
                    break;
            }
        }
    }
    /// <summary>
    /// log4net 日志等级类型枚举
    /// </summary>
    public enum Log4NetLevel
    {
        [Description("警告信息")]
        Warn = 1,
        [Description("调试信息")]
        Debug = 2,
        [Description("一般信息")]
        Info = 3,
        [Description("严重错误")]
        Fatal = 4,
        [Description("错误日志")]
        Error = 5
    }

}
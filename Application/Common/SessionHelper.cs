using Application.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Application.Common
{
    public class SessionHelper
    {
        public static object Get(string Key)
        {
            return HttpContext.Current.Session[Key];
        }


        public static bool SetUserSession(string Key, UserData data)
        {
            try
            {

                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                jsonData = ConfigTools.DESEncrypt(jsonData);
                if (data == null && HttpContext.Current.Session[Key] != null)
                {
                    HttpContext.Current.Session.Remove(Key);
                }
                else if (HttpContext.Current.Session[Key] == null)
                {
                    HttpContext.Current.Session.Add(Key, jsonData);
                }
                else
                {
                    HttpContext.Current.Session[Key] = jsonData;
                }

                return true;
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = "设置session出错：" + ex.Message,
                    MemberID = Key,
                    MethodName = $"[{MethodBase.GetCurrentMethod().DeclaringType.Name}:{MethodBase.GetCurrentMethod().Name}]"
                }, Log4NetLevel.Error);
                return false;
            }
        }


    }
}

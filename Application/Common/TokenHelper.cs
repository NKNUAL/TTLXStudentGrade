using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class TokenHelper
    {

        public static string GenerateToken(string userId, DateTime time, string guid)
        {
            string token = userId + "#" + time.ToString() + "#" + guid;
            return ConfigTools.DESEncrypt(token);
        }


        public static bool ValidToken(string token)
        {

            if (string.IsNullOrEmpty(token)) return false;
            //判断登录
            if (!CookieHelper.IsLogin()) return false;
            try
            {
                token = ConfigTools.DESDecrypt(token);
            }
            catch
            {
                return false;
            }

            string[] arrToken = token.Split('#');
            if (arrToken == null || arrToken.Length != 3) return false;

            var userData = CookieHelper.GetUserData();
            if (arrToken[2] != userData.Guid) return false;

            if (arrToken[0] != userData.Lexueid) return false;

            if (!DateTime.TryParse(arrToken[1], out DateTime time)) return false;

            return (DateTime.Now - time).TotalMinutes <= 60;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Application.Common
{
    public class RandomString
    {
        char[] _chars = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
                                 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
                                 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
                                 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f',
                                 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
                                 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
                                 'w', 'x', 'y', 'z','0','1','2','3','4',
                                 '5','6','7','8','9'};

        public string GenerateRandowString(int length)
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(_chars[random.Next(0, _chars.Length - 1)]);
            }
            return sb.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class StringEx
    {

        public static string TrimEx(this string value)
        {
            return value.Trim('\r', '\n');
        }
    }
}

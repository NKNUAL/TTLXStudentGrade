using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.ServiceModels
{
    public class LoginResult
    {
        public int code { get; set; }

        public string message { get; set; }

        public UserData data { get; set; }
    }
}

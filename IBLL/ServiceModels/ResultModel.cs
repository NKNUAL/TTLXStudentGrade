﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.ServiceModels
{
    public class ResultModel
    {
        public int code { get; set; }

        public string message { get; set; }

        public dynamic data { get; set; }
    }
}

using Application.Logger;
using IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace TTLXStudentGrade.Api.Controller
{

    public class BaseApiControler : ApiController
    {
        public IUserService _userService { get; set; }
        public IMonthService _monthService { get; set; }
        public IBaseService _baseService { get; set; }
        public IPhoneService _phoneService { get; set; }
        public BaseApiControler(IUserService userService)
        {
            _userService = userService;
        }

        public BaseApiControler(IUserService userService, IMonthService monthService)
        {
            _userService = userService;
            _monthService = monthService;
        }

        public BaseApiControler(IUserService userService, IMonthService monthService, IBaseService baseService)
        {
            _userService = userService;
            _monthService = monthService;
            _baseService = baseService;
        }

        public BaseApiControler() { }

        public BaseApiControler(IMonthService monthService)
        {
            _monthService = monthService;
        }


        public BaseApiControler(IPhoneService phoneService)
        {
            _phoneService = phoneService;
        }


        
    }
}
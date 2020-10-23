using IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTLXStudentGrade
{
    [SysAuth(Roles = "0,1,3")]
    public class HomeController : Controller
    {
        public IHomeService _homeService { get; set; }
        public HomeController() { }
        public HomeController(IHomeService homeService)
        {
            this._homeService = homeService;
        }


        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetModules()
        {
            if (Request.IsAuthenticated)
            {
                return Json(_homeService.GetMenuModule());
            }
            return null;
        }
    }
}
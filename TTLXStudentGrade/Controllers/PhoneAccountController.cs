using IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTLXStudentGrade.Controllers
{
    [SysAuth(Roles = "0")]
    public class PhoneAccountController : Controller
    {
        public IBaseService _baseService { get; set; }
        public IPhoneService _phoneService { get; set; }
        public PhoneAccountController(IBaseService baseService, IPhoneService phoneService)
        {
            _baseService = baseService; _phoneService = phoneService;
        }

        // GET: PhoneAccount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PhoneCount()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetSchools()
        {
            if (Request.IsAuthenticated)
            {
                var schools = _baseService.GetSchools();
                return Json(new { success = true, data = schools }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "未授权" }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetSpecialtyLimit(string schoolNo)
        {
            if (Request.IsAuthenticated)
            {
                var limits = _phoneService.GetSchoolLimitCount(schoolNo);
                return Json(new { success = true, data = limits }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "未授权" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateSchoolLimit(string schoolNo, string specialtyId, int limitCount)
        {
            if (Request.IsAuthenticated)
            {
                if (limitCount < 0)
                {
                    return Json(new { success = false, message = "数值不允许小于0" }, JsonRequestBehavior.AllowGet);
                }
                var limits = _phoneService.UpdateSchoolLimit(schoolNo, specialtyId, limitCount);
                return Json(new { success = true, data = limits }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "未授权" }, JsonRequestBehavior.AllowGet);
        }

    }
}
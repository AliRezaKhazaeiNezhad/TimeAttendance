using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        [AllowAnonymous]
        public ActionResult Index2()
        {
            return View();
        }


        public ActionResult Index()
        {
            ViewBag.MenuName = "home";
            ViewBag.PanelName = "portal";

            return View();
        }


        #region گزارشات مدیریتی
        public ActionResult MonthlyReport()
        {
            return PartialView("_MonthlyReport");
        }
        public ActionResult PendingRequests()
        {
            return PartialView("_PendingRequests");
        }
        public ActionResult ToadyReport()
        {
            return PartialView("_ToadyReport");
        }
        public ActionResult WeekReport()
        {
            return PartialView("_WeekReport");
        }
        #endregion
    }
}
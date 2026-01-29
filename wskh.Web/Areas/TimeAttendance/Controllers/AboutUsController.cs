using AutoMapper;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Model;
using wskh.Core;
using wskh.Model;
using wskh.Service;
using wskh.Web.Helper;
using wskh.WebEssentials.DataTablePart;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class AboutUsController : Controller
    {
        #region Propertices
        #endregion
        #region Ctor
        public AboutUsController()
        {
        }
        #endregion
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MenuName = "aboutus";

            return View();
        }
        #endregion
    }
}
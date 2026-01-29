using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class CommingSoonController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeAttendance.Web.Controllers
{
    public class ErrorPagesController : Controller
    {
        // GET: ErrorPages
        public ActionResult Error404()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}
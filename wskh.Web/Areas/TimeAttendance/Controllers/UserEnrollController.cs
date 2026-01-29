using System.Linq;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Model;
using TimeAttendance.Web.Helper;
using wskh.Core.Enumerator;
using wskh.Service;
using wskh.WebEssentials.DataTablePart;
using wskh.WebEssentials.DateAndTime;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class UserEnrollController : Controller
    {
        #region Ctor
        public UserEnrollController()
        {
        }
        #endregion
        #region Index
        public ActionResult Index()
        {
            ViewBag.MenuName = "userenroll";
            return View();
        }
        #endregion
      
    }
}
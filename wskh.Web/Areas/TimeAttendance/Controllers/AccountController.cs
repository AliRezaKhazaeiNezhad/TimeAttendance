using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAttendance.Model;
using TimeAttendance.Web.Helper;
using wskh.Service;
using wskh.Web;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Ctor
        private ApplicationUserManager _userManager;
        public AccountController()
        {

        }
        public AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion
        public ActionResult ChangePassword()
        {
            ViewBag.MenuName = "changepassword";

            ChangePassWordModel model = new ChangePassWordModel();
            model.Error = null;
            return View(model);
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePassWordModel model)
        {
            ViewBag.MenuName = "changepassword";

            try
            {
                if (ModelState.IsValid)
                {
                    var userService = DependencyResolver.Current.GetService<IUserService>();

                    var userId = UserHelper.CurrentUserId();
                    var _userService = DependencyResolver.Current.GetService<IUserService>();
                    var userList = _userService.List2();
                    var user = userList.FirstOrDefault(x => x.Id.Contains(userId));


                    var hashPassWord = UserManager.PasswordHasher.HashPassword(model.Password);
                    user.PasswordHash = hashPassWord;
                    _userService.Update(user);
                    return RedirectToAction("Success");
                }
            }
            catch (System.Exception)
            {
                model.Error = "خطایی رخ داده است!!! در فرصتی دیگر تلاش نمایید";
            }
            return View(model);
        }
        public ActionResult Success()
        {
            ViewBag.PanelName = "setting";
            ViewBag.MenuName = "changepassword";

            ChangePassWordModel model = new ChangePassWordModel();
            return View(model);
        }
    }
}
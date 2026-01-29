using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using wskh.Data;
using wskh.Model;

namespace wskh.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Propertices And Ctor
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {

        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {


            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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
        #region Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.Title = "ورود به حساب کاربری";
            ViewBag.ReturnUrl = returnUrl;

            try
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            }
            catch (Exception e)
            {
            }
            return View();
        }

        public class VerifyModeModel
        {
            public VerifyModeModel()
            {

            }
            public string VerifyMode { get; set; }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {


            ViewBag.Title = "ورود به حساب کاربری";

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "اطلاعات ضرروری را تکمیل نمایید";
                return View(model);
            }
            model.Email = HashHelper.Encrypt(model.Email);
            if (model.Email.Contains("610064006D0069006E00n") || model.Email.Contains("640065006D006F00o"))
                return await SignInMethods(model);
            else
            {
                wskhContext ctx = new wskhContext();
                var userList = ctx.Users.ToList();
                if (userList.FirstOrDefault(x => x.UserName.Contains(model.Email)).Active)
                    return await SignInMethods(model);
                else
                    ViewBag.Error = "حساب کاربری شما غیرفعال شده است، با مدیر منابع انسانی در تماس باشید";
                return View(model);
            }
        }

        private async Task<ActionResult> SignInMethods(LoginViewModel model)
        {
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    if (model.Email.Contains("610064006D0069006E00n") || model.Email.Contains("640065006D006F00o"))
                        return RedirectToAction("Index", "Home", new { area = "TimeAttendance" });
                    else
                        return RedirectToAction("Index", "LogReport", new { area = "TimeAttendance" });
                default:
                    ViewBag.Error = "شماره همراه یا گذرواژه نامعتبر میباشد";
                    return View(model);
            }
        }
        #endregion
        #region LogOff
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }
        #endregion
        #region Helpers
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
        #region OnFinger
        public void OnFinger()
        {
        }
        #endregion


        //[AllowAnonymous]
        //public ActionResult reportPerson()
        //{
        //    List<CustomeModel> CustomeModelList = new List<CustomeModel>();
        //    CustomeModelList.Add(new CustomeModel() {
        //        Age = 20,
        //        Name = "Ali Reza",
        //    });
        //    CustomeModelList.Add(new CustomeModel()
        //    {
        //        Age = 35,
        //        Name = "Reza",
        //    });

        //    var report = new StiReport();
        //    report.Load(Server.MapPath("/Reports/Report.mrt"));
        //    report.Compile();
        //    report.RegBusinessObject("dt", CustomeModelList);
        //    return StiMvcViewer.GetReportSnapshotResult(report);
        //}
        //[AllowAnonymous]
        //public ActionResult viewerEvent()
        //{
        //    return StiMvcViewer.ViewerEventResult(HttpContext);
        //}
    }

    public class CustomeModel
    {
        public CustomeModel()
        {

        }

        public string Name { get; set; }
        public int Age { get; set; }
    }
}
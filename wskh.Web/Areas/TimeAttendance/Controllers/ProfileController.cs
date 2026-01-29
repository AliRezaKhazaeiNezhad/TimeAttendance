using System;
using System.Linq;
using System.Web.Mvc;
using TimeAttendance.Web.Helper;
using wskh.Data;
using wskh.Model;
using wskh.Service;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    public class ProfileController : Controller
    {
        #region Propertices
        private IUserService _userService { get; set; }
        #endregion
        #region Ctor
        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion
        #region AddOrUpdate
        public ActionResult AddOrUpdate()
        {
            ViewBag.MenuName = "profile";

            UserInformationModel model = new UserInformationModel();
            EntityToModel(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(UserInformationModel model)
        {
            ViewBag.PanelName = "setting";
            ViewBag.MenuName = "profile";

            model.SuccessMessage = null;
            model.ErrorMessage = null;

            if (ModelState.IsValid)
            {
                try
                {
                    var entity = _userService.List2().FirstOrDefault(x => x.Id == model.Id);
                    entity.FirstName = model.FirstName;
                    entity.Lastname = model.Lastname;
                    entity.NationalCode = HashHelper.Encrypt(model.NationalCode);
                    _userService.Update(entity);
                    model.SuccessMessage = "ثبت با موفقیت انجلم شد";
                }
                catch (Exception e)
                {
                    model.ErrorMessage = "خطایی رخ داده است! در فرصتی دیگر تلاش نمایید.";
                }
            }
            else
                model.ErrorMessage = "اطلاعات ضروری را تکمیل نمایید";
            EntityToModel(model);
            return View(model);
        }
        #endregion
        #region PrepareMethod
        private void EntityToModel(UserInformationModel model)
        {
            string currentUserId = UserHelper.CurrentUserId();
            var entity = _userService.List2().FirstOrDefault(x => x.Id == currentUserId);
            model.Id = entity.Id;
            model.UserRoleType = entity.UserRoleType;
            model.SexId = entity.Sex == true ? 1 : 0;
            model.FirstName = string.IsNullOrEmpty(entity.FirstName) ? "-" : entity.FirstName;
            model.Lastname = string.IsNullOrEmpty(entity.Lastname) ? "-" : entity.Lastname;
            model.NationalCode = HashHelper.Decrypt(entity.NationalCode);
            model.EducationLevel = entity.EducationLevel == null || string.IsNullOrEmpty(entity.EducationLevel.Title) ? "-" : entity.EducationLevel.Title;
            model.EmployeType = entity.EmploymentType == null || string.IsNullOrEmpty(entity.EmploymentType.Title) ? "-" : entity.EmploymentType.Title;
            model.Branch = entity.OrganizationBranch == null || string.IsNullOrEmpty(entity.OrganizationBranch.Title) ? "-" : entity.OrganizationBranch.Title;
            model.OrganizationLevel = entity.OrganizationLevel == null || string.IsNullOrEmpty(entity.OrganizationLevel.Title) ? "-" : entity.OrganizationLevel.Title;
        }
        #endregion
    }
}
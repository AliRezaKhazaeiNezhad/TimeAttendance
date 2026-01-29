using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TimeAttendance.Web.Helper;
using TimeAttendance.WebEssentials.CommandPart;
using wskh.Core;
using wskh.Core.Enumerator;
using wskh.Data;
using wskh.FingerTec;
using wskh.Model;
using wskh.Service;
using wskh.Web;
using wskh.Web.Helper;
using wskh.WebEssentials.DataTablePart;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        #region Propertices
        private readonly IUserService _userService;
        private ApplicationUserManager _userManager;
        private readonly IUserGroupService _userGroupService;
        private readonly IEducationLevelService _EducationLevelService;
        private readonly IEmploymentTypeService _employmentTypeService;
        private readonly IOrganizationBranchService _organizationBranchService;
        private readonly IOrganizationLevelService _organizationLevelService;
        private readonly IEnrollService _enrollService;
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
        #region Ctor
        public UserController(
            IUserService userService,
            IUserGroupService userGroupService,
            IEducationLevelService EducationLevelService,
            IEmploymentTypeService employmentTypeService,
            IOrganizationBranchService organizationBranchService,
            IOrganizationLevelService organizationLevelService,
            IEnrollService enrollService
            )
        {
            _userService = userService;
            _userGroupService = userGroupService;
            _userGroupService = userGroupService;
            _EducationLevelService = EducationLevelService;
            _employmentTypeService = employmentTypeService;
            _organizationBranchService = organizationBranchService;
            _organizationLevelService = organizationLevelService;
            _enrollService = enrollService;
        }
        #endregion
        #region Index
        public ActionResult Index()
        {
            ViewBag.PanelName = "users";
            ViewBag.MenuName = "user";
            return View();
        }
        #endregion
        #region List
        [HttpGet]
        public ActionResult ListIndex()
        {
            return PartialView("_List");
        }
        [HttpGet]
        public JsonResult List(DataTableRequest request, [ModelBinder(typeof(DataTableModelBinder))]DataTableRequestFilter filter)
        {
            #region Grid configuration
            if (request.length == -1)
            {
                request.length = int.MaxValue;
            }
            var modelItem = new DataTableResponse<UserModel>();
            modelItem.draw = request.draw;
            var data = _userService.FilterData(request.start, request.length, filter.Search);
            modelItem.recordsTotal = _userService.Count(filter.Search);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new UserModel();
                model.Index = ++add;
                model.Id = x.Id;
                model.GroupName = x.UserGroup.Title;
                model.SexName = x.Sex == true ? "آقا" : "خانم";
                model.UserName = HashHelper.Decrypt(x.UserName);
                model.NationalCode = HashHelper.Decrypt(x.NationalCode);
                model.FirstName = $"{model.SexName} {x.FirstName} {x.Lastname}";
                model.Lastname = x.Lastname;
                model.Active = x.Active ? "فعال" : "<span style='color:red'>غیرفعال</span>";

                modelItem.data.Add(model);
            });
            #endregion

            return Json(modelItem, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add or update
        [HttpGet]
        public ActionResult AddOrUpdate(string id = null)
        {
            UserModel model = new UserModel();
            PrepareDrop(model);
            if (!string.IsNullOrEmpty(id))
            {
                wskhUser entity = _userService.GetList.FirstOrDefault(x => x.Id == id);
                model.Id = entity.Id;
                model.SexId = entity.Sex == true ? 1 : 0;
                model.FirstName = entity.FirstName;
                model.Lastname = entity.Lastname;
                model.NationalCode = HashHelper.Decrypt(entity.NationalCode);
                model.UserName = HashHelper.Decrypt(entity.UserName);
                model.UserGroupId = entity.UserGroupId.GetValueOrDefault();
                model.EducationLevelId = entity.EducationLevelId.GetValueOrDefault();
                model.EmployeTypeId = entity.EmploymentTypeId.GetValueOrDefault();
                model.BranchId = entity.OrganizationBranchId.GetValueOrDefault();
                model.OrganizationLevelId = entity.OrganizationLevelId.GetValueOrDefault();
                model.Password = "XXX";
            }
            return PartialView("_AddOrUpdate", model);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(UserModel model)
        {
            JsonModel jsonModel = new JsonModel();
            wskhUser entity = new wskhUser();

            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(model.Id) && model.Id.Length > 1)
                    {
                        entity = _userService.GetList.FirstOrDefault(x => x.Id == model.Id);
                        entity.Sex = model.SexId == 1 ? true : false;
                        entity.FirstName = model.FirstName;
                        entity.Lastname = model.Lastname;
                        entity.NationalCode = HashHelper.Encrypt(model.NationalCode);
                        entity.UserRoleType = model.UserRoleType;

                        entity.UserGroupId = model.UserGroupId;
                        entity.EducationLevelId = model.EducationLevelId;
                        entity.EmploymentTypeId = model.EmployeTypeId;
                        entity.OrganizationBranchId = model.BranchId;
                        entity.OrganizationLevelId = model.OrganizationLevelId;

                        //entity.UserName = HashHelper.Encrypt(model.UserName);
                        _userService.Update(entity);
                        jsonModel.Success();

                    }
                    else
                    {
                        entity.Sex = model.SexId == 1 ? true : false;
                        entity.UserName = HashHelper.Encrypt(model.UserName);
                        entity.FirstName = model.FirstName;
                        entity.Lastname = model.Lastname;
                        entity.NationalCode = HashHelper.Encrypt(model.NationalCode);
                        entity.UserRoleType = model.UserRoleType;
                        entity.Email = $"{entity.NationalCode}@{entity.NationalCode}{entity.UserRoleType}.com";
                        entity.UserGroupId = model.UserGroupId;
                        entity.EducationLevelId = model.EducationLevelId;
                        entity.EmploymentTypeId = model.EmployeTypeId;
                        entity.OrganizationBranchId = model.BranchId;
                        entity.OrganizationLevelId = model.OrganizationLevelId;
                        entity.Active = true;

                        var result = await UserManager.CreateAsync(entity, model.Password);
                        if (result.Succeeded)
                            jsonModel.Success();
                    }
                }
                catch (Exception e)
                {
                    jsonModel.Exception();
                }
            }

            model.Password = "XXX";
            PrepareDrop(model);
            jsonModel.Html = HtmlToJsonHelper.RenderPartialView(this, "_AddOrUpdate", model);
            return Json(jsonModel);
        }
        #endregion
        #region Delete
        public ActionResult Delete(string id = null)
        {
            bool result = false;
            try
            {
                wskhUser entity = _userService.GetList.FirstOrDefault(x => x.Id == id);
                _userService.Delete(entity);
                result = true;
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Number exist
        public ActionResult NumberExist(string mobileNumber)
        {
            bool result = false;
            try
            {
                var hashMobileNumber = HashHelper.Encrypt(mobileNumber);
                var userList = _userService.GetList;
                if (userList.Where(x => x.UserName.Contains(hashMobileNumber)).Count() > 0)
                    result = true;
            }
            catch (Exception e)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Reset password
        public async Task<ActionResult> ResetPassword(string id)
        {
            bool result = false;
            try
            {
                wskhUser userFind = await UserManager.FindByIdAsync(id);
                var pass = HashHelper.Decrypt(userFind.NationalCode);
                userFind.PasswordHash = UserManager.PasswordHasher.HashPassword(pass);
                var finalResult = UserManager.Update(userFind);
                if (finalResult.Succeeded)
                    result = true;
            }
            catch (Exception e)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Active Deactive
        public async Task<ActionResult> ActiveDeactive(string id)
        {
            bool result = false;
            try
            {
                wskhUser userFind = await UserManager.FindByIdAsync(id);
                userFind.Active = userFind.Active == true ? false : true;
                var finalResult = UserManager.Update(userFind);
                if (finalResult.Succeeded)
                    result = true;
            }
            catch (Exception e)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Deatil
        public async Task<ActionResult> Deatil(string id)
        {
            wskhUser userFind = await UserManager.FindByIdAsync(id);
            return View("_Detail", userFind);
        }
        #endregion
        #region AssignEnroll
        public ActionResult AssignEnroll(string id)
        {
            ViewBag.AssignUserId = id;
            return View("_AssignEnroll");
        }
        #endregion
        #region AssignEnrollToUser
        public ActionResult AssignEnrollToUser(string userId, int enroll)
        {
            bool result = false;
            try
            {
                var entity = _enrollService.FindById(enroll);
                if (!string.IsNullOrEmpty(entity.UserId))
                {
                    var userInformation = UserHelper.FullInformation(entity.UserId);
                    entity.UserId = null;
                    CommandHelper.Create(CommandCategory.RemoveEnrollFromUser, false, $"کاربر سخت افزار به شماره {entity.EnrollNo} از {userInformation} گرفته شد", 0, UserHelper.CurrentUser(), entity.Id);
                }
                else
                {
                    entity.UserId = userId;
                    var userInformation = UserHelper.FullInformation(entity.UserId);
                    CommandHelper.Create(CommandCategory.AssignEnrollToUser, false, $"کاربر سخت افزار به شماره {entity.EnrollNo} به {userInformation} انتساب داده شد", 0, UserHelper.CurrentUser(), entity.Id);
                }
                _enrollService.Update(entity);

                result = true;
            }
            catch (Exception e)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogToReport(int enroll)
        {
            bool result = false;
            try
            {
                var entity = _enrollService.FindById(enroll);
               
                _enrollService.Update(entity);

                result = true;
            }
            catch (Exception e)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region PrepareMethod
        public void PrepareDrop(UserModel model)
        {
            model.UserRoleType = HashHelper.Encrypt("enduser");

            var entites = _userGroupService.GetList;
            entites.ForEach(x => model.UserGroupList.Add(new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Title
            }));

            var EducationLevels = _EducationLevelService.GetList;
            EducationLevels.ForEach(x => model.EducationLevelList.Add(new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Title
            }));

            var employmentTypes = _employmentTypeService.GetList;
            employmentTypes.ForEach(x => model.EmployeTypeList.Add(new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Title
            }));

            var organizationBranch = _organizationBranchService.GetList;
            organizationBranch.ForEach(x => model.BranchList.Add(new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Title
            }));

            var organizationLevels = _organizationLevelService.GetList;
            organizationLevels.ForEach(x => model.OrganizationLevelList.Add(new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Title
            }));
        }
        #endregion
        #region Description
        public ActionResult Description()
        {
            return PartialView("_Description");
        }
        #endregion
    }
}
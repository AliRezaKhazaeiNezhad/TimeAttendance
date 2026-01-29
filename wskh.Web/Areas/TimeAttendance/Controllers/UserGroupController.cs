using AutoMapper;
using System;
using System.Linq;
using System.Web.Mvc;
using TimeAttendance.Core;
using wskh.Model;
using wskh.Service;
using wskh.Web.Helper;
using wskh.WebEssentials.DataTablePart;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class UserGroupController : Controller
    {
        #region Propertices
        private readonly IUserGroupService _userGroupService;
        private readonly ICalendarService _calendarService;
        private readonly IUserGroupCalendareService _userGroupCalendareService;
        #endregion
        #region Ctor
        public UserGroupController(IUserGroupService UserGroupService, ICalendarService calendarService, IUserGroupCalendareService userGroupCalendareService)
        {
            _userGroupService = UserGroupService;
            _calendarService = calendarService;
            _userGroupCalendareService = userGroupCalendareService;
        }
        #endregion
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PanelName = "users";
            ViewBag.MenuName = "usergroup";
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
            var modelItem = new DataTableResponse<UserGroupModel>();
            modelItem.draw = request.draw;
            var data = _userGroupService.FilterData(request.start, request.length, filter.Search);
            modelItem.recordsTotal = _userGroupService.Count(filter.Search);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new UserGroupModel();
                model.Index = ++add;
                model.Id = x.Id;
                model.Title = x.Title;

                modelItem.data.Add(model);
            });
            #endregion

            return Json(modelItem, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add or update
        [HttpGet]
        public ActionResult AddOrUpdate(int id = 0)
        {
            UserGroupModel model = new UserGroupModel();
            if (id > 0)
            {
                UserGroup entity = _userGroupService.FindById(id);
                model = Mapper.Map<UserGroup, UserGroupModel>(entity);
            }
            return PartialView("_AddOrUpdate", model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(UserGroupModel model)
        {
            JsonModel jsonModel = new JsonModel();
            if (ModelState.IsValid)
            {
                try
                {
                    UserGroup entity = new UserGroup();
                    if (model.Id.GetValueOrDefault() > 0)
                    {
                        entity = new UserGroup();
                        entity = Mapper.Map<UserGroupModel, UserGroup>(model);
                        _userGroupService.Update(entity);
                    }
                    else
                    {
                        entity = Mapper.Map<UserGroupModel, UserGroup>(model);
                        _userGroupService.Create(entity);
                    }
                    model = new UserGroupModel();
                    jsonModel.Success();
                }
                catch (Exception e)
                {
                    jsonModel.Exception();
                }
            }
            jsonModel.Html = HtmlToJsonHelper.RenderPartialView(this, "_AddOrUpdate", model);
            return Json(jsonModel);
        }
        #endregion
        #region Delete
        public ActionResult Delete(int id = 0)
        {
            int result = -1;
            try
            {
                UserGroup entity = _userGroupService.FindById(id);
                if (entity.Users != null && entity.Users.Count() > 0)
                    result = 1;
                else
                {
                    entity.Remove = true;
                    _userGroupService.Update(entity);
                    result = 0;
                }
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Description
        public ActionResult Description()
        {
            return PartialView("_Description");
        }
        #endregion


        #region Contract Part
        public ActionResult GetContracts(int id = 0)
        {
            var entity = _userGroupService.FindById(id);
            return PartialView("_GetContracts", entity);
        }

        public ActionResult ContractList(int id = 0)
        {
            var entity = _userGroupService.FindById(id);
            return PartialView("_ContractList", entity);
        }
        public ActionResult AddCalendare(int id = 0, int calendareId = 0)
        {
            int result = 1;
            try
            {
                UserGroup entity = _userGroupService.FindById(id);
                Calendar entityCalendare = _calendarService.FindById(calendareId);

                if (entity.UserGroupCalendares != null && entity.UserGroupCalendares.Count() > 0)
                {
                    if (entity.UserGroupCalendares.Where(x => x.Calendar.Year == entityCalendare.Year && x.Remove == false).Count() > 0)
                        result = 2;
                    else
                    {
                        entity.UserGroupCalendares.Add(new UserGroupCalendare() {
                            CalendarId = entityCalendare.Id,
                            UserGroupId = entity.Id
                        });
                        _userGroupService.Update(entity);
                        result = 0;
                    }
                }
                else
                {
                    entity.UserGroupCalendares.Add(new UserGroupCalendare()
                    {
                        CalendarId = entityCalendare.Id,
                        UserGroupId = entity.Id
                    });
                    _userGroupService.Update(entity);
                    result = 0;
                }

            }
            catch (Exception e)
            {
                result = -1;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteCalendare(int id = 0, int calendareId = 0)
        {
            bool result = false;
            try
            {
                var findCalendare = _userGroupCalendareService.GetList.FirstOrDefault(x => x.CalendarId == calendareId && x.UserGroupId == id && x.Remove == false);
                findCalendare.Remove = true;
                _userGroupCalendareService.Update(findCalendare);
                result = true;
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
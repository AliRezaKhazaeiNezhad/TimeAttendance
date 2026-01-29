using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Model;
using TimeAttendance.Web.Helper;
using TimeAttendance.WebEssentials.DateAndTime;
using TimeAttendance.WebEssentials.RequestPart;
using wskh.Core.Enumerator;
using wskh.Service;
using wskh.Web.Helper;
using wskh.WebEssentials.DataTablePart;
using wskh.WebEssentials.DateAndTime;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    public class MissionRequestController : Controller
    {
        #region Propertices
        private IRequestService _requestService { get; set; }
        #endregion


        #region Ctor

        public MissionRequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        #endregion


        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PanelName = "workdesk";
            ViewBag.MenuName = $"missionrequest";
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
            string userId = null;
            var CurrentUserRoleType = UserHelper.CurrentUser().UserRoleType;
            if (!CurrentUserRoleType.Contains("610064006D0069006E00n") && !CurrentUserRoleType.Contains("640065006D006F00o"))
                userId = UserHelper.CurrentUserId();


            if (request.length == -1)
            {
                request.length = int.MaxValue;
            }
            var modelItem = new DataTableResponse<RequestModel>();
            modelItem.draw = request.draw;
            var data = _requestService.FilterData(request.start, request.length, filter.Search, RequestType.MissionDaily, RequestState.All, userId);
            modelItem.recordsTotal = _requestService.Count(filter.Search, RequestType.MissionDaily, RequestState.All, userId);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new RequestModel();
                model.Index = ++add;
                model.Id = x.Id;

                model.CreateDateStriing = DateTimeHelper.TopersianDate(x.CreateDate);
                switch (x.State)
                {
                    case RequestState.Pending:
                        model.StateString = "<span style='color:orange'>معلق</span>";
                        break;
                    case RequestState.Approved:
                        model.StateString = "<span style='color:green'>تایید شده</span>";
                        break;
                    case RequestState.Rejected:
                        model.StateString = "<span style='color:red'>رد شده</span>";
                        break;
                    case RequestState.All:
                        model.StateString = "<span style='color:black'>نامشخص</span>";
                        break;
                    default:
                        model.StateString = "<span style='color:black'>نامشخص</span>";
                        break;
                }

                switch (x.Type)
                {
                    case RequestType.HourlyRest:
                        model.StartDateString = $"{DateTimeHelper.TopersianDate(x.StartDate)} <br/> {x.StartDate.TimeOfDay} <br/> {x.EndDate.TimeOfDay}";
                        break;
                    case RequestType.DailyRest:
                        model.StartDateString = $"{DateTimeHelper.TopersianDate(x.StartDate)} <br/> {DateTimeHelper.TopersianDate(x.EndDate)} ";
                        break;
                    case RequestType.MissionDaily:
                        model.StartDateString = $"{DateTimeHelper.TopersianDate(x.StartDate)} <br/> {DateTimeHelper.TopersianDate(x.EndDate)} ";
                        break;
                    case RequestType.All:
                        model.StartDateString = $"- ";
                        break;
                    default:
                        model.StartDateString = $"- ";
                        break;
                }
                model.UserRequester = UserHelper.FullInformation(x.UserRequesterId);
                model.UserRequesteManager = x.UserRequesteManagerId != null ? UserHelper.FullInformation(x.UserRequesteManagerId) : "-";


                if (x.State == RequestState.Pending)
                {
                    if (!CurrentUserRoleType.Contains("610064006D0069006E00n") && !CurrentUserRoleType.Contains("640065006D006F00o"))
                        model.Buttom = $"<div class='dropdown mPointer'><button class='btn btn-dark btn-sm dropdown-toggle' type='button' data-toggle='dropdown' aria-expanded='true'>عملیات<span class='caret'></span></button><ul class='dropdown-menu'><li onclick='Delete(\"/MissionRequest/Delete?id={x.Id}\")><span class='dropdown-item '> حذف </span></li></ul></div>";
                    else
                        model.Buttom = $"<div class='dropdown mPointer'><button class='btn btn-dark btn-sm dropdown-toggle' type='button' data-toggle='dropdown' aria-expanded='true'>عملیات<span class='caret'></span></button><ul class='dropdown-menu'><li onclick='Delete(\"/MissionRequest/Delete?id={x.Id}\")'><span class='dropdown-item '> حذف </span></li><li onclick='Approve({x.Id})'><span class='dropdown-item '> تایید </span></li><li onclick='DisApprove({x.Id})'><span class='dropdown-item '> عدم پذیرش </span></li></ul></div>";
                }
                else
                    model.Buttom = $"<div class='dropdown mPointer'><button class='btn btn-dark btn-sm dropdown-toggle' type='button' data-toggle='dropdown' aria-expanded='true'>عملیات<span class='caret'></span></button><ul class='dropdown-menu'><li onclick='Show({x.Id})'><span class='dropdown-item '> نمایش </span></li></ul></div>";


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
            DailyRequestModel model = new DailyRequestModel();

            var currentUser = UserHelper.CurrentUser();
            if (currentUser.UserRoleType != "610064006D0069006E00n")
                model.UsersList = model.UsersList.Where(x => x.Value == currentUser.Id).ToList();


            return PartialView("_AddOrUpdate", model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(DailyRequestModel model)
        {
            JsonModel jsonModel = new JsonModel();
            var _userService = DependencyResolver.Current.GetService<IUserService>();
            var _userGroupCalendarService = DependencyResolver.Current.GetService<IUserGroupCalendareService>();
            var user = _userService.GetList.FirstOrDefault(x => x.Id == model.UserId);
            bool state = false;

            int year = int.Parse(model.StartDate.Split('/')[0]);


            var userGroupList = _userGroupCalendarService.GetList;
            if (userGroupList != null && userGroupList.Count() > 0)
                userGroupList = userGroupList.Where(x => x.Remove == false && x.Calendar.Year == year && x.UserGroupId == user.UserGroupId).ToList();



            if (RequestHelper.HasCalendar(userGroupList, year))
            {
                var StartDateGeo = DateTimeHelper.ToGeoDate(model.StartDate);
                var EndDateGeo = DateTimeHelper.ToGeoDate(model.EndDate);
                string CurrentUserId = UserHelper.CurrentUserId();

                var list = _requestService.GetList;
                if (list != null && list.Count() > 0)
                {
                    list = list.Where(x =>
                    x.UserRequesterId == CurrentUserId &&
                    (x.StartDate.Date <= StartDateGeo && StartDateGeo <= x.EndDate.Date) ||
                    (x.StartDate.Date <= EndDateGeo && EndDateGeo <= x.EndDate.Date))
                        .ToList();

                    if (list.Count() > 0)
                    {
                        model.Error = "در این بازه درخواستی دیگر (مرخصی/ماموریت) ثبت شده است";
                        state = true;
                    }
                }

                if (StartDateGeo.GetValueOrDefault().Date > EndDateGeo.GetValueOrDefault().Date)
                {
                    model.Error = "تاریخ پایان باید بزرگتر از تاریخ شروع باشد";
                    state = true;
                }


                if (ModelState.IsValid && state == false)
                {
                    try
                    {

                        Request entity = new Request();

                        entity.State = RequestState.Pending;
                        entity.Type = RequestType.MissionDaily;
                        entity.StartDate = StartDateGeo.GetValueOrDefault();
                        entity.EndDate = EndDateGeo.GetValueOrDefault();
                        entity.UserRequesterId = model.UserId;
                        entity.CreateDate = DateTime.Now;
                        entity.CalendarId = userGroupList.FirstOrDefault().CalendarId;
                     


                        _requestService.Create(entity);
                        model = new DailyRequestModel();
                        jsonModel.Success();
                    }
                    catch (Exception e)
                    {
                        jsonModel.Exception();
                    }
                }
            }
            else
            {
                model.Error = "شیفت کاری برای این کاربر وجود ندارد";
                state = true;
            }

            
            jsonModel.Html = HtmlToJsonHelper.RenderPartialView(this, "_AddOrUpdate", model);
            return Json(jsonModel);
        }
        #endregion
        #region Delete
        public ActionResult Delete(int id = 0)
        {
            bool result = false;
            try
            {
                Request entity = _requestService.FindById(id);
                _requestService.Delete(entity);
                result = true;
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Approve
        public ActionResult Approve(int id = 0)
        {
            bool result = false;
            try
            {
                Request entity = _requestService.FindById(id);
                entity.State = RequestState.Approved;
                _requestService.Update(entity);
                result = true;
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Show
        public ActionResult Show(int id = 0)
        {
            var entity = _requestService.FindById(id);
            return PartialView("_Show", entity);
        }
        #endregion

        #region DisApprove
        public ActionResult DisApprove(int id = 0)
        {
            var entity = _requestService.FindById(id);
            return PartialView("_DisApprove", entity);
        }
        [HttpPost]
        public ActionResult DisApprove(int id = 0, string desc = null)
        {
            bool result = false;
            try
            {
                var entity = _requestService.FindById(id);
                entity.RejectReason = desc;
                entity.State = RequestState.Rejected;
                _requestService.Update(entity);
                result = true;
            }
            catch (Exception e)
            {
                result = false;
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
    }
}
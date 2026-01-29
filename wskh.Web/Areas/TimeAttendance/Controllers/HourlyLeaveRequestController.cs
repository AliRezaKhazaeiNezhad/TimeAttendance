using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Model;
using TimeAttendance.Web.Helper;
using TimeAttendance.WebEssentials.DateAndTime;
using TimeAttendance.WebEssentials.OtherHelper;
using TimeAttendance.WebEssentials.RequestPart;
using TimeAttendance.WebEssentials.StringAndNumber;
using wskh.Core;
using wskh.Core.Enumerator;
using wskh.Service;
using wskh.Web.Helper;
using wskh.WebEssentials.DataTablePart;
using wskh.WebEssentials.DateAndTime;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    public class HourlyLeaveRequestController : Controller
    {
        #region Propertices
        private IRequestService _requestService { get; set; }
        private IUserService _userService { get; set; }
        #endregion


        #region Ctor

        public HourlyLeaveRequestController(IRequestService requestService, IUserService userService)
        {
            _requestService = requestService;
            _userService = userService;
        }

        #endregion


        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MenuName = $"leavehourly";
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
            var data = _requestService.FilterData(request.start, request.length, filter.Search, RequestType.HourlyRest, RequestState.All, userId);
            modelItem.recordsTotal = _requestService.Count(filter.Search, RequestType.DailyRest, RequestState.All, userId);
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
                        model.StartDateString = $"{DateTimeHelper.TopersianDate(x.StartDate)} <br/> {x.StartTime} <br/> {x.EndTime}";
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
                    case RequestType.MissionHourly:
                        model.StartDateString = $"{DateTimeHelper.TopersianDate(x.StartDate)} <br/> {x.StartTime} <br/> {x.EndTime}";
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
                        model.Buttom = $"<div class='dropdown mPointer'><button class='btn btn-dark btn-sm dropdown-toggle' type='button' data-toggle='dropdown' aria-expanded='true'>عملیات<span class='caret'></span></button><ul class='dropdown-menu'><li onclick='Delete(\"/TimeAttendance/HourlyLeaveRequest/Delete?id={x.Id}\")><span class='dropdown-item '> حذف </span></li></ul></div>";
                    else
                        model.Buttom = $"<div class='dropdown mPointer'><button class='btn btn-dark btn-sm dropdown-toggle' type='button' data-toggle='dropdown' aria-expanded='true'>عملیات<span class='caret'></span></button><ul class='dropdown-menu'><li onclick='Delete(\"/TimeAttendance/HourlyLeaveRequest/Delete?id={x.Id}\")'><span class='dropdown-item '> حذف </span></li><li onclick='Approve({x.Id})'><span class='dropdown-item '> تایید </span></li><li onclick='DisApprove({x.Id})'><span class='dropdown-item '> عدم پذیرش </span></li></ul></div>";
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
            HourlyRequestModel model = new HourlyRequestModel();
            var currentUser = UserHelper.CurrentUser();
            if (currentUser.UserRoleType != "610064006D0069006E00n")
                model.UsersList = model.UsersList.Where(x => x.Value == currentUser.Id).ToList();

            return PartialView("_AddOrUpdate", model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(HourlyRequestModel model)
        {
            JsonModel jsonModel = new JsonModel();

            model.StartDate = model.DateStriing;

            var DateGeo = DateTimeHelper.ToGeoDate(model.DateStriing);
            var currentUser = _userService.GetList.FirstOrDefault(x => x.Id == model.UserId);
            string CurrentUserId = UserHelper.CurrentUserId();
            int calendarId = 0;
            int currentYear = int.Parse(model.DateStriing.Split('/')[0]);
            calendarId = currentUser
                .UserGroup
                .UserGroupCalendares
                .FirstOrDefault(x => x.Remove == false && x.Calendar.Year == currentYear)
                .CalendarId;

            bool state = false;

            var list = _requestService.GetList;
            if (list != null && list.Count() > 0)
                list = list.Where(x => (DateGeo.GetValueOrDefault().Date <= x.StartDate.Date ||
                x.EndDate.Date <= DateGeo.GetValueOrDefault().Date) &&
                x.State == RequestState.Approved &&
                x.Type != RequestType.HourlyRest).ToList();

            if (list != null && list.Count() > 0)
                list = list.Where(x =>
                (DateTimeHelper.ToTimeSpan(model.StartHour) <= DateTimeHelper.ToTimeSpan(x.StartTime) ||
                DateTimeHelper.ToTimeSpan(x.EndTime) <= DateTimeHelper.ToTimeSpan(model.EndHour)) &&
                x.State == RequestState.Approved && x.Type == RequestType.HourlyRest).ToList();





            if (list != null && list.Count() > 0)
            {

                list = list.Where(x => x.UserRequesterId == CurrentUserId).ToList();
                list = list.Where(x => x.StartDate.Date <= DateGeo && DateGeo <= x.EndDate.Date).ToList();


                if (list.Count() > 0)
                {
                    model.Error = "در این بازه درخواستی دیگر (مرخصی/ماموریت) ثبت شده است";
                    state = true;
                }

            }
            else
            {
                if (state == false)
                {
                    try
                    {
                       
                        Request entity = new Request();

                        var reportResult = RequestHelper
                            .RemainLeave(model.UserId, model.LeaveTypeId, model.DateStriing, model.StartHour, model.EndHour);

                        ///اگر مرخصی درخواستی استحقاقی باشد
                        if (model.LeaveTypeId == 1 && reportResult.Item6)
                        {
                            entity.LeaveTypeId = model.LeaveTypeId;
                            entity.State = RequestState.Pending;

                            ///بررسی میشود آیا سقف  ساعتی را زده و روزانه محاسبه شود یا خیر
                            if (reportResult.Item7)
                            {
                                /// در این قسمت مرخصی روزانه است
                                entity.Type = RequestType.DailyRest;

                            }
                            else
                            {
                                /// در این قسمت مرخصی ساعتی است
                                entity.Type = RequestType.HourlyRest;

                                entity.StartTime = model.StartHour;
                                entity.EndTime = model.EndHour;
                            }

                            entity.StartDate = DateGeo.GetValueOrDefault();
                            entity.EndDate = DateGeo.GetValueOrDefault();
                            entity.UserRequesterId = model.UserId;
                            entity.CreateDate = DateTime.Now;

                            entity.TotalTime = reportResult.Item1;
                        }
                        else
                        {
                            entity.LeaveTypeId = model.LeaveTypeId;
                            entity.State = RequestState.Pending;
                            entity.Type = RequestType.HourlyRest;
                            entity.StartTime = model.StartHour;
                            entity.EndTime = model.EndHour;
                            entity.StartDate = DateGeo.GetValueOrDefault();
                            entity.EndDate = DateGeo.GetValueOrDefault();
                            entity.UserRequesterId = model.UserId;
                            entity.CreateDate = DateTime.Now;

                            entity.TotalTime = reportResult.Item1;
                        }
                        entity.CalendarId = calendarId;


                        _requestService.Create(entity);
                        model = new HourlyRequestModel();
                        jsonModel.Success();
                    }
                    catch (Exception e)
                    {
                        jsonModel.Exception();
                    }
                }
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

        #region GetDetail
        public ActionResult GetDetail(string userId)
        {
            List<PersonalRequestDetailModel> modelList = new List<PersonalRequestDetailModel>();

            var _userGroupCalendarService = DependencyResolver.Current.GetService<IUserGroupCalendareService>();
            var _calendarService = DependencyResolver.Current.GetService<ICalendarService>();
            var _userService = DependencyResolver.Current.GetService<IUserService>();

            #region شناسایی میکند آیا کاربر مدیر است یا خیر
            var currentUser = UserHelper.CurrentUser();
            if (currentUser.UserRoleType != "610064006D0069006E00n")
                userId = currentUser.Id;
            else
                currentUser = _userService.GetList.FirstOrDefault(x => x.Id == userId);
            #endregion

            #region تقویم سال جاری و قبلی را میگیرد
            var list = _userGroupCalendarService.GetList;

            var calendarLastYear = list.FirstOrDefault(x => x.Remove == false && x.UserGroupId == currentUser.UserGroupId && x.Calendar.Year == TimeHelper.LastYear());
            var calendarThisYear = list.FirstOrDefault(x => x.Remove == false && x.UserGroupId == currentUser.UserGroupId && x.Calendar.Year == TimeHelper.ThisYear());
            #endregion




            return View("_GetDetail", modelList);
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
        #region Check
        [HttpGet]
        public ActionResult Check(string userId, string dateStriing)
        {
            int result = 0;

            var user = _userService.GetList.FirstOrDefault(x => x.Id == userId);
            int year = int.Parse(dateStriing.Split('/')[0]);

            JsonRequestModel model = new JsonRequestModel();
            try
            {
                if (RequestHelper.HasCalendar(user.UserGroup.UserGroupCalendares, year))
                {
                    model.Message = "امکان ثبت وجود دارد";
                    model.State = 1;
                }
                else
                {
                    model.Message = "شیفت کاری برای این کاربر وجود ندارد";
                    model.State = 0;
                }
            }
            catch (Exception e)
            {
                model.Message = "خطایی رخ داده است";
                model.State = -1;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Report
        [HttpGet]
        public ActionResult Report(string userId, int leaveTypeId, string dateStriing, string startHour, string endHour, string date)
        {
            LeaveReportModel model = new LeaveReportModel();
            try
            {
                model.UserId = userId;
                model.LeaveTypeId = leaveTypeId;
                model.StartHour = startHour;
                model.EndHour = endHour;
                model.Date = date;
                model.Year = int.Parse(dateStriing.Split('/')[0]);
            }
            catch (Exception)
            {
                model = new LeaveReportModel();
            }

            return PartialView("_Report", model);
        }
        #endregion
    }
}
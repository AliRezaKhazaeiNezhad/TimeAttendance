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
    public class DailyMissionReportController : Controller
    {
        #region Propertices
        private IUserService _userService { get; set; }
        private IUserGroupService _userGroupService { get; set; }
        #endregion


        #region Ctor
        public DailyMissionReportController(IUserService userService, IUserGroupService userGroupService)
        {
            _userService = userService;
            _userGroupService = userGroupService;
        }
        #endregion


        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MenuName = $"dailymissionReport";
            return View();
        }
        #endregion
        #region Search
        [HttpGet]
        public ActionResult Search(int groupId = 0, int year = 0, int stMonth = 0, int stDay = 0, int edMonth = 0, int edDay = 0)
        {
            #region پر کردن مدل
            HourlyLeaveReportModel model = PrepareSearchModel(groupId, year, stMonth, stDay, edMonth, edDay);
            #endregion

            #region پر کردن سشن
            PrepareSession(model);
            #endregion

            return PartialView("_Search", model);
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
            var sessionModel = RetriveSession();
            var modelList = new List<HourlyLeaveGridModel>();
            var list = _userGroupService.GetList;


            if (request.length == -1)
                request.length = int.MaxValue;
            var modelItem = new DataTableResponse<HourlyLeaveGridModel>();


            #region بدست آوردن لیست مرخصی ها
            ///********************************این بخش باید متد کمکی شود
            PrepareLeaveGridModel(sessionModel, modelList, list);
            #endregion

            #region تهیه لیست مدل برای گیرید
            modelItem.draw = request.draw;
            var data = PrepareFilterData(request, filter, modelList);
            modelItem.recordsTotal = PrepareFilterData(request, filter, modelList).Count();
            modelItem.recordsFiltered = modelItem.recordsTotal;
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new HourlyLeaveGridModel();
                model = x;
                model.Index = ++add;
                model.Id = x.Id;
                //model.MaximumLeave = TimeHelper.TotalHours(x.MaximumLeave);
                model.UsedLeave = TimeHelper.TotalHours(x.UsedLeave);
                //model.RemainLeave = TimeHelper.TotalHours(x.RemainLeave);

                modelItem.data.Add(model);
            });
            #endregion

            return Json(modelItem, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region PrepareMethods
        private void PrepareSession(HourlyLeaveReportModel model)
        {
            Session["UserGroupId"] = model.UserGroupId;
            Session["LeaveTypeId"] = model.LeaveTypeId;
            Session["YearId"] = model.YearId;
            Session["StartMonthId"] = model.StartMonthId;
            Session["StartDay"] = model.StartDay;
            Session["EndMonthId"] = model.EndMonthId;
            Session["EndDay"] = model.EndDay;
            Session["LeaveTypeId"] = 0;
        }
        private static HourlyLeaveReportModel PrepareSearchModel(int groupId, int year, int stMonth, int stDay, int edMonth, int edDay)
        {
            HourlyLeaveReportModel model = new HourlyLeaveReportModel(DateTimeHelper.CurrentPersianYear());
            model.UserGroupId = groupId;
            model.LeaveTypeId = 0;
            model.YearId = year <= 0 ? DateTimeHelper.CurrentPersianYear() : year;
            model.StartMonthId = stMonth <= 0 ? 1 : stMonth;
            model.StartDay = stDay <= 0 ? 1 : stDay;
            model.EndMonthId = edMonth <= 0 ? 12 : edMonth;
            model.EndDay = edDay <= 0 ? 29 : edDay;

            if (model.StartDay < 1 || model.StartDay > 30)
                model.StartDay = 1;

            var bigYears = WebConfigHelper.Bigyears();
            if (model.EndDay < 1 || model.EndDay > 30)
                model.EndDay = 29;

            if (bigYears.Contains(model.YearId.ToString()))
                model.EndDay = 30;

            return model;
        }
        private HourlyLeaveReportModel RetriveSession()
        {
            HourlyLeaveReportModel model = new HourlyLeaveReportModel(DateTimeHelper.CurrentPersianYear());
            model.UserGroupId = string.IsNullOrEmpty(Session["UserGroupId"].ToString()) ? 0 : int.Parse(Session["UserGroupId"].ToString());
            model.LeaveTypeId = string.IsNullOrEmpty(Session["LeaveTypeId"].ToString()) ? 0 : int.Parse(Session["LeaveTypeId"].ToString());
            model.YearId = string.IsNullOrEmpty(Session["YearId"].ToString()) ? 0 : int.Parse(Session["YearId"].ToString());
            model.StartMonthId = string.IsNullOrEmpty(Session["StartMonthId"].ToString()) ? 0 : int.Parse(Session["StartMonthId"].ToString());
            model.StartDay = string.IsNullOrEmpty(Session["StartDay"].ToString()) ? 0 : int.Parse(Session["StartDay"].ToString());
            model.EndMonthId = string.IsNullOrEmpty(Session["EndMonthId"].ToString()) ? 0 : int.Parse(Session["EndMonthId"].ToString());
            model.EndDay = string.IsNullOrEmpty(Session["EndDay"].ToString()) ? 0 : int.Parse(Session["EndDay"].ToString());

            return model;
        }
        private static void PrepareLeaveGridModel(HourlyLeaveReportModel sessionModel, List<HourlyLeaveGridModel> modelList, List<UserGroup> list)
        {
            var _requsetService = DependencyResolver.Current.GetService<IRequestService>();
            var _calendarService = DependencyResolver.Current.GetService<ICalendarService>();
            var allRequests = _requsetService.GetList;
            var allCalendars = _calendarService.GetList;
            if (allRequests != null)
                allRequests = allRequests.Where(x => x.State == RequestState.Approved && x.Type == RequestType.MissionDaily).ToList();

            if (list != null && list.Count() > 0)
            {
                foreach (var group in list)
                {
                    if (group != null && group.Users != null && group.Users.Count() > 0)
                    {
                        foreach (var user in group.Users)
                        {
                            HourlyLeaveGridModel modelItem1 = new HourlyLeaveGridModel();
                            modelItem1.GroupName = group.Title;
                            modelItem1.NameAndFamily = $"{user.FirstName} {user.Lastname}";
                            modelItem1.Year = sessionModel.YearId;
                            modelItem1.Type = "";

                            var userRequestList = allRequests.Where(x => x.UserRequesterId == user.Id).ToList();

                            ///فیلتر کردن براساس نوع مرخصی
                            //userRequestList = userRequestList.Where(x => x.LeaveTypeId == sessionModel.LeaveTypeId).ToList();

                            modelItem1.MaximumLeave = "0";
                            modelItem1.RemainLeave = "0";

                            /// محاسبه ماموریت استفاده شده
                            if (userRequestList != null && userRequestList.Count() > 0)
                            {
                                int usedLeave = 0;
                                foreach (var request in userRequestList)
                                {
                                    usedLeave = usedLeave + int.Parse(request.TotalTime);
                                }
                                modelItem1.UsedLeave = usedLeave.ToString();
                            }


                            modelList.Add(modelItem1);
                        }
                    }
                }
            }
        }
        private static List<HourlyLeaveGridModel> PrepareFilterData(DataTableRequest request, DataTableRequestFilter filter, List<HourlyLeaveGridModel> modelList)
        {
            /// قسمت جستجو در لیست
            if (!string.IsNullOrEmpty(filter.Search))
            {
                filter.Search = filter.Search.ToLower();
                modelList = modelList
                    .Where(x =>
                        x.GroupName.ToLower().Contains(filter.Search) ||
                        x.NameAndFamily.ToLower().Contains(filter.Search) ||
                        x.Year.ToString().ToLower().Contains(filter.Search) ||
                        x.MaximumLeave.ToLower().Contains(filter.Search) ||
                        x.UsedLeave.ToLower().Contains(filter.Search) ||
                        x.RemainLeave.ToLower().Contains(filter.Search))
                    .ToList();
            }
            /// قسمت سورت کردن ستون ها
            switch (filter.SortColumn)
            {
                case 1:
                    modelList = filter.SortDirection == "asc" ? modelList.OrderBy(x => x.GroupName).ToList() : modelList.OrderByDescending(x => x.GroupName).ToList();
                    break;
                case 2:
                    modelList = filter.SortDirection == "asc" ? modelList.OrderBy(x => x.NameAndFamily).ToList() : modelList.OrderByDescending(x => x.NameAndFamily).ToList();
                    break;
                case 3:
                    modelList = filter.SortDirection == "asc" ? modelList.OrderBy(x => x.Year).ToList() : modelList.OrderByDescending(x => x.Year).ToList();
                    break;
                case 4:
                    modelList = filter.SortDirection == "asc" ? modelList.OrderBy(x => x.MaximumLeave).ToList() : modelList.OrderByDescending(x => x.MaximumLeave).ToList();
                    break;
                case 5:
                    modelList = filter.SortDirection == "asc" ? modelList.OrderBy(x => x.UsedLeave).ToList() : modelList.OrderByDescending(x => x.UsedLeave).ToList();
                    break;
                case 6:
                    modelList = filter.SortDirection == "asc" ? modelList.OrderBy(x => x.RemainLeave).ToList() : modelList.OrderByDescending(x => x.RemainLeave).ToList();
                    break;
                case 7:
                    modelList = filter.SortDirection == "asc" ? modelList.OrderBy(x => x.Type).ToList() : modelList.OrderByDescending(x => x.Type).ToList();
                    break;
                default:
                    break;
            }
            ///تهیه لیست نهایی
            modelList = modelList.GetRange(request.start, Math.Min(request.length, modelList.Count - request.start));
            return modelList;
        }
        #endregion
    }
}
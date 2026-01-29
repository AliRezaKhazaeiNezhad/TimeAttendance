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
using TimeAttendance.WebEssentials.ReportHelper;
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
    public class TradeReportController : Controller
    {
        #region Propertices
        private IReportDayService _reportDayService { get; set; }
        private IUserService _userService { get; set; }
        private ILogService _logService { get; set; }
        #endregion

        #region Ctor

        public TradeReportController(IUserService userService, IReportDayService reportDayService, ILogService logService)
        {
            _userService = userService;
            _reportDayService = reportDayService;
            _logService = logService;
        }

        #endregion

        #region Search
        [HttpGet]
        public ActionResult Search(int groupId = 0, string userId = "", string startDate = "", string endDate = "", int tradeTypeId = 0)
        {
            #region پر کردن مدل
            TradeSearchModel model = PrepareSearchModel(groupId, userId, startDate, endDate, tradeTypeId);
            #endregion

            #region پر کردن سشن
            PrepareSession(model);
            #endregion

            return PartialView("_Search", model);
        }
        #endregion

        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MenuName = $"tradereport";
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
            var sessionModel = RetriveSession();
            var modelList = new List<TradeSearchModel>();


            #region Grid configuration
            if (request.length == -1)
            {
                request.length = int.MaxValue;
            }
            var modelItem = new DataTableResponse<InstantReportModel>();
            modelItem.draw = request.draw;

            var data = _reportDayService.TradeFilterData(request.start, request.length, filter.Search, sessionModel.UserGroupId, sessionModel.UserId, DateTimeHelper.ToGeoDate(sessionModel.StartDate), DateTimeHelper.ToGeoDate(sessionModel.EndDate), sessionModel.TradeTypeId, filter.SortColumn, filter.SortDirection);
            modelItem.recordsTotal = _reportDayService.TradeCount(filter.Search, sessionModel.UserGroupId, sessionModel.UserId, DateTimeHelper.ToGeoDate(sessionModel.StartDate), DateTimeHelper.ToGeoDate(sessionModel.EndDate), sessionModel.TradeTypeId);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new InstantReportModel();
                model.Index = ++add;
                try
                {
                    model.Id = 0;
                    model.NameAndFamily = $"{x.User.FirstName} {x.User.Lastname}";
                    model.GroupName = x.User.UserGroup.Title;
                    model.EnteranceTime = "-";
                    model.ExitTime = "-";
                    model.Logs = "-";
                    model.DayName = "-";
                    model.PersianDaet = "-";


                    var reportDay = x;


                    model.Id = reportDay.Id;
                    var logs = reportDay.Logs;

                    model.DayName = reportDay.PersianDayName;
                    model.PersianDaet = reportDay.PersianDate;

                    if (logs != null && logs.Count() > 0)
                    {
                        logs = logs.Where(f => f.Remove == false).ToList();
                    }

                    var res = ReportHelper.RawTrades(logs, true, reportDay.Id, x.UserId);
                    model.EnteranceTime = res.Item1;
                    model.Button1 = res.Item2;
                }
                catch (Exception e)
                {
                }

                modelItem.data.Add(model);
            });
            #endregion

            return Json(modelItem, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Show
        public ActionResult Show(int id, string userId)
        {
            var entity = _reportDayService.FindById(id);
            if (id == 0)
            {
                entity = new ReportDay();
                entity.UserId = userId;
                entity.User = _userService.GetList.FirstOrDefault(x => x.Id == userId);
            }
            return PartialView("_Show", entity);
        }
        #endregion

        #region TradeList
        public ActionResult TradeList(int id, string userId)
        {
            ReportDay entity = _reportDayService.FindById(id);


            if (entity != null && entity.Id > 0)
                id = entity.Id;

            if (id == 0)
            {
                entity = new ReportDay();
                entity.UserId = userId;
                entity.User = _userService.GetList.FirstOrDefault(x => x.Id == userId);
            }
            return PartialView("_TradeList", entity);
        }
        #endregion

        #region AddLog
        public ActionResult AddLog(int reportDayId = 0, string userId = "", string time = "")
        {
            bool result = false;
            time = time + ":00";
            try
            {
                ReportDay entity = reportDayId <= 0 ? new ReportDay() : _reportDayService.FindById(reportDayId);
                var findUser = _userService.GetList.FirstOrDefault(x => x.Id == userId);
                if (reportDayId == 0)
                {
                    entity.UserId = userId;
                    DateTime dt = DateTime.Now;
                    entity.PersianDate = DateTimeHelper.TopersianDate(dt);
                    entity.PersianYear = int.Parse(entity.PersianDate.Split('/')[0]);
                    entity.PersianMonth = int.Parse(entity.PersianDate.Split('/')[1]);
                    entity.PersianDay = int.Parse(entity.PersianDate.Split('/')[2]);
                    entity.PersianDayName = DateTimeHelper.GetPersianDayName(dt);
                    entity.ReportDate = DateTime.Now;
                    entity.DayInWeek = TimeHelper.GetDayInWeek(DateTime.Now);
                    entity.State = ReportState.Analyzing;
                    entity.WorkType = WorkType.Other;
                    entity.Logs = new List<Log>();
                    _reportDayService.Create(entity);

                    entity.Logs.Add(new Log()
                    {
                        VerifyMode = "0",
                        InOutMode = "0",
                        WorkCode = "0",
                        Orgin = false,
                        LogDate = DateTime.Now.Date,
                        LogTime = time,
                        State = LogState.Analyzing,
                        TransportType = LogTransportType.Enterance,
                        EnrollId = findUser.Enrolls.FirstOrDefault().Id,
                        DeviceId = findUser.Enrolls.FirstOrDefault().FingerDeviceId,
                        EnrollNo = findUser.Enrolls.FirstOrDefault().EnrollNo,
                        Remove = false,
                        CreatorUserId = UserHelper.CurrentUserId()
                    });
                    if (entity.Logs == null || entity.Logs.Count <= 0)
                        entity.TradeType = TradeType.Absence;
                    if (entity.Logs.Count % 2 == 0)
                        entity.TradeType = TradeType.Completed;
                    else
                        entity.TradeType = TradeType.Fraction;

                    _reportDayService.Update(entity);
                    result = true;
                }
                else
                {
                    entity.Logs.Add(new Log()
                    {
                        CreatorUserId = UserHelper.CurrentUserId(),
                        VerifyMode = "0",
                        InOutMode = "0",
                        WorkCode = "0",
                        Orgin = false,
                        LogDate = DateTime.Now.Date,
                        LogTime = time,
                        State = LogState.Analyzing,
                        TransportType = LogTransportType.Enterance,
                        EnrollId = findUser.Enrolls.FirstOrDefault().Id,
                        DeviceId = findUser.Enrolls.FirstOrDefault().FingerDeviceId,
                        EnrollNo = findUser.Enrolls.FirstOrDefault().EnrollNo,
                        Remove = false
                    });
                    _reportDayService.Update(entity);
                    result = true;
                }

            }
            catch (Exception e)
            {
                result = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete
        public ActionResult Delete(int logId = 0)
        {
            int result = 0;
            try
            {
                Log entity = _logService.FindById(logId);
                entity.Remove = true;
                entity.CreatorUserId = UserHelper.CurrentUserId();
                _logService.Update(entity);

                result = 1;
            }
            catch (Exception e)
            {
                result = -2;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PrepareMethods
        private void PrepareSession(TradeSearchModel model)
        {
            Session["UserGroupId"] = model.UserGroupId;
            Session["UserId"] = model.UserId;
            Session["StartDate"] = model.StartDate;
            Session["EndDate"] = model.EndDate;
            Session["TradeTypeId"] = model.TradeTypeId;
        }
        private static TradeSearchModel PrepareSearchModel(int groupId, string userId, string startDate, string endDate, int tradeTypeId)
        {
            TradeSearchModel model = new TradeSearchModel();
            if (!string.IsNullOrEmpty(userId) && userId == "0")
            {
                userId = "";
            }
            model.UserGroupId = groupId;
            model.UserId = userId;
            model.StartDate = string.IsNullOrEmpty(startDate) ? DateTimeHelper.TopersianDate(DateTime.Now) : startDate;
            model.EndDate = string.IsNullOrEmpty(endDate) ? DateTimeHelper.TopersianDate(DateTime.Now) : endDate;
            model.TradeTypeId = tradeTypeId;

            return model;
        }
        private TradeSearchModel RetriveSession()
        {
            TradeSearchModel model = new TradeSearchModel();
            model.UserGroupId = string.IsNullOrEmpty(Session["UserGroupId"].ToString()) ? 0 : int.Parse(Session["UserGroupId"].ToString());
            model.UserId = string.IsNullOrEmpty(Session["UserId"].ToString()) ? "" : Session["UserId"].ToString();
            model.StartDate = string.IsNullOrEmpty(Session["StartDate"].ToString()) ? DateTimeHelper.TopersianDate(DateTime.Now) : Session["StartDate"].ToString();
            model.EndDate = string.IsNullOrEmpty(Session["EndDate"].ToString()) ? DateTimeHelper.TopersianDate(DateTime.Now) : Session["EndDate"].ToString();
            model.TradeTypeId = string.IsNullOrEmpty(Session["TradeTypeId"].ToString()) ? 0 : int.Parse(Session["TradeTypeId"].ToString());

            return model;
        }
        #endregion


        #region GetDrop
        public ActionResult GetDrop(int usergroupId = 0)
        {
            List<DropDownModel> modelList = new List<DropDownModel>();
            modelList.Add(new DropDownModel() { Value = "0", Text = "تمامی پرسنل" });

            if (usergroupId > 0)
            {
                var userList = _userService.GetList;
                if (!ListHelper.IsListNull(userList))
                {
                    userList = userList.Where(x => x.UserGroupId == usergroupId).ToList();
                    foreach (var user in userList)
                    {
                        modelList.Add(new DropDownModel() { Value = user.Id, Text = $"{user.FirstName} {user.Lastname}" });
                    }
                }
            }
            return View("_GetDrop", modelList);
        }
        #endregion

        #region DeleteTrade
        public ActionResult DeleteTrade(int firstId = 0, int secondId = 0)
        {
            int result = 0;
            try
            {
                Log entityFirst = _logService.FindById(firstId);
                Log entitySecond = new Log();
                if (secondId > 0)
                {
                    entitySecond = _logService.FindById(secondId);
                    entitySecond.Remove = true;
                    entitySecond.CreatorUserId = UserHelper.CurrentUserId();
                    _logService.Update(entitySecond);
                }
                entityFirst.Remove = true;
                entityFirst.CreatorUserId = UserHelper.CurrentUserId();
                _logService.Update(entityFirst);
                result = 1;
            }
            catch (Exception e)
            {
                result = -2;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AddRowToUser
        public ActionResult AddRowToUser(int reportDayId, string userId)
        {
            AddTradeModel model = new AddTradeModel();
            model.ReportDayId = reportDayId;
            model.UserId = userId;
            model.EnteranceTime = "08:00";
            model.ExitTime = "15:00";

            return PartialView("_AddRowToUser", model);
        }

        [HttpPost]
        public ActionResult AddRowToUser(int reportDayId, string userId, string enteranceTime, string exitTime)
        {
            bool result = false;
            enteranceTime = enteranceTime + ":00";
            exitTime = exitTime + ":00";
            try
            {
                ReportDay entity = reportDayId <= 0 ? new ReportDay() : _reportDayService.FindById(reportDayId);
                var findUser = _userService.GetList.FirstOrDefault(x => x.Id == userId);
                result = PrepareReport(reportDayId, userId, entity, findUser, enteranceTime, exitTime);
            }
            catch (Exception e)
            {
                result = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CheckTimes
        public ActionResult CheckTimes(string enteranceTime, string exitTime)
        {
            bool result = false;

            enteranceTime = enteranceTime + ":00";
            exitTime = exitTime + ":00";

            var timeOne = TimeHelper.GlobalTimeFormat(enteranceTime);
            var timeTwo = TimeHelper.GlobalTimeFormat(exitTime);

            result = timeTwo > timeOne ? true : false;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PrepareMethods
        private bool PrepareReport(int reportDayId, string userId, ReportDay entity, wskhUser findUser, string time, string timeTwo)
        {
            bool result;
            if (reportDayId == 0)
            {
                entity.UserId = userId;
                DateTime dt = DateTime.Now;
                entity.PersianDate = DateTimeHelper.TopersianDate(dt);
                entity.PersianYear = int.Parse(entity.PersianDate.Split('/')[0]);
                entity.PersianMonth = int.Parse(entity.PersianDate.Split('/')[1]);
                entity.PersianDay = int.Parse(entity.PersianDate.Split('/')[2]);
                entity.PersianDayName = DateTimeHelper.GetPersianDayName(dt);
                entity.ReportDate = DateTime.Now;
                entity.DayInWeek = TimeHelper.GetDayInWeek(DateTime.Now);
                entity.State = ReportState.Analyzing;
                entity.WorkType = WorkType.Other;
                entity.Logs = new List<Log>();
                _reportDayService.Create(entity);

                entity.Logs.Add(new Log()
                {
                    VerifyMode = "0",
                    InOutMode = "0",
                    WorkCode = "0",
                    Orgin = false,
                    LogDate = DateTime.Now.Date,
                    LogTime = time,
                    State = LogState.Analyzing,
                    TransportType = LogTransportType.Enterance,
                    EnrollId = findUser.Enrolls.FirstOrDefault().Id,
                    DeviceId = findUser.Enrolls.FirstOrDefault().FingerDeviceId,
                    EnrollNo = findUser.Enrolls.FirstOrDefault().EnrollNo,
                    Remove = false,
                    CreatorUserId = UserHelper.CurrentUserId()
                });
                entity.Logs.Add(new Log()
                {
                    VerifyMode = "0",
                    InOutMode = "0",
                    WorkCode = "0",
                    Orgin = false,
                    LogDate = DateTime.Now.Date,
                    LogTime = timeTwo,
                    State = LogState.Analyzing,
                    TransportType = LogTransportType.Enterance,
                    EnrollId = findUser.Enrolls.FirstOrDefault().Id,
                    DeviceId = findUser.Enrolls.FirstOrDefault().FingerDeviceId,
                    EnrollNo = findUser.Enrolls.FirstOrDefault().EnrollNo,
                    Remove = false,
                    CreatorUserId = UserHelper.CurrentUserId()
                });
                if (entity.Logs == null || entity.Logs.Count <= 0)
                    entity.TradeType = TradeType.Absence;
                if (entity.Logs.Count % 2 == 0)
                    entity.TradeType = TradeType.Completed;
                else
                    entity.TradeType = TradeType.Fraction;

                _reportDayService.Update(entity);
                result = true;
            }
            else
            {
                entity.Logs.Add(new Log()
                {
                    CreatorUserId = UserHelper.CurrentUserId(),
                    VerifyMode = "0",
                    InOutMode = "0",
                    WorkCode = "0",
                    Orgin = false,
                    LogDate = DateTime.Now.Date,
                    LogTime = time,
                    State = LogState.Analyzing,
                    TransportType = LogTransportType.Enterance,
                    EnrollId = findUser.Enrolls.FirstOrDefault().Id,
                    DeviceId = findUser.Enrolls.FirstOrDefault().FingerDeviceId,
                    EnrollNo = findUser.Enrolls.FirstOrDefault().EnrollNo,
                    Remove = false
                });
                entity.Logs.Add(new Log()
                {
                    VerifyMode = "0",
                    InOutMode = "0",
                    WorkCode = "0",
                    Orgin = false,
                    LogDate = DateTime.Now.Date,
                    LogTime = timeTwo,
                    State = LogState.Analyzing,
                    TransportType = LogTransportType.Enterance,
                    EnrollId = findUser.Enrolls.FirstOrDefault().Id,
                    DeviceId = findUser.Enrolls.FirstOrDefault().FingerDeviceId,
                    EnrollNo = findUser.Enrolls.FirstOrDefault().EnrollNo,
                    Remove = false,
                    CreatorUserId = UserHelper.CurrentUserId()
                });
                _reportDayService.Update(entity);
                result = true;
            }

            return result;
        }
        #endregion
    }
}
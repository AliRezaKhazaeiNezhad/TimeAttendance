using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Model;
using TimeAttendance.Web.Helper;
using wskh.Core.Enumerator;
using wskh.Data;
using wskh.Service;
using wskh.StoredProcedure.Services;
using wskh.Web.Helper;
using wskh.WebEssentials.DataTablePart;
using wskh.WebEssentials.DateAndTime;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    public class LogReportController : Controller
    {
        #region Propertices
        private ILogService _logService;
        private IFingerDeviceService _deviceService;
        private IEnrollService _enrollService;
        private IReportDayService _reportDayService;
        private IUserService _userService;
        private LogSP _logSP;
        #endregion
        #region Ctor
        public LogReportController(ILogService logService, IFingerDeviceService deviceService, IEnrollService enrollService, IReportDayService reportDayService, IUserService userService)
        {
            _logService = logService;
            _deviceService = deviceService;
            _enrollService = enrollService;
            _reportDayService = reportDayService;
            _userService = userService;
            _logSP = new LogSP();
        }
        #endregion
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PanelName = "report";
            ViewBag.MenuName = "rawlog";

            return View();
        }
        #endregion
        #region Search part
        [HttpGet]
        public ActionResult SearchPart()
        {
            Session["logsp"] = _logSP.AnalyzeList();

            if (Session["STDate"] == null)
                Session["STDate"] = DateTimeHelper.TopersianDate(DateTime.Now);

            if (Session["EDDate"] == null)
                Session["EDDate"] = DateTimeHelper.TopersianDate(DateTime.Now);


            return PartialView("_SearchPart");
        }
        #endregion
        #region List
        [HttpGet]
        public ActionResult ListIndex(string stDate = null, string edDate = null, int deviceId = 0, string userId = null, int enrollId = 0)
        {
            if (!string.IsNullOrEmpty(stDate))
                Session["STDate"] = stDate;
            if (!string.IsNullOrEmpty(edDate))
            Session["EDDate"] = edDate;
            Session["DeviceId"] = deviceId;
            Session["UserId"] = userId;
            Session["EnrollId"] = enrollId;
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
            var modelItem = new DataTableResponse<FullLogModel>();
            modelItem.draw = request.draw;


            var CurrentUserRoleType = UserHelper.CurrentUser().UserRoleType;

            string stDate = Session["STDate"] != null ? Session["STDate"].ToString() : null;
            string edDate = Session["EDDate"] != null ? Session["EDDate"].ToString() : null;
            int deviceId = int.Parse(Session["DeviceId"].ToString());
            string userId = Session["UserId"] != null ? Session["UserId"].ToString() : null;
            int enrollId = int.Parse(Session["EnrollId"].ToString());

            if (userId != null && userId.ToLower() == "undefined")
                userId = null;

            if (!CurrentUserRoleType.Contains("610064006D0069006E00n") && !CurrentUserRoleType.Contains("640065006D006F00o"))
                userId = UserHelper.CurrentUserId();


            var startDateTime = DateTimeHelper.ToGeoDate(stDate);
            var endDateTime = DateTimeHelper.ToGeoDate(edDate);


            var list = Session["logsp"] != null ? (List<FullAnalayzedModel>)Session["logsp"] : new List<FullAnalayzedModel>();

            if (!string.IsNullOrEmpty(stDate))
                list = list.Where(x => x.FullLogModels.FirstOrDefault().LogDate.Date >= startDateTime).ToList();

            if (!string.IsNullOrEmpty(edDate))
                list = list.Where(x => x.FullLogModels.FirstOrDefault().LogDate.Date <= endDateTime).ToList();

            if (deviceId > 0)
                list = list.Where(x => x.FullLogModels.FirstOrDefault().DeviceId == deviceId).ToList();

            if (enrollId > 0)
                list = list.Where(x => x.FullLogModels.FirstOrDefault().EnrollId <= enrollId).ToList();

            var data = list;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new FullLogModel();
                model.Index = ++add;
                model.DayName = x.FullLogModels.FirstOrDefault().LogDate.GetPersianDayName();
                model.LogDatePersian = x.FullLogModels.FirstOrDefault().LogDate.TopersianDate();
                model.DeviceName = x.FullLogModels.FirstOrDefault().DeviceName;
                model.EnrollNo = x.FullLogModels.FirstOrDefault().EnrollNo;
                model.LogTime = x.Logs;
                model.UserName = x.FullLogModels.FirstOrDefault().UserId != null ? $"{x.FullLogModels.FirstOrDefault().FirstName} {x.FullLogModels.FirstOrDefault().Lastname}" : "";
                model.StateString = x.FullLogModels.FirstOrDefault().Index % 2 == 0 ? $"{x.FullLogModels.FirstOrDefault().Index} <br/> <span class='enterance'>تردد کامل</span>" : $"{x.FullLogModels.FirstOrDefault().Index} <br/> <span class='exit'>تردد ناقص</span>";

                modelItem.data.Add(model);
                //modelItem.data.Add(x);
            });
            int start = request.start;
            int lenght = request.length;
            modelItem.recordsTotal = modelItem.data.Count();
            modelItem.recordsFiltered = modelItem.recordsTotal;
            modelItem.data = modelItem.data.GetRange(start, Math.Min(lenght, modelItem.data.Count - start));
            #endregion

            return Json(modelItem, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region EnrollDropDown
        public ActionResult EnrollDropDown(int deviceId = 0)
        {
            List<DropDownModel> ModelList = new List<DropDownModel>();
            ModelList.Add(new DropDownModel()
            {
                Text = "انتخاب نمایید",
                Value = "0"
            });

            try
            {
                var list = _enrollService.GetList;
                if (list != null && list.Count() > 0 && deviceId > 0)
                {
                    list = list.Where(x => x.FingerDeviceId == deviceId).ToList();
                    list.ForEach(x => ModelList.Add(new DropDownModel()
                    {
                        Text = $"{x.EnrollNo } {(x.UserId != null ? UserHelper.FullInformation(x.UserId) : " ")}",
                        Value = x.Id.ToString()
                    }));
                }
            }
            catch (Exception e)
            {
            }

            return PartialView("_EnrollDropDown", ModelList);
        }
        #endregion
    }
}
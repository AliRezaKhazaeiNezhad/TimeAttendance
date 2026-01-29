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
    public class RemovedLogsController : Controller
    {
        #region Propertices
        private IUserService _userService { get; set; }
        private ILogService _logService { get; set; }
        #endregion


        #region Ctor

        public RemovedLogsController(IUserService userService, ILogService logService)
        {
            _userService = userService;
            _logService = logService;
        }

        #endregion


        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MenuName = $"removedlogs";
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
            var modelItem = new DataTableResponse<RemovedLogReport>();
            modelItem.draw = request.draw;

            var data = _logService.FilterDataRemovedLogs(request.start, request.length);
            modelItem.recordsTotal = _logService.CountRemovedLogs();
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new RemovedLogReport();
                model.Index = ++add;
                model.LogDate = DateTimeHelper.TopersianDate(x.LogDate.GetValueOrDefault());
                model.LogTime = x.LogTime;
                model.EnrollNumber = x.Enroll.EnrollNo.ToString();
                model.UserName = x.Enroll.UserId != null ? UserHelper.FullInformation(x.Enroll.UserId) : "-";
                model.Device = x.Device.Title;
                model.UserRemove = UserHelper.FullInformation(x.CreatorUserId);


                modelItem.data.Add(model);
            });
            #endregion

            return Json(modelItem, JsonRequestBehavior.AllowGet);
        }
        #endregion

       
    }
}
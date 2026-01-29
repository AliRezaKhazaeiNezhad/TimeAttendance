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
    public class AbsenceReportController : Controller
    {
        #region Propertices
        private IAnalyzedReportService _analyzedReportService  { get; set; }
        private IUserService _userService { get; set; }
        #endregion


        #region Ctor

        public AbsenceReportController(IAnalyzedReportService analyzedReportService, IUserService userService)
        {
            _analyzedReportService = analyzedReportService;
            _userService = userService;
        }

        #endregion


        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MenuName = $"absenceeport";
            return View();
        }
        #endregion


        #region Search
        [HttpGet]
        public ActionResult Search()
        {
            AnalyzedReportModel model = new AnalyzedReportModel();
            return PartialView("_Search", model);
        }
        #endregion


        #region List
        [HttpGet]
        public ActionResult ListIndex(int groupId = 0, string userId = null, string stDate = null, string edDate = null, int analyzedReportState = 0)
        {
            List<AnalyzedReportListModel> enttityList = new List<AnalyzedReportListModel>();

            var userList = _userService.List().Where(x => x.UserGroupId == groupId).ToList();


            try
            {

                if (userList != null)
                {
                    foreach (var user in userList)
                    {
                        var list = new List<AnalyzedReport>();

                        var geoSTDate = !string.IsNullOrEmpty(stDate) ? DateTimeHelper.ToGeoDate(stDate) : DateTime.Now;
                        var geoEDDate = !string.IsNullOrEmpty(edDate) ? DateTimeHelper.ToGeoDate(edDate) : DateTime.Now;

                        list = _analyzedReportService.List(user.Id, geoSTDate.GetValueOrDefault(), geoEDDate.GetValueOrDefault());


                        if (list != null && list.Count() > 0)
                            list = list.Where(x =>
                                                    x.State == AnalyzedReportState.Absence_NoTransaction ||
                                                    x.State == AnalyzedReportState.Holiday_Absence_NoTransaction ||
                                                    x.State == AnalyzedReportState.Absence_Systemic ||
                                                    x.State == AnalyzedReportState.Holiday_Absence_Systemic ||
                                                    x.State == AnalyzedReportState.FractionTrade ||
                                                    x.State == AnalyzedReportState.HolidayFraction)
                                       .ToList();

                        if (list != null && list.Count() > 0)
                        {
                            switch (analyzedReportState)
                            {
                                case 1:
                                    list = list.Where(x => x.State == AnalyzedReportState.Absence_NoTransaction || x.State == AnalyzedReportState.Holiday_Absence_NoTransaction).ToList();
                                    break;
                                case 2:
                                    list = list.Where(x => x.State == AnalyzedReportState.Absence_Systemic || x.State == AnalyzedReportState.Holiday_Absence_Systemic).ToList();
                                    break;
                                case 3:
                                    list = list.Where(x => x.State == AnalyzedReportState.FractionTrade || x.State == AnalyzedReportState.HolidayFraction).ToList();
                                    break;
                                default:
                                    break;
                            }

                            list = list.OrderBy(x => x.Date.Date).ToList();
                        }

                        enttityList.Add(new AnalyzedReportListModel() {
                            StartDate = stDate,
                            EndDate = edDate,
                            UserGroup = user.UserGroup.Title,
                            UserInformattion = $"{user.FirstName} {user.Lastname}",
                            Reports = list
                        });
                    }
                }
              
            }
            catch (Exception e)
            {
                enttityList = new List<AnalyzedReportListModel>();
            }


            return PartialView("_List", enttityList);
        }
        #endregion


        #region PrepareDrop
        [HttpGet]
        public ActionResult GetUserDrop(int userGroupId = 0)
        {
            List<DropDownModel> modelList = new List<DropDownModel>();
            var userList = _userService.GetList.Where(x => x.UserGroupId == userGroupId).ToList();
            if (userList != null && userList.Count() > 0)
            {
                foreach (var user in userList)
                {
                    DropDownModel model = new DropDownModel()
                    {
                        Value = user.Id,
                        Text = $"{user.FirstName} {user.Lastname}"
                    };
                    modelList.Add(model);
                }
            }
            else
            {
                foreach (var user in userList)
                {
                    DropDownModel model = new DropDownModel()
                    {
                        Value = "0",
                        Text = $"کاربری در این گروه وجود ندارد"
                    };
                    modelList.Add(model);
                }
            }
            return PartialView("_GetUserDrop", modelList);
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
    }
}